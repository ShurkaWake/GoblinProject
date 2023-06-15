using BusinessLogic.ViewModels.Statistics;
using DataAccess.Enums;
using FluentResults;

namespace BusinessLogic.Abstractions
{
    public interface IStatisticService
    {
        public Task<Result<StatisticViewModel>> GetWorkShiftStatisticsAsync(
            string userId, 
            int shiftId, 
            Currency currency);

        public Task<Result<IEnumerable<StatisticViewModel>>> GetWorkingShiftsStatisticsAsync(
            string userId, 
            DateTime from, 
            DateTime to, 
            Currency currency);
    }
}
