using BusinessLogic.Abstractions;
using BusinessLogic.ViewModels.ExternalApiResponses;
using DataAccess.Entities;
using DataAccess.Enums;
using FluentResults;
using System.Net.Http.Json;

namespace BusinessLogic.Services
{
    public class ExchangeService : IExchangeService
    {
        private const string NbuApiUrl = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";
        private Dictionary<string, decimal> rates = new Dictionary<string, decimal>();
        private DateTime lastFetch;

        public ExchangeService()
        {
            FetchRateToUahAsync().Wait();
        }

        public async Task<Result<decimal>> ExchangeAsync(MoneyAmount from, Currency to)
        {
            if (DateTime.Now - lastFetch > TimeSpan.FromHours(1))
            {
                FetchRateToUahAsync().Wait();
            }

            var rateFrom = rates[from.Currency.ToString()];
            var rateTo = rates[to.ToString()];

            return from.Amount * (rateFrom / rateTo);
        }

        public async Task<Result<decimal>> GetGoldPriceAsync(decimal ozGold, Currency to)
        {
            if (DateTime.Now - lastFetch > TimeSpan.FromHours(1))
            {
                FetchRateToUahAsync().Wait();
            }

            var rateFrom = rates["XAU"];
            var rateTo = rates[to.ToString()];
            return ozGold * (rateFrom / rateTo);
        }

        private async Task<Result> FetchRateToUahAsync()
        {
            try
            {
                var client = new HttpClient();
                rates = new Dictionary<string, decimal>();
                var response = await client.GetFromJsonAsync<IEnumerable<NbuResponseModel>>(NbuApiUrl);
                rates.Add("UAH", 1m);
                lastFetch = DateTime.Now;

                foreach (var item in response)
                {
                    rates.Add(item.Cc, item.Rate);
                }

                return Result.Ok();
            }
            catch
            {
                return Result.Fail("Unable to get exchange rates from bank.gov.ua");
            }
        }

        public Result<decimal> OzToGrams(decimal ozGold)
        {
            const decimal k = 31.10348013m;
            return Result.Ok(k * ozGold);
        }

        public Result<decimal> GramsToOz(decimal gramsGold)
        { 
            const decimal k = 0.032150743126506m;
            return Result.Ok(k * gramsGold);
        }
    }
}
