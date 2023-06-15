using BusinessLogic.Abstractions;
using DataAccess.Entities;
using DataAccess.Abstractions;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services.Repositories
{
    public sealed class BusinessRepository : Repository<Business, int>, IBusinessRepository
    {
        private readonly IUserRepository _userRepository;

        public BusinessRepository(ApplicationContext context, IUserRepository userRepository) 
            : base(context) 
        {
            _userRepository = userRepository;
        }

        public DbSet<Business> Businesses => Context.Businesses; 

        public Task<Business> GetBusinessIncludingAll(int id)
        {
            return Businesses
                .Include(x => x.Users)
                .Include(x => x.Scales)
                .Include(x => x.Resources)
                .ThenInclude(x => x.Ammortization)
                .Include(x => x.WorkingShifts)
                .ThenInclude(x => x.UsedResources)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Business> GetUserBusinessIncludingAll(string userId)
        {
            var user = await _userRepository.GetUserIncludingJob(userId);
            if (user == null || user.Job == null)
            {
                return null;
            }

            return await GetBusinessIncludingAll(user.Job.Id);
        }
    }
}
