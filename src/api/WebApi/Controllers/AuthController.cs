using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.ViewModels.AppUser;
using GenericWebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Requests.Auth;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(_mapper.Map<UserLoginModel>(request));

            return result.ToObjectResponse();
        }
    }
}
