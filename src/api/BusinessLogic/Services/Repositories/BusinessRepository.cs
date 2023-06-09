using BusinessLogic.Abstractions;
using DataAccess.Entities;
using DataAccess.Abstractions;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services.Repositories
{
    public sealed class BusinessRepository : Repository<Business, int>, IBusinessRepository
    {
        public BusinessRepository(ApplicationContext context) : base(context) { }

        public DbSet<Business> Businesses => Context.Businesses; 

        public Task<Business> GetBusinessIncludingAll(int id)
        {
            return Businesses
                .Include(x => x.Users)
                .Include(x => x.Scales)
                .Include(x => x.Resources)
                .Include(x => x.WorkingShifts)
                .ThenInclude(x => x.UsedResources)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
