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
        private const string NbuApiUrl = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json&valcode={0}";

        public async Task<Result<decimal>> ExchangeAsync(MoneyAmount from, Currency to)
        {
            var rateFrom = await GetRateToUahAsync(from.Currency);
            if (rateFrom.IsFailed)
            {
                return Result.Fail(rateFrom.Errors);
            }

            var rateTo = await GetRateToUahAsync(to);
            if (rateTo.IsFailed)
            {
                return Result.Fail(rateFrom.Errors);
            }

            return from.Amount * (rateFrom.Value / rateTo.Value);
        }

        public async Task<Result<decimal>> GetGoldPriceAsync(decimal ozGold, Currency to)
        {
            var rateFrom = await GetGoldRateUah();
            if (rateFrom.IsFailed)
            {
                return Result.Fail(rateFrom.Errors);
            }

            var rateTo = await GetRateToUahAsync(to);
            if (rateTo.IsFailed)
            {
                return Result.Fail(rateFrom.Errors);
            }

            return ozGold * (rateFrom.Value / rateTo.Value);
        }

        private async Task<Result<decimal>> GetRateToUahAsync(Currency currency)
        {
            if (currency == Currency.UAH)
            {
                return 1;
            }

            try
            {
                var client = new HttpClient();
                var requestUrl = string.Format(NbuApiUrl, currency.ToString());
                var response = (await client.GetFromJsonAsync<IEnumerable<NbuResponseModel>>(requestUrl))
                    .FirstOrDefault();

                return Result.Ok(response.Rate);
            }
            catch
            {
                return Result.Fail("Unable to get exchange rates from bank.gov.ua");
            }
        }

        private async Task<Result<decimal>> GetGoldRateUah()
        {
            try
            {
                var client = new HttpClient();
                var requestUrl = string.Format(NbuApiUrl, "XAU");
                var response = (await client.GetFromJsonAsync<IEnumerable<NbuResponseModel>>(requestUrl))
                    .FirstOrDefault();

                return Result.Ok(response.Rate);
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
