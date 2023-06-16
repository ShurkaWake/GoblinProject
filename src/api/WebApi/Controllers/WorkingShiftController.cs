using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Enums;
using BusinessLogic.Filtering;
using BusinessLogic.Services;
using BusinessLogic.ViewModels.WorkingShift;
using DataAccess.Entities;
using GenericWebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/shift")]
    [Authorize]
    [ApiController]
    public class WorkingShiftController : ControllerBase
    {
        private readonly IWorkingShiftService _workingShiftService;
        private readonly IMeasurementService _measurementService;
        private readonly UserManager<AppUser> _userManager;

        public WorkingShiftController(
            IWorkingShiftService workingShiftService, 
            UserManager<AppUser> userManager,
            IMeasurementService measurementService)
        {
            _workingShiftService = workingShiftService;
            _measurementService = measurementService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkingShiftAsync([FromBody] WorkingShiftCreateModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _workingShiftService.CreateWorkingShiftAsync(user.Id, model);
            
            if (result.IsFailed)
            {
                return result.ToObjectResponse();
            }

            return CreatedAtAction("CreateWorkingShift", result.Value);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetWorkingShiftAsync([FromRoute] int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _workingShiftService.GetWorkingShiftAsync(user.Id, id);
            return result.ToObjectResponse();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllWorkingShiftsAsync([FromQuery] WorkingShiftFilter filter)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _workingShiftService.GetAllWorkingShiftAsync(user.Id, filter);
            return result.ToObjectResponse();
        }

        [HttpPatch("{id:int}/end")]
        [Authorize(Roles = $"{Roles.Foreman}")]
        public async Task<IActionResult> EndWorkingShiftAsync([FromRoute] int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _workingShiftService.EndWorkingShiftAsync(user.Id, id);
            return result.ToNoContent();
        }

        [HttpPatch("{shiftId:int}/add/{resourceId:int}")]
        [Authorize(Roles = $"{Roles.Foreman}")]
        public async Task<IActionResult> AddResourceAsync([FromRoute] int shiftId, [FromRoute] int resourceId)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _workingShiftService.AddResourceAsync(user.Id, shiftId, resourceId);
            return result.ToNoContent();
        }

        [HttpPatch("{shiftId:int}/delete/{resourceId:int}")]
        [Authorize(Roles = $"{Roles.Foreman}")]
        public async Task<IActionResult> DeleteResourceAsync([FromRoute] int shiftId, [FromRoute] int resourceId)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _workingShiftService.DeleteResourceAsync(user.Id, shiftId, resourceId);
            return result.ToNoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{Roles.Manager},{Roles.Owner}")]
        public async Task<IActionResult> DeleteWorkingShiftAsync([FromRoute] int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _workingShiftService.DeleteWorkingShiftAsync(user.Id, id);
            return result.ToObjectResponse();
        }

        [HttpGet("{id:int}/resources")]
        public async Task<IActionResult> GetWorkingShiftResourcesAsync([FromRoute] int id, [FromQuery] ResourceFilter filter)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _workingShiftService.GetWorkingShiftResources(user.Id, id, filter);
            return result.ToObjectResponse();
        }

        [HttpGet("{id:int}/measurement")]
        public async Task<IActionResult> GetMeasurementAsync([FromRoute] int id, [FromQuery] WeightUnits units = WeightUnits.Oz)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _measurementService.GetMeasurementAsync(user.Id, id, units);
            return result.ToObjectResponse();
        }

        [HttpDelete("{id:int}/measurement")]
        public async Task<IActionResult> GetMeasurementAsync([FromRoute] int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _measurementService.DeleteMeasurementAsync(user.Id, id);
            return result.ToObjectResponse();
        }
    }
}
