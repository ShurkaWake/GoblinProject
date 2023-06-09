using AutoMapper;
using BusinessLogic.ViewModels.AppUser;
using WebApi.Requests.Auth;

namespace WebApi.Mapping
{
    public class ApiProfile : Profile
    {
        public ApiProfile() 
        {
            CreateMap<LoginRequest, UserLoginModel>();
        }
    }
}
