using BusinessLogic.Abstractions;
using BusinessLogic.ViewModels.Statistics;
using DataAccess.Abstractions;
using DataAccess.Entities;
using DataAccess.Enums;
using FluentResults;

namespace BusinessLogic.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IExchangeService _exchangeService;

        public StatisticService(IBusinessRepository businessRepository, IExchangeService exchangeService)
        {
            _businessRepository = businessRepository;
            _exchangeService = exchangeService;
        }

        public async Task<Result<IEnumerable<StatisticViewModel>>> GetWorkingShiftsStatisticsAsync(
            string userId, 
            DateTime from, 
            DateTime to, 
            Currency currency)
        {
            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (business is null)
            {
                return Result.Fail("User not found");
            }
            var workingShifts = business.WorkingShifts.Where(x => x.Start > from && x.Start < to).OrderBy(x => x.Start);
            IEnumerable<string> errors = new List<string>();
            IEnumerable<StatisticViewModel> response = new List<StatisticViewModel>();

            foreach (var workingShift in workingShifts)
            {
                var result = await WorkShiftStatistic(workingShift, currency);
                errors = errors.Concat(result.Errors.Select(x => x.Message));
                response = response.Append(result.Value);
            }

            return Result.Ok(response).WithErrors(errors);
        }

        public async Task<Result<StatisticViewModel>> GetWorkShiftStatisticsAsync(
            string userId,
            int shiftId,
            Currency currency)
        {
            var business = await _businessRepository.GetUserBusinessIncludingAll(userId);
            if (business is null)
            {
                return Result.Fail("User not found");
            }

            var shift = business.WorkingShifts.FirstOrDefault(x => x.Id == shiftId);
            if (shift is null)
            {
                return Result.Fail("Shift not found");
            }

            return await WorkShiftStatistic(shift, currency);
        }

        private async Task<Result<StatisticViewModel>> WorkShiftStatistic(WorkingShift shift, Currency currency)
        {
            IEnumerable<string> errors = new List<string>();

            var income = 0m;
            if (shift.Measurement is not null)
            {
                var goldPrice = await _exchangeService.GetGoldPriceAsync(shift.Measurement.Weight, currency);

                if (goldPrice.IsFailed)
                {
                    errors = errors.Concat(goldPrice.Errors.Select(x => x.Message));
                }
                else
                {
                    income = goldPrice.Value;
                }
            }

            var costs = 0m;
            foreach (var resource in shift.UsedResources)
            {
                var exchangeResult = await _exchangeService.ExchangeAsync(resource.Ammortization, currency);

                if (exchangeResult.IsFailed)
                {
                    errors = errors.Concat(exchangeResult.Errors.Select(x => x.Message));
                }
                else
                {
                    costs += exchangeResult.Value;
                }
            }
            TimeSpan time = (shift.End ?? DateTime.Now) - shift.Start;
            decimal hours = (decimal) time.TotalHours;
            costs = hours * costs;

            StatisticViewModel result = new StatisticViewModel()
            {
                ShiftId = shift.Id,
                Date = shift.Start,
                Income = income,
                Costs = costs,
                Profit = income - costs
            };

            return Result.Ok(result).WithErrors(errors);
        }
    }
}
