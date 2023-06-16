using BusinessLogic.Filtering;
using BusinessLogic.ViewModels.General;
using BusinessLogic.ViewModels.Scales;
using FluentResults;

namespace BusinessLogic.Abstractions
{
    public interface IScalesService
    {
        public Task<Result<ScalesViewModel>> CreateScalesAsync(string userId, ScalesCreateModel model);

        public Task<Result<ScalesViewModel>> GetScalesAsync(string userId, int scalesId);

        public Task<PaginationViewModel<ScalesViewModel>> GetAllScalesAsync(string userId, ScalesFilter filter);

        public Task<Result<int>> DeleteScalesAsync(string userId, int scalesId);
    }
}
