using BusinessLogic.Filtering;
using BusinessLogic.ViewModels.Business;
using FluentResults;

namespace BusinessLogic.Abstractions
{
    public interface IBusinessService
    {
        public Task<Result<BusinessViewModel>> CreateAsync(BusinessCreateModel model);

        public Task<Result<BusinessViewModel>> UpdateAsync(string userId, BusinessUpdateModel model);

        public Task<Result<int>> DeleteAsync(string userId, int id);

        public Task<Result<IEnumerable<BusinessViewModel>>> GetAllBusinessesAsync(string userId, BusinessFilter filter);

        public Task<Result<BusinessViewModel>> GetUserBusinessAsync(string userId);
    }
}
