using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.Business;

namespace WebApi.Requests.Business
{
    public record BusinessCreateRequest
    {
        public BusinessCreateModel BusinessCreateModel { get; set; }

        public UserCreateModel UserCreateModel { get; set; }
    }
}
