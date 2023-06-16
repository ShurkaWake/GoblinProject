using AutoFilterer.Extensions;
using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Filtering;
using BusinessLogic.Validators.WorkingShift;
using BusinessLogic.ViewModels.General;
using BusinessLogic.ViewModels.Resource;
using BusinessLogic.ViewModels.WorkingShift;
using DataAccess.Abstractions;
using DataAccess.Entities;
using DataAccess.Enums;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Services
{
    public class WorkingShiftService : IWorkingShiftService
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IUserRepository _userRepository;
        private readonly IScalesRepository _scalesRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;

        public WorkingShiftService(
            IBusinessRepository businessRepository, 
            IUserRepository userRepository, 
            IScalesRepository scalesRepository,
            UserManager<AppUser> userManager, 
            IMapper mapper,
            IHashService hashService)
        {
            _businessRepository = businessRepository;
            _userRepository = userRepository;
            _scalesRepository = scalesRepository;
            _userManager = userManager;
            _mapper = mapper;
            _hashService = hashService;
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
            var response = _mapper.Map<WorkingShiftViewModel>(workingShift);
            response.Hash = _hashService.Hash(workingShift.Id, 6);
            return (await _businessRepository.ConfirmAsync()) > 0
                ? Result.Ok(response)
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
            foreach (var resource in workingShift.UsedResources)
            {
                resource.Status = ResourceStatus.Free;
            }

            return (await _businessRepository.ConfirmAsync()) > 0
                ? Result.Ok()
                : Result.Fail("Failed to end working shift");
        }

        public async Task<PaginationViewModel<WorkingShiftViewModel>> GetAllWorkingShiftAsync(
            string userId, 
            WorkingShiftFilter filter
            )
        {
            var job = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (job is null)
            {
                return new PaginationViewModel<WorkingShiftViewModel>()
                {
                    Data = Result.Fail("User not found"),
                    PageCount = 1
                };
            }

            var perPage = filter.PerPage;
            var workingShift = job.WorkingShifts.AsQueryable().ApplyFilter(filter);
            var response = _mapper.Map<IEnumerable<WorkingShift>, IEnumerable<WorkingShiftViewModel>>(workingShift);
            foreach (var item in response)
            {
                item.Hash = _hashService.Hash(item.Id, 6);
            }

            filter.Page = 1;
            filter.PerPage = int.MaxValue;
            var allCount = job.WorkingShifts.AsQueryable().ApplyFilter(filter).Count();
            var pages = (allCount / perPage)
                + (allCount % perPage == 0
                    ? 0
                    : 1);

            return new PaginationViewModel<WorkingShiftViewModel>
            {
                Data = Result.Ok(response),
                PageCount = pages
            };
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
            var response = _mapper.Map<WorkingShiftViewModel>(workingShift);
            response.Hash = _hashService.Hash(workingShift.Id, 6);

            return Result.Ok(response);
        }

        public async Task<Result<IEnumerable<WorkingShiftForIot>>> GetWorkingShiftForScalesAsync(string serialNumber)
        {
            var scales = await _scalesRepository.GetScalesBySerialNumberIncludingAll(serialNumber);
            if (scales is null)
            {
                return Result.Fail("Scales not found");
            }

            var business = await _businessRepository.GetBusinessIncludingAll(scales.Business.Id);
            var workingShifts = business.WorkingShifts;

            var response = _mapper.Map<
                IEnumerable<WorkingShift>, 
                IEnumerable<WorkingShiftForIot>>
                (workingShifts);

            foreach (var workingShift in response) 
            {
                workingShift.Hash = _hashService.Hash(workingShift.Id, 6);
            }

            return Result.Ok(response);
        }

        public async Task<Result<IEnumerable<ResourceViewModel>>> GetWorkingShiftResources(string userId, int shiftId, ResourceFilter filter)
        {
            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (business is null)
            {
                return Result.Fail("User not found");
            }

            var workingShift = business.WorkingShifts.FirstOrDefault(x => x.Id == shiftId);
            if (workingShift is null)
            {
                return Result.Fail("Working shift not found");
            }
            var resources = workingShift.UsedResources.AsQueryable().ApplyFilter(filter);

            var response = _mapper.Map<IEnumerable<Resource>, IEnumerable<ResourceViewModel>>(resources);
            return Result.Ok(response);
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
