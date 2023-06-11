﻿using BusinessLogic.Filtering;
using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.Business;
using FluentResults;

namespace BusinessLogic.Abstractions
{
    public interface IUserService
    {
        public Task<Result<string>> CreateAsync(int businessId, string role, UserCreateModel model);

        public Task<Result> UpdateAsync(string userId, UserUpdateModel model);

        public Task<Result<string>> DeleteAsync(string userId);

        public Task<Result<IEnumerable<UserViewModel>>> GetAllUsersAsync(UserFilter filter);

        public Task<Result> ChangePasswordAsync(string userId, UserChangePasswordModel model);
    }
}