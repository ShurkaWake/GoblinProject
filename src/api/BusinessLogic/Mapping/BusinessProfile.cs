﻿using AutoMapper;
using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.Business;
using BusinessLogic.ViewModels.Resource;
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
        }
    }
}
