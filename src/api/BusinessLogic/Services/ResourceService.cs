using AutoFilterer.Extensions;
using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Filtering;
using BusinessLogic.Validators.Resource;
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

        public async Task<Result<IEnumerable<ResourceViewModel>>> GetAllBusinessResourcesAsync(string userId, ResourceFilter filter)
        {
            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            IEnumerable<Resource> resources = business.Resources.AsQueryable().ApplyFilter(filter);
            var response = _mapper.Map<IEnumerable<Resource>, IEnumerable<ResourceViewModel>>(resources);
            return Result.Ok(response);
        }

        public async Task<Result<ResourceViewModel>> GetResourceAsync(string userId, int id)
        {
            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            var resource = business.Resources.Where(x => x.Id == id).FirstOrDefault();

            return resource is null
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
            resource.Ammortization = model.Ammortization;

            return Result.Ok();
        }
    }
}
