using BusinessLogic.Enums;
using BusinessLogic.ViewModels.Measurement;
using FluentResults;

namespace BusinessLogic.Abstractions
{
    public interface IMeasurementService
    {
        public Task<Result<MeasurementViewModel>> AddMeasurementAsync(string scalesSerialNumber, MeasurementCreateModel model);

        public Task<Result<int>> DeleteMeasurementAsync(string userId, int workingShiftId);

        public Task<Result<MeasurementViewModel>> GetMeasurementAsync(string userId, int workingShiftId, WeightUnits unit);
    }
}
