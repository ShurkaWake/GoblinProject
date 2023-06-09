using BusinessLogic.ViewModels.AppUser;
using FluentResults;

namespace BusinessLogic.Abstractions
{
    public interface IAuthService
    {
        Task<Result<UserAuthModel>> LoginAsync(UserLoginModel model);
    }
}
