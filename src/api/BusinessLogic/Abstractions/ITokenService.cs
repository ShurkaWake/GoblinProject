using DataAccess.Entities;
using FluentResults;

namespace BusinessLogic.Abstractions
{
    public interface ITokenService
    {
        public Task<Result<string>> CreateTokenAsync(AppUser user);
    }
}
