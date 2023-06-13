﻿using BusinessLogic.Filtering;
using BusinessLogic.ViewModels.WorkingShift;
using FluentResults;

namespace BusinessLogic.Abstractions
{
    public interface IWorkingShiftService
    {
        public Task<Result<WorkingShiftViewModel>> CreateWorkingShiftAsync(
            string userId, 
            WorkingShiftCreateModel model
            );
        
        public Task<Result<WorkingShiftViewModel>> GetWorkingShiftAsync(
            string userId, 
            int id
            );

        public Task<Result<IEnumerable<WorkingShiftViewModel>>> GetAllWorkingShiftAsync(
            string userId,
            WorkingShiftFilter filter
            );

        public Task<Result> EndWorkingShiftAsync(string userId, int shiftId);

        public Task<Result> AddResourceAsync(string userId, int shiftId, int resourceId);

        public Task<Result> DeleteResourceAsync(string userId, int shiftId, int resourceId);

        public Task<Result<int>> DeleteWorkingShiftAsync(string userId, int shiftId);
    }
}
