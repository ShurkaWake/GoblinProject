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
    [Route("api/business")]
    [Authorize]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly IUserService _userService;
        private readonly IResourceService _resourceService;
        private readonly UserManager<AppUser> _userManager;

        public BusinessController(
            IBusinessService businessService, 
            IUserService userService,
            UserManager<AppUser> userManager,
            IResourceService resourceService)
        {
            _businessService = businessService;
            _userService = userService;
            _userManager = userManager;
            _resourceService = resourceService;
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

            return CreatedAtAction("CreateBusiness", businessResult.Value);
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
            return result.ToObjectResponse();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteBusinessAsync([FromRoute] int id)
        {
            var result = await _businessService.DeleteAsync(id);
            return result.ToObjectResponse();
        }

        [HttpGet("resources")]
        public async Task<IActionResult> GetBusinessResourcesAsync([FromQuery] ResourceFilter filter)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _resourceService.GetAllBusinessResourcesAsync(user.Id, filter);
            return result.ToObjectResponse();
        }
    }
}
