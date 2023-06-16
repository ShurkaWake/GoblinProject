using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Filtering;
using BusinessLogic.ViewModels.Resource;
using DataAccess.Entities;
using GenericWebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/resource")]
    [Authorize]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService _resourceService;
        private readonly UserManager<AppUser> _userManager;

        public ResourceController(IResourceService resourceService, UserManager<AppUser> userManager)
        {
            _resourceService = resourceService;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.Manager},{Roles.Owner}")]
        public async Task<IActionResult> CreateResourceAsync(ResourceCreateModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _resourceService.CreateResourceAsync(user.Id, model);

            if (result.IsFailed)
            {
                result.ToObjectResponse();
            }

            return CreatedAtAction("CreateResource", result.Value);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetResourceAsync([FromRoute] int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _resourceService.GetResourceAsync(user.Id, id);
            return result.ToObjectResponse();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllResourcesAsync([FromQuery] ResourceFilter filter)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _resourceService.GetAllBusinessResourcesAsync(user.Id, filter);
            return result.ToObjectResponse();
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = $"{Roles.Manager},{Roles.Owner}")]
        public async Task<IActionResult> UpdateResourceAsync(
            [FromRoute] int id, 
            [FromBody] ResourceUpdateModel model
            )
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _resourceService.UpdateResourceAsync(user.Id, id, model);
            return result.ToNoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{Roles.Manager},{Roles.Owner}")]
        public async Task<IActionResult> DeleteResourceAsync([FromRoute] int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _resourceService.DeleteResourceAsync(user.Id, id);
            return result.ToObjectResponse();
        }
    }
}
