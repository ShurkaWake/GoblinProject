using BusinessLogic.Abstractions;
using BusinessLogic.ViewModels;
using DataAccess.Entities;
using DataAccess.Enums;
using FluentResults;
using System.Net.Http.Json;

namespace BusinessLogic.Services
{
    public class ExchangeService : IExchangeService
    {
        private const string NbuApiUrl = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json&valcode={0}";

        private static readonly Dictionary<Currency, string> CurrencyName 
            = new Dictionary<Currency, string>()
        {
            { Currency.EUR, "EUR" },
            { Currency.UAH, "UAH" },
            { Currency.EUR, "EUR" },
        };

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
            try
            {
                var client = new HttpClient();
                var requestUrl = string.Format(NbuApiUrl, CurrencyName[currency]);
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
    }
}
