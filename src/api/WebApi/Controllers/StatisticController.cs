using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using DataAccess.Entities;
using DataAccess.Enums;
using GenericWebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/statistic")]
    [Authorize(Roles = $"{Roles.Owner},{Roles.Manager}")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;
        private readonly UserManager<AppUser> _userManager;

        public StatisticController(IStatisticService statisticService, UserManager<AppUser> userManager)
        {
            _statisticService = statisticService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetPeriodStatisticsAsync(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromHeader] Currency currency = Currency.USD)
        {
            if (from is null)
            {
                from = DateTime.MinValue;
            }

            if (to is null)
            {
                to = DateTime.Now;
            }

            var user = await _userManager.GetUserAsync(User);
            var result = await _statisticService.GetWorkingShiftsStatisticsAsync(
                user.Id, 
                (DateTime) from, 
                (DateTime) to, 
                currency);

            return result.ToObjectResponse();
        }

        [HttpGet("{shiftId:int}")]
        public async Task<IActionResult> GetWorkiShiftStatisticsAsync(
            [FromRoute] int shiftId,
            [FromHeader] Currency currency = Currency.USD
            )
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _statisticService.GetWorkShiftStatisticsAsync(user.Id, shiftId, currency);
            return result.ToObjectResponse();
        }
    }
}
