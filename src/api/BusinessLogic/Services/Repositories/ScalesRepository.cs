using BusinessLogic.Abstractions;
using DataAccess;
using DataAccess.Abstractions;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services.Repositories
{
    public class ScalesRepository : Repository<Scales, int>, IScalesRepository
    {
        public ScalesRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<Scales> GetScalesBySerialNumberIncludingAll(string serialNumber)
        {
            return Context.Scales
                .Include(x => x.Measurements)
                .Include(x => x.Business)
                .FirstOrDefault(s => s.SerialNumber == serialNumber);
        }
    }
}
