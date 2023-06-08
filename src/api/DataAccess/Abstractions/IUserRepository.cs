using DataAccess.Entities;

namespace DataAccess.Abstractions
{
    public interface IUserRepository : IRepository<AppUser, string>
    {
        public Task<AppUser> GetUserIncludingJob(string id);
    }
}
