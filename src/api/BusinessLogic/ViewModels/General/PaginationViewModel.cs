using FluentResults;

namespace BusinessLogic.ViewModels.General
{
    public class PaginationViewModel<TViewModel>
    {
        public Result<IEnumerable<TViewModel>> Data { get; set; }

        public int PageCount { get; set; }
    }
}
