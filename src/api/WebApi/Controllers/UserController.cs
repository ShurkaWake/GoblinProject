using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Filtering;
using BusinessLogic.ViewModels.AppUser;
using DataAccess.Entities;
using FluentResults;
using GenericWebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extenstions;

namespace WebApi.Controllers
{
    [Route("api/user")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IBusinessService _businessService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(
            IUserService userService, 
            IBusinessService businessService,
            UserManager<AppUser> userManager,
            IMapper mapper)
        {
            _userService = userService;
            _businessService = businessService;
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
            var response = _mapper.Map<UserViewModel>(user);
            return Result.Ok(response).ToObjectResponse();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewWorkerAsync([FromBody] UserCreateModel model, [FromQuery] string role)
        {
            if (role == Roles.Admin || role == Roles.Owner)
            {
                return Result.Fail("Unable to create worker of this role").ToNoContent();
            }
            if (Roles.AllowedRoles.Contains(role) is false)
            {
                return Result.Fail("There is no role with this name").ToNoContent();
            }

            var user = await _userManager.GetUserAsync(User);
            var business = await _businessService.GetUserBusinessAsync(user.Id);

            var result = await _userService.CreateAsync(business.Value.Id, role, model);

            if (result.IsFailed)
            {
                return result.ToObjectResponse();
            }

            return CreatedAtAction("CreateNewWorker", result.Value);
        }
    }
}
