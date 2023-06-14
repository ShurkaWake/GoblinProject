using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Filtering;
using BusinessLogic.ViewModels.AppUser;
using DataAccess.Entities;
using GenericWebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extenstions;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(
            IUserService userService, 
            UserManager<AppUser> userManager,
            IMapper mapper)
        {
            _userService = userService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("all")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] UserFilter filter)
        {
            var result = await _userService.GetAllUsersAsync(filter);
            return result.ToObjectResponse(filter);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string id)
        {
            var result = await _userService.DeleteAsync(id);
            return result.ToObjectResponse();
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserUpdateModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _userService.UpdateAsync(user.Id, model);
            return result.ToNoContent();
        }

        [HttpPatch("password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] UserChangePasswordModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _userService.ChangePasswordAsync(user.Id, model);
            return result.ToNoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            return Ok(_mapper.Map<UserViewModel>(user));
        }
    }
}
