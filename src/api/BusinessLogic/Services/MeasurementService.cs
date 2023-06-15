using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Enums;
using BusinessLogic.ViewModels.Measurement;
using DataAccess.Abstractions;
using DataAccess.Entities;
using FluentResults;

namespace BusinessLogic.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IScalesRepository _scalesRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IExchangeService _exchangeService;
        private readonly IMapper _mapper;

        public MeasurementService(
            IScalesRepository scalesRepository, 
            IBusinessRepository businessRepository, 
            IExchangeService exchangeService, 
            IMapper mapper)
        {
            _scalesRepository = scalesRepository;
            _businessRepository = businessRepository;
            _exchangeService = exchangeService;
            _mapper = mapper;
        }

        public async Task<Result<MeasurementViewModel>> AddMeasurementAsync(string scalesSerialNumber, MeasurementCreateModel model)
        {
            var scales = await _scalesRepository.GetScalesBySerialNumberIncludingAll(scalesSerialNumber);
            if (scales is null)
            {
                return Result.Fail("Scales not found");
            }

            var business = await _businessRepository.GetBusinessIncludingAll(scales.Business.Id);
            var workingShift = business.WorkingShifts.FirstOrDefault(x => x.Id == model.ShiftId);

            var measurement = new Measurement()
            {
                Scales = scales,
                Weight = model.Units == WeightUnits.Oz
                    ? model.Weight
                    : _exchangeService.GramsToOz(model.Weight).Value,
            };
            workingShift.Measurement = measurement;

            return (await _businessRepository.ConfirmAsync()) > 0
                ? Result.Ok(_mapper.Map<MeasurementViewModel>(measurement))
                : Result.Fail("Unable to add measurement");
        }

        public async Task<Result<int>> DeleteMeasurementAsync(string userId, int workingShiftId)
        {
            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (business is null)
            {
                return Result.Fail("Unable to find a user");
            }

            var workingShift = business.WorkingShifts.FirstOrDefault(x => x.Id == workingShiftId);
            if (workingShift is null)
            {
                return Result.Fail("Unable to find WorkingShift");
            }

            var measurement = workingShift.Measurement;
            if(measurement is null)
            {
                return Result.Fail("There is no measurement for this working shift");
            }

            workingShift.Measurement = null;
            return (await _businessRepository.ConfirmAsync()) > 0
                ? Result.Ok(measurement.Id)
                : Result.Fail("Unable to delete measurement");
        }

        public async Task<Result<MeasurementViewModel>> GetMeasurementAsync(string userId, int workingShiftId, WeightUnits unit)
        {
            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (business is null)
            {
                return Result.Fail("Unable to find a user");
            }

            var workingShift = business.WorkingShifts.FirstOrDefault(x => x.Id == workingShiftId);
            if (workingShift is null)
            {
                return Result.Fail("Unable to find WorkingShift");
            }

            var measurement = workingShift.Measurement;
            if (measurement is null)
            {
                return Result.Fail("There is no measurement for this working shift");
            }

            var response = _mapper.Map<MeasurementViewModel>(measurement);
            if (unit == WeightUnits.Grams)
            {
                response.Weight = _exchangeService.OzToGrams(measurement.Weight).Value;
            }

            return Result.Ok(response);
        }
    }
}
