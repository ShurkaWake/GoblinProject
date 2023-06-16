using BusinessLogic.Filtering;
using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.Business;
using BusinessLogic.ViewModels.General;
using FluentResults;

namespace BusinessLogic.Abstractions
{
    public interface IUserService
    {
        public Task<Result<string>> CreateAsync(int businessId, string role, UserCreateModel model);

        public Task<Result> UpdateAsync(string userId, UserUpdateModel model);

        public Task<Result<string>> DeleteAsync(string userId);

        public Task<PaginationViewModel<UserViewModel>> GetAllUsersAsync(UserFilter filter);

        public Task<Result> ChangePasswordAsync(string userId, UserChangePasswordModel model);

        public Task<PaginationViewModel<UserViewModel>> GetAllWorkersAsync(string userId, UserFilter filter);

        public Task<Result<string>> FireWorker(string userId, string workerId);
    }
}
