using DataAccess.Entities;
using DataAccess.Enums;
using FluentResults;

namespace BusinessLogic.Abstractions
{
    public interface IExchangeService
    {
        public Task<Result<decimal>> ExchangeAsync(MoneyAmount from, Currency to);
        
        public Task<Result<decimal>> GetGoldPriceAsync(decimal ozGold, Currency to);

        public Result<decimal> OzToGrams(decimal ozGold);

        public Result<decimal> GramsToOz(decimal kgGold);
    }
}
