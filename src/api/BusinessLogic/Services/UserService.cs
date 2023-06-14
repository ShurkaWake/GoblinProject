using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Filtering;
using BusinessLogic.Validators.AppUser;
using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.Business;
using DataAccess.Abstractions;
using DataAccess.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using PasswordGenerator;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
            IUserRepository userRepository, 
            IBusinessRepository businessRepository, 
            IEmailService emailService, 
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _businessRepository = businessRepository;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<Result> ChangePasswordAsync(string userId, UserChangePasswordModel model)
        {
            var validator = new ChangePasswordValidator();
            var validatorResult = validator.Validate(model);
            if (validatorResult.IsValid is false)
            {
                return Result.Fail(validatorResult.Errors.Select(x => x.ErrorMessage));
            }

            var user = await _userRepository.GetAsync(userId);
            if (user is null)
            {
                return Result.Fail("User not found");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (changePasswordResult.Succeeded is false)
            {
                return Result.Fail("Old password is wrong");
            }

            await _signInManager.RefreshSignInAsync(user);
            return Result.Ok();
        }

        public async Task<Result<string>> CreateAsync(int businessId, string role, UserCreateModel model)
        {
            var validator = new UserCreateValidator();
            var validationResult = await validator.ValidateAsync(model);
            if(validationResult.IsValid is false)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            var existingUsers = await _userRepository.FindAsync(u => u.Email == model.Email);
            var existingUser = existingUsers.FirstOrDefault();
            if (existingUser is not null) 
            {
                return Result.Fail("User with this Email already exists");
            }

            var business = await _businessRepository.GetAsync(businessId);
            if (business is null)
            {
                return Result.Fail("Business not found");
            }

            var user = _mapper.Map<AppUser>(model);
            user.UserName = model.Email;
            user.Job = business;

            var password = new Password(20)
                .IncludeSpecial()
                .IncludeLowercase()
                .IncludeUppercase()
                .IncludeNumeric()
                .Next();

            var createResult = 
                (await _userManager.CreateAsync(user, $"${password}")).Succeeded
                && (await _userManager.AddToRoleAsync(user, role)).Succeeded;

            if (createResult is false)
            {
                return Result.Fail("Failed to create user");
            }

            var emailResult = await _emailService.SendEmailAsync(
                user.Email,
                "Registration on Goblin Project",
                "You successfully registrated on Goblin Project. Your login credentials:" +
                $"\nEmail: {user.Email}" +
                $"\nPassword: ${password}");

            if (emailResult.IsFailed)
            {
                await _userManager.DeleteAsync(user);
                Result.Fail("Failed to send registration email. User not created");
            }

            return Result.Ok(user.Id);
        }

        public async Task<Result<string>> DeleteAsync(string userId)
        {
            var user = await _userRepository.GetUserIncludingJob(userId);
            if (user is null)
            {
                return Result.Fail("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(Roles.Owner))
            {
                _businessRepository.Remove(user.Job);
                return (await _businessRepository.ConfirmAsync()) > 0
                    ? Result.Ok(user.Id)
                    : Result.Fail("Failed to delete User");
            }

            _userRepository.Remove(user);
            return (await _userRepository.ConfirmAsync()) > 0
                ? Result.Ok(user.Id)
                : Result.Fail("Failed to delete User");
        }

        public async Task<Result<IEnumerable<UserViewModel>>> GetAllUsersAsync(UserFilter filter)
        {
            IEnumerable<AppUser> users;
            try
            {
                users = await _userRepository.GetAllAsync(filter);
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }

            var response = _mapper.Map<IEnumerable<AppUser>, IEnumerable<UserViewModel>>(users);
            return Result.Ok(response);
        }

        public async Task<Result> UpdateAsync(string userId, UserUpdateModel model)
        {
            var validator = new UserUpdateValidator();
            var validatorResult = validator.Validate(model);

            if (validatorResult.IsValid is false)
            {
                return Result.Fail(validatorResult.Errors.Select(x => x.ErrorMessage));
            }

            var user = await _userRepository.GetAsync(userId);
            if (user is null)
            {
                return Result.Fail("User not Found");
            }
            user.FullName = model.FullName;

            _userRepository.Update(user);

            return (await _userRepository.ConfirmAsync()) > 0
                ? Result.Ok()
                : Result.Fail("Failed to save User");
        }
    }
}
