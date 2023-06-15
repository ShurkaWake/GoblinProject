using AutoFilterer.Extensions;
using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Filtering;
using BusinessLogic.Validators.Scales;
using BusinessLogic.ViewModels.Scales;
using DataAccess.Abstractions;
using DataAccess.Entities;
using FluentResults;

namespace BusinessLogic.Services
{
    public class ScalesService : IScalesService
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IMapper _mapper;

        public ScalesService(IBusinessRepository businessRepository, IMapper mapper)
        {
            _businessRepository = businessRepository;
            _mapper = mapper;
        }

        public async Task<Result<ScalesViewModel>> CreateScalesAsync(string userId, ScalesCreateModel model)
        {
            var validator = new ScalesCreateValidator();
            var validationResult = validator.Validate(model);
            if (validationResult.IsValid is false)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (business == null)
            {
                return Result.Fail("User not found");
            }
            var duplicate = business.Scales.FirstOrDefault(x => x.SerialNumber == model.SerialNumber);
            if (duplicate is not null)
            {
                return Result.Fail("Scales with this Serial Number already exists");
            }


            var scales = _mapper.Map<Scales>(model);
            business.Scales.Add(scales);

            return (await _businessRepository.ConfirmAsync()) > 0
                ? Result.Ok(_mapper.Map<ScalesViewModel>(scales))
                : Result.Fail("Failed to add scales");
        }

        public async Task<Result<int>> DeleteScalesAsync(string userId, int scalesId)
        {
            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (business == null)
            {
                return Result.Fail("User not found");
            }

            var scales = business.Scales.FirstOrDefault(x => x.Id == scalesId);
            if (scales == null)
            {
                return Result.Fail("Scales not found");
            }

            business.Scales.Remove(scales);
            return (await _businessRepository.ConfirmAsync()) > 0
                ? Result.Ok(scales.Id)
                : Result.Fail("Failed to delete scales");
        }

        public async Task<Result<IEnumerable<ScalesViewModel>>> GetAllScalesAsync(string userId, ScalesFilter filter)
        {
            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (business == null)
            {
                return Result.Fail("User not found");
            }

            var scales = business.Scales.AsQueryable().ApplyFilter(filter);
            return Result.Ok(_mapper.Map<IEnumerable<Scales>, IEnumerable<ScalesViewModel>>(scales));
        }

        public async Task<Result<ScalesViewModel>> GetScalesAsync(string userId, int scalesId)
        {
            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (business == null)
            {
                return Result.Fail("User not found");
            }

            var scales = business.Scales.FirstOrDefault(x => x.Id == scalesId);
            return scales is not null
                ? Result.Ok(_mapper.Map<ScalesViewModel>(scales))
                : Result.Fail("Scales not found");
        }
    }
}
