using AutoMapper;
using BusinessLogic.ViewModels.AppUser;
using DataAccess.Entities;

namespace BusinessLogic.Mapping
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile() 
        {
            CreateMap<AppUser, UserAuthModel>();
            CreateMap<AppUser, UserViewModel>();
        }
    }
}
