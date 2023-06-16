using AutoFilterer.Types;
using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Filtering;
using BusinessLogic.Validators.Business;
using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.Business;
using BusinessLogic.ViewModels.General;
using DataAccess.Abstractions;
using DataAccess.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly IBusinessRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public BusinessService(
            IBusinessRepository repository,
            IUserRepository userRepository, 
            IMapper mapper
            )
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<BusinessViewModel>> CreateAsync(BusinessCreateModel model)
        {
            var validator = new BusinessCreateValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));
            }
            var business = new Business()
            {
                Name = model.Name,
                Location = model.Location,
            };
            
            try
            {
                await _repository.AddAsync(business);
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }

            var savingResult = await _repository.ConfirmAsync();
            if(savingResult == 0)
            {
                return Result.Fail("Failed to save Business");
            }

            var response = _mapper.Map<BusinessViewModel>(business);
            return Result.Ok(response);
        }

        public async Task<Result<int>> DeleteAsync(int id)
        {
            var business = await _repository.GetAsync(id);
            if (business is null)
            {
                return Result.Fail("Business not found");
            }

            try
            {
                _repository.Remove(business);
                var result = await _repository.ConfirmAsync();
                if (result == 0)
                {
                    return Result.Fail("Failed to delete Business");
                }
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }

            return Result.Ok(business.Id);
        }

        public async Task<PaginationViewModel<BusinessViewModel>> GetAllBusinessesAsync(BusinessFilter filter = null)
        {
            IEnumerable<Business> businesses;
            int perPage = filter.PerPage;
            int pages;
            try
            {
                businesses = (await _repository.GetAllAsync(filter)).ToArray();
                filter.Page = 1;
                filter.PerPage = int.MaxValue;
                pages = (await _repository.GetAllAsync(filter)).Count();
                var responseCount = businesses.Count();
                pages = (pages / perPage)
                    + (pages % perPage == 0
                        ? 0
                        : 1);
            }
            catch (Exception ex)
            {
                return new PaginationViewModel<BusinessViewModel>()
                {
                    Data = Result.Fail(ex.Message),
                    PageCount = 1,
                };
            }

            var response = _mapper.Map<IEnumerable<Business>, IEnumerable<BusinessViewModel>>(businesses);
            return new PaginationViewModel<BusinessViewModel>()
            {
                Data = Result.Ok(response),
                PageCount = pages,
            };
        }

        public async Task<Result<BusinessViewModel>> GetUserBusinessAsync(string userId)
        {
            var user = await _userRepository.GetUserIncludingJob(userId);
            if (user is null)
            {
                return Result.Fail("User not found");
            }

            var response = _mapper.Map<BusinessViewModel>(user.Job);
            return Result.Ok(response);
        }

        public async Task<Result<BusinessViewModel>> UpdateAsync(int id, BusinessUpdateModel model)
        {
            var validator = new BusinessUpdateValidator();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            var business = await _repository.GetAsync(id);
            if (business is null)
            {
                return Result.Fail("Business not found");
            }
            business.Name = model.Name;
            business.Location = model.Location;
            
            try
            {
                _repository.Update(business);
                var result = await _repository.ConfirmAsync();
                if (result == 0)
                {
                    return Result.Fail("Failed to save Business");
                }
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }

            var response = _mapper.Map<BusinessViewModel>(business);
            return Result.Ok(response);
        }
    }
}
