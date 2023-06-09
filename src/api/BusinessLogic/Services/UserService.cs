﻿using AutoFilterer.Extensions;
using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Filtering;
using BusinessLogic.Validators.AppUser;
using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.Business;
using BusinessLogic.ViewModels.General;
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
            user.Role = role;

            var password = new Password(12)
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
                "You successfully registered on Goblin Project. Your login credentials:<br>" +
                $"Email: {user.Email} <br>" +
                $"Password: ${password}");

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

        public async Task<Result<string>> FireWorker(string userId, string workerId)
        {
            if(userId == workerId)
            {
                return Result.Fail("Cannot delete yourself");
            }

            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (business is null)
            {
                return Result.Fail("User not found");
            }

            var worker = business.Users.FirstOrDefault(x => x.Id == workerId);
            if (worker is null)
            {
                return Result.Fail("Worker not found");
            }
            if (worker.Role.ToLower() == Roles.Owner.ToLower())
            {
                return Result.Fail("Cannot delete owner");
            }

            business.Users.Remove(worker);

            return (await _businessRepository.ConfirmAsync()) > 0
                ? Result.Ok(worker.Id)
                : Result.Fail("Unable to delete worker");
        }

        public async Task<PaginationViewModel<UserViewModel>> GetAllUsersAsync(UserFilter filter)
        {
            IEnumerable<AppUser> users;
            var pages = 1;
            var perPage = filter.PerPage;
            try
            {
                users = await _userRepository.GetAllAsync(filter);
                filter.Page = 1;
                filter.PerPage = int.MaxValue;
                pages = (await _userRepository.GetAllAsync(filter)).Count();
                pages = (pages / perPage) 
                    + (pages % perPage == 0
                        ? 0
                        : 1);
            }
            catch (Exception ex)
            {
                return new PaginationViewModel<UserViewModel>
                {
                    Data = Result.Fail(ex.Message),
                    PageCount = 1,
                };  
            }

            var response = _mapper.Map<IEnumerable<AppUser>, IEnumerable<UserViewModel>>(users);

            return new PaginationViewModel<UserViewModel>
            {
                Data = Result.Ok(response),
                PageCount = pages
            };
        }

        public async Task<PaginationViewModel<UserViewModel>> GetAllWorkersAsync(string userId, UserFilter filter)
        {
            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (business is null)
            {
                return new PaginationViewModel<UserViewModel>
                {
                    Data = Result.Fail("User not found"),
                    PageCount = 1
                };
            }

            var perPage = filter.PerPage;
            var workers = business.Users.AsQueryable().ApplyFilter(filter);
            filter.Page = 1;
            filter.PerPage = int.MaxValue;
            var allCount = business.Users.AsQueryable().ApplyFilter(filter).Count();
            var pages = (allCount / perPage) 
                + (allCount % perPage == 0
                    ? 0
                    : 1);

            var response = _mapper.Map<IEnumerable<AppUser>, IEnumerable<UserViewModel>>(workers);
            return new PaginationViewModel<UserViewModel>
            {
                Data = Result.Ok(response),
                PageCount = pages
            };
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
