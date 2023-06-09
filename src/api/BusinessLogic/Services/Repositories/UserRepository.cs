using BusinessLogic.Abstractions;
using DataAccess;
using DataAccess.Abstractions;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services.Repositories
{
    public sealed class UserRepository : Repository<AppUser, string>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context) { }

        private DbSet<AppUser> Users => Context.Users;

        public Task<AppUser> GetUserIncludingJob(string id)
        {
            return Users
                .Include(x => x.Job)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
