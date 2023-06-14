using DataAccess.Entities;

namespace DataAccess.Abstractions
{
    public interface IScalesRepository : IRepository<Scales, int>
    {
        public Task<Scales> GetScalesBySerialNumberIncludingAll(string serialNumber);
    }
}
