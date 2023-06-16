using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Filtering;
using BusinessLogic.ViewModels.Scales;
using DataAccess.Entities;
using GenericWebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/scales")]
    [Authorize(Roles = $"{Roles.Manager},{Roles.Owner}")]
    [ApiController]
    public class ScalesController : ControllerBase
    {
        private readonly IScalesService _scalesService;
        private readonly UserManager<AppUser> _userManager;

        public ScalesController(IScalesService scalesService, UserManager<AppUser> userManager)
        {
            _scalesService = scalesService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateScalesAsync([FromBody] ScalesCreateModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _scalesService.CreateScalesAsync(user.Id, model);

            if (result.IsFailed)
            {
                return result.ToObjectResponse();
            }

            return CreatedAtAction("CreateScales", result.Value);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetScalesAsync([FromRoute] int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _scalesService.GetScalesAsync(user.Id, id);
            return result.ToObjectResponse();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllScalesAsync([FromQuery] ScalesFilter filter)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _scalesService.GetAllScalesAsync(user.Id, filter);
            return result.ToObjectResponse();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteScalesAsync([FromRoute] int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _scalesService.DeleteScalesAsync(user.Id, id);
            return result.ToObjectResponse();
        } 
    }
}
