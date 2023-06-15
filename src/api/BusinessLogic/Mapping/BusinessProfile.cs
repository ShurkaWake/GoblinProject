using AutoMapper;
using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.Business;
using BusinessLogic.ViewModels.Measurement;
using BusinessLogic.ViewModels.MoneyAmount;
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
            CreateMap<UserCreateModel, AppUser>();

            CreateMap<BusinessCreateModel, Business>();
            CreateMap<Business, BusinessViewModel>();
            CreateMap<Business, BusinessUpdateModel>();

            CreateMap<Resource, ResourceViewModel>();
            CreateMap<ResourceCreateModel, Resource>();
            CreateMap<ResourceUpdateModel, Resource>();

            CreateMap<MoneyAmountCreateModel, MoneyAmount>();
            CreateMap<MoneyAmount, MoneyAmountViewModel>();

            CreateMap<WorkingShift, WorkingShiftViewModel>();
            CreateMap<WorkingShift, WorkingShiftForIot>();

            CreateMap<Scales, ScalesViewModel>();
            CreateMap<ScalesCreateModel, Scales>();

            CreateMap<Measurement, MeasurementViewModel>();
        }
    }
}
