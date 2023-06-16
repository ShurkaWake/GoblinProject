using AutoFilterer.Extensions;
using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Filtering;
using BusinessLogic.Validators.Resource;
using BusinessLogic.ViewModels.General;
using BusinessLogic.ViewModels.Resource;
using DataAccess.Abstractions;
using DataAccess.Entities;
using FluentResults;
using FluentValidation;

namespace BusinessLogic.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IMapper _mapper;

        public ResourceService(IBusinessRepository businessRepository, IMapper mapper)
        {
            _businessRepository = businessRepository;
            _mapper = mapper;
        }

        public async Task<Result<ResourceViewModel>> CreateResourceAsync(string userId, ResourceCreateModel model)
        {
            var validator = new ResourceCreateValidator();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid is false) 
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            var resource = _mapper.Map<Resource>(model);
            business.Resources.Add(resource);

            return (await _businessRepository.ConfirmAsync()) > 0
                ? Result.Ok(_mapper.Map<ResourceViewModel>(resource))
                : Result.Fail("Failed to save resource");
        }

        public async Task<Result<int>> DeleteResourceAsync(string userId, int id)
        {
            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            var resource = business.Resources.Where(x => x.Id == id).FirstOrDefault();

            if (resource is null)
            {
                return Result.Fail("Resource not found");
            }

            business.Resources.Remove(resource);

            return (await _businessRepository.ConfirmAsync()) > 0
                ? Result.Ok(resource.Id)
                : Result.Fail("Failed to save");
        }

        public async Task<PaginationViewModel<ResourceViewModel>> GetAllBusinessResourcesAsync(string userId, ResourceFilter filter = null)
        {
            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            int perPage = filter.PerPage;
            IEnumerable<Resource> resources = business.Resources.AsQueryable().ApplyFilter(filter);
            filter.Page = 1;
            filter.PerPage = int.MaxValue;
            int count = business.Resources.AsQueryable().ApplyFilter(filter).Count();
            int pages = (count / perPage)
                + (count % perPage == 0
                    ? 0
                    : 1);

            var resourceViewModels = _mapper.Map<IEnumerable<Resource>, IEnumerable<ResourceViewModel>>(resources);
            var response = new PaginationViewModel<ResourceViewModel>()
            {
                Data = Result.Ok(resourceViewModels),
                PageCount = pages
            };

            return response;
        }

        public async Task<Result<ResourceViewModel>> GetResourceAsync(string userId, int id)
        {
            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            var resource = business.Resources.Where(x => x.Id == id).FirstOrDefault();

            return resource is not null
                ? Result.Ok(_mapper.Map<ResourceViewModel>(resource))
                : Result.Fail("Resource not found");
        }

        public async Task<Result> UpdateResourceAsync(string userId, int id, ResourceUpdateModel model)
        {
            var validator = new ResourceUpdateValidator();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid is false)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            var resource = business.Resources.Where(x => x.Id == id).FirstOrDefault();

            resource.Name = model.Name;
            resource.Description = model.Description;
            resource.Ammortization = _mapper.Map<MoneyAmount>(model.Ammortization);

            return (await _businessRepository.ConfirmAsync()) > 0
                ? Result.Ok()
                : Result.Fail("Cannot save resource");
        }
    }
}
