using BusinessLogic.Abstractions;
using BusinessLogic.Enums;
using GenericWebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/iot")]
    public class IotController : ControllerBase
    {
        private readonly IMeasurementService _measurementService;
        private readonly IWorkingShiftService _workingShiftService;

        public IotController(IMeasurementService measurementService, IWorkingShiftService workingShiftService)
        {
            _measurementService = measurementService;
            _workingShiftService = workingShiftService;
        }

        [HttpGet("{serialNumber}")]
        public async Task<IActionResult> GetWorkingShiftsAsync([FromRoute] string serialNumber)
        {
            var result = await _workingShiftService.GetWorkingShiftForScalesAsync(serialNumber);
            return result.ToObjectResponse();
        }

        [HttpPost("{serialNumber}")]
        public async Task<IActionResult> AddMeasurementAsync(
            [FromRoute] string serialNumber, 
            [FromBody] decimal weight, 
            [FromQuery] WeightUnits units = WeightUnits.Oz)
        {
            var result = await _measurementService.AddMeasurementAsync(serialNumber, weight, units);

            if (result.IsFailed)
            {
                return result.ToObjectResponse();
            }

            return CreatedAtAction("AddMeasurement", result.Value);
        }
    }
}
