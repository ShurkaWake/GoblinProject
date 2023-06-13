using AutoFilterer.Extensions;
using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Filtering;
using BusinessLogic.Validators.WorkingShift;
using BusinessLogic.ViewModels.WorkingShift;
using DataAccess.Abstractions;
using DataAccess.Entities;
using DataAccess.Enums;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;

namespace BusinessLogic.Services
{
    public class WorkingShiftService : IWorkingShiftService
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public WorkingShiftService(
            IBusinessRepository businessRepository, 
            IUserRepository userRepository, 
            UserManager<AppUser> userManager, 
            IMapper mapper)
        {
            _businessRepository = businessRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Result> AddResourceAsync(string userId, int workingShiftId, int resourceId)
        {
            var job = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (job is null)
            {
                return Result.Fail("User not found");
            }

            var resource = job.Resources.FirstOrDefault(x => x.Id == resourceId);
            if (resource is null)
            {
                return Result.Fail("Resource not found");
            }
            else if (resource.Status == ResourceStatus.InUse)
            {
                return Result.Fail("Resource already in use");
            }
            
            var workingShift = job.WorkingShifts.FirstOrDefault(x => x.Id == workingShiftId);
            if (workingShift is null)
            {
                return Result.Fail("Working shift not found");
            }
            else if (workingShift.End is not null)
            {
                return Result.Fail("Working shift ended");
            }

            workingShift.UsedResources.Add(resource);
            resource.Status = ResourceStatus.InUse;

            return (await _businessRepository.ConfirmAsync()) > 0
                ? Result.Ok()
                : Result.Fail("Failed to save resource");
        }

        public async Task<Result<WorkingShiftViewModel>> CreateWorkingShiftAsync(
            string userId, 
            WorkingShiftCreateModel model
            )
        {
            var validator = new WorkingShiftCreateValidator();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid is false)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            var user = await _userRepository.GetUserIncludingJob(userId);
            if (user == null)
            {
                return Result.Fail("User not found");
            }

            var isForeman = await IsForemanAndWorkerOf(model.ForemanId, user.Job.Id);
            if (isForeman.IsFailed)
            {
                return Result.Fail(isForeman.Errors);
            }

            var foreman = await _userRepository.GetAsync(userId);
            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            var workingShift = new WorkingShift()
            {
                Business = user.Job,
                Foreman = foreman,
            };

            business.WorkingShifts.Add(workingShift);
            return (await _businessRepository.ConfirmAsync()) > 0
                ? Result.Ok()
                : Result.Fail("Failed to save working shift");
        }

        public async Task<Result> DeleteResourceAsync(string userId, int workingShiftId, int resourceId)
        {
            var job = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (job is null)
            {
                return Result.Fail("User not found");
            }

            var resource = job.Resources.FirstOrDefault(x => x.Id == resourceId);
            if (resource is null)
            {
                return Result.Fail("Resource not found");
            }

            var workingShift = job.WorkingShifts.FirstOrDefault(x => x.Id == workingShiftId);
            if (workingShift is null)
            {
                return Result.Fail("Working shift not found");
            }
            else if (workingShift.End is not null)
            {
                return Result.Fail("Working shift ended");
            }

            workingShift.UsedResources.Remove(resource);
            resource.Status = ResourceStatus.Free;

            return (await _businessRepository.ConfirmAsync()) > 0
                ? Result.Ok()
                : Result.Fail("Failed to save resource");
        }

        public async Task<Result<int>> DeleteWorkingShiftAsync(string userId, int shiftId)
        {
            var job = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (job is null)
            {
                return Result.Fail("User not found");
            }

            var workingShift = job.WorkingShifts.FirstOrDefault(x => x.Id == shiftId);
            if (workingShift is null)
            {
                return Result.Fail("Working shift not found");
            }

            job.WorkingShifts.Remove(workingShift);
            return (await _businessRepository.ConfirmAsync()) > 0
                ? Result.Ok()
                : Result.Fail("Failed to delete working shift");
        }

        public async Task<Result> EndWorkingShiftAsync(string userId, int shiftId)
        {
            var job = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (job is null)
            {
                return Result.Fail("User not found");
            }

            var workingShift = job.WorkingShifts.FirstOrDefault(x => x.Id == shiftId);
            if (workingShift is null)
            {
                return Result.Fail("Working shift not found");
            }

            workingShift.End = DateTime.Now;
            return (await _businessRepository.ConfirmAsync()) > 0
                ? Result.Ok()
                : Result.Fail("Failed to end working shift");
        }

        public async Task<Result<IEnumerable<WorkingShiftViewModel>>> GetAllWorkingShiftAsync(
            string userId, 
            WorkingShiftFilter filter
            )
        {
            var job = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (job is null)
            {
                return Result.Fail("User not found");
            }

            var workingShift = job.WorkingShifts.AsQueryable().ApplyFilter(filter);

            return Result.Ok(_mapper.Map<IEnumerable<WorkingShift>, IEnumerable<WorkingShiftViewModel>>(workingShift));
        }

        public async Task<Result<WorkingShiftViewModel>> GetWorkingShiftAsync(string userId, int shiftId)
        {
            var job = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (job is null)
            {
                return Result.Fail("User not found");
            }

            var workingShift = job.WorkingShifts.FirstOrDefault(x => x.Id == shiftId);
            if (workingShift is null)
            {
                return Result.Fail("Working shift not found");
            }

            return Result.Ok(_mapper.Map<WorkingShiftViewModel>(workingShift));
        }

        private async Task<Result> IsForemanAndWorkerOf(string userId, int businessId)
        {
            var user = await _userRepository.GetUserIncludingJob(userId);
            if (user is null)
            {
                return Result.Fail("User not found");
            }

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(x => x == Roles.Foreman);
            if(role is null)
            {
                return Result.Fail("User is not foreman");
            }

            return businessId == user.Job.Id
                ? Result.Ok()
                : Result.Fail("Foreman is not worker of your business");
        }
    }
}
