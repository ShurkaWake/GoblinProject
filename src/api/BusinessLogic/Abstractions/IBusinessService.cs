﻿using BusinessLogic.Filtering;
using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.Business;
using BusinessLogic.ViewModels.General;
using FluentResults;

namespace BusinessLogic.Abstractions
{
    public interface IBusinessService
    {
        public Task<Result<BusinessViewModel>> CreateAsync(BusinessCreateModel model);

        public Task<Result<BusinessViewModel>> UpdateAsync(int id, BusinessUpdateModel model);

        public Task<Result<int>> DeleteAsync(int id);

        public Task<PaginationViewModel<BusinessViewModel>> GetAllBusinessesAsync(BusinessFilter filter);

        public Task<Result<BusinessViewModel>> GetUserBusinessAsync(string userId);
    }
}
