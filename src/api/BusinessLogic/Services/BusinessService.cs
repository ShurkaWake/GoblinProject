using AutoFilterer.Types;
using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Filtering;
using BusinessLogic.Validators.Business;
using BusinessLogic.ViewModels.Business;
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
            var validator = new CreateValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            var business = _mapper.Map<Business>(model);
            
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

        public async Task<Result<IEnumerable<BusinessViewModel>>> GetAllBusinessesAsync(BusinessFilter filter = null)
        {
            IEnumerable<Business> businesses;
            try
            {
                businesses = await _repository.GetAllAsync(filter);
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }

            var response = _mapper.Map<IEnumerable<Business>, IEnumerable<BusinessViewModel>>(businesses);
            return Result.Ok(response);
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

        public async Task<Result<BusinessViewModel>> UpdateAsync(string userId, BusinessUpdateModel model)
        {
            var validator = new UpdateValidator();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            var user = await _userRepository.GetUserIncludingJob(userId);

            var business = await _repository.GetAsync(user.Job.Id);
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
