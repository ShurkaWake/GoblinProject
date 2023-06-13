using AutoMapper;
using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.Business;
using BusinessLogic.ViewModels.Resource;
using BusinessLogic.ViewModels.Scales;
using BusinessLogic.ViewModels.WorkingShift;
using DataAccess.Entities;

namespace BusinessLogic.Mapping
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile() 
        {
            CreateMap<AppUser, UserAuthModel>();
            CreateMap<AppUser, UserViewModel>();
            CreateMap<AppUser, UserCreateModel>();

            CreateMap<Business, BusinessCreateModel>();
            CreateMap<Business, BusinessViewModel>();
            CreateMap<Business, BusinessUpdateModel>();

            CreateMap<Resource, ResourceViewModel>();
            CreateMap<Resource, ResourceCreateModel>();
            CreateMap<Resource, ResourceUpdateModel>();

            CreateMap<WorkingShift, WorkingShiftViewModel>();

            CreateMap<Scales, ScalesViewModel>();
            CreateMap<Scales, ScalesCreateModel>();
        }
    }
}
