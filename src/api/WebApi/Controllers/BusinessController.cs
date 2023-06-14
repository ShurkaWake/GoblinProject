using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Filtering;
using BusinessLogic.ViewModels.Business;
using DataAccess.Entities;
using GenericWebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Requests.Business;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class BusinessController : Controller
    {
        private readonly IBusinessService _businessService;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;

        public BusinessController(
            IBusinessService businessService, 
            IUserService userService,
            UserManager<AppUser> userManager)
        {
            _businessService = businessService;
            _userService = userService;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateBusinessAsync([FromBody] BusinessCreateRequest model)
        {
            var businessResult = await _businessService.CreateAsync(model.BusinessCreateModel);
            if (businessResult.IsFailed)
            {
                return BadRequest(businessResult.ToErrors());
            }

            var userResult = await _userService.CreateAsync(businessResult.Value.Id, Roles.Owner, model.UserCreateModel);
            if (userResult.IsFailed)
            {
                await _businessService.DeleteAsync(businessResult.Value.Id);
                return BadRequest(userResult.ToErrors());
            }

            return userResult.ToObjectResponse();
        }

        [HttpGet]
        public async Task<IActionResult> GetBusinessAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _businessService.GetUserBusinessAsync(user.Id);
            return result.ToObjectResponse();
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateBusinessAsync([FromRoute] int id, [FromBody] BusinessUpdateModel model)
        {
            var result = await _businessService.UpdateAsync(id, model);
            return result.ToObjectResponse();
        }

        [HttpGet("all")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAllBusinessAsync([FromQuery] BusinessFilter filter)
        {
            var result = await _businessService.GetAllBusinessesAsync(filter);
            return result.ToObjectResponse(filter);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteBusinessAsync([FromRoute] int id)
        {
            var result = await _businessService.DeleteAsync(id);
            return result.ToObjectResponse();
        }
    }
}
