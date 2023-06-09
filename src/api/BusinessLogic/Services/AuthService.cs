using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Validators.AppUser;
using BusinessLogic.ViewModels.AppUser;
using DataAccess.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            IMapper mapper, 
            ITokenService tokenService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<Result<UserAuthModel>> LoginAsync(UserLoginModel model)
        {
            var validator = new LoginValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid) 
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                return Result.Fail($"User with email {model.Email} was not found");
            }

            var passwordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);

            if (passwordCorrect is false)
            {
                return Result.Fail("Wrong password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

            if (result.Succeeded is false)
            {
                return Result.Ok(_mapper.Map<UserAuthModel>(user)).WithError("Unable to log in user");
            }

            return await CreateTokenFor(user);
        }

        private async Task<Result<UserAuthModel>> CreateTokenFor(AppUser user)
        {
            var tokenResult = await _tokenService.CreateTokenAsync(user);

            if (tokenResult.IsFailed)
            {
                return Result.Ok(_mapper.Map<UserAuthModel>(user)).WithErrors(tokenResult.Errors);
            }

            var userViewModel = _mapper.Map<UserAuthModel>(user);
            userViewModel.Token = tokenResult.Value;

            return Result.Ok(userViewModel);
        }
    }
}
