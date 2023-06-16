using BusinessLogic.Filtering;
using BusinessLogic.ViewModels.General;
using BusinessLogic.ViewModels.Resource;
using FluentResults;

namespace BusinessLogic.Abstractions
{
    public interface IResourceService
    {
        public Task<Result<ResourceViewModel>> CreateResourceAsync(string userId, ResourceCreateModel model);

        public Task<Result<ResourceViewModel>> GetResourceAsync(string userId, int id);

        public Task<PaginationViewModel<ResourceViewModel>> GetAllBusinessResourcesAsync(string userId, ResourceFilter filter);

        public Task<Result> UpdateResourceAsync(string userId, int id, ResourceUpdateModel model);

        public Task<Result<int>> DeleteResourceAsync(string userId, int id);
    }
}
