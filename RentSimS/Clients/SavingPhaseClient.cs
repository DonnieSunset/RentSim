using Domain;
using Finance.Results;
using Protocol;
using SavingPhaseService.Contracts;
using System.Globalization;

namespace RentSimS.Clients
{
    public class SavingPhaseClient : ISavingPhaseClient
    {
        //private readonly IHttpClientFactory _clientFactory;

        public string myUrl { get; set; }

        public SavingPhaseClient(string url)
        {
            myUrl = url;
          //  _clientFactory = httpClientFactory;
        }

        public async Task<decimal> GetSavingPhaseResultAsync(int ageFrom, int ageTo, decimal startCapital, int growthRate, decimal saveAmountPerMonth)
        {
            var ub = new UriBuilder(myUrl);
            ub.Path = "SavingPhase/Calculate";
            ub.Query = $"?ageFrom={ageFrom}" +
                $"&ageTo={ageTo}" +
                $"&startCapital={startCapital}" +
                $"&growthRate={growthRate}" +
                $"&saveAmountPerMonth={saveAmountPerMonth}";

            //var httpClient = _clientFactory.CreateClient("SavingPhaseService");
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(ub.ToString());
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Http response error: {response.Content}.");
                }

                var stringResponse = await response.Content.ReadAsStringAsync();
                if (stringResponse == null)
                {
                    throw new Exception($"{nameof(stringResponse)} is null.");
                }

                decimal result = decimal.Parse(stringResponse, CultureInfo.InvariantCulture);
                return result;
            }
        }

        public async Task<SimulationResult> GetSavingPhaseSimulationAsync(int ageFrom, int ageTo, decimal startCapital, int growthRate, decimal saveAmountPerMonth)
        {
            var ub = new UriBuilder(myUrl);
            ub.Path = "SavingPhase/Simulate";
            ub.Query = $"?ageFrom={ageFrom}" +
                $"&ageTo={ageTo}" +
                $"&startCapital={startCapital}" +
                $"&growthRate={growthRate}" +
                $"&saveAmountPerMonth={saveAmountPerMonth}";

            //var httpClient = _clientFactory.CreateClient("SavingPhaseService");
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(ub.ToString());
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Http response error: {response.Content}.");
                }

                var jsonResponse = await response.Content.ReadFromJsonAsync<SimulationResult>();
                if (jsonResponse == null)
                {
                    throw new Exception($"{nameof(jsonResponse)} is null.");
                }

                return jsonResponse;
            }
        }

        public async Task<SavingPhaseResult> GetAndLogSavingPhase(
            int ageFrom, int ageTo,
            decimal cash_startCapital, int cash_growthRate, decimal cash_saveAmountPerMonth,
            decimal stocks_startCapital, int stocks_growthRate, decimal stocks_saveAmountPerMonth,
            decimal metals_startCapital, int metals_growthRate, decimal metals_saveAmountPerMonth,
            IProtocolWriter protocolWriter)
        {
            var totalSavingsCash = await GetSavingPhaseResultAsync(ageFrom, ageTo, cash_startCapital, cash_growthRate, cash_saveAmountPerMonth);
            var totalSavingsStocks = await GetSavingPhaseResultAsync(ageFrom, ageTo, stocks_startCapital, stocks_growthRate, stocks_saveAmountPerMonth);
            var totalSavingsMetals = await GetSavingPhaseResultAsync(ageFrom, ageTo, metals_startCapital, metals_growthRate, metals_saveAmountPerMonth);

            decimal totalSavings = totalSavingsCash + totalSavingsStocks + totalSavingsMetals;

            var savingPhaseSimCash = await GetSavingPhaseSimulationAsync(ageFrom, ageTo, cash_startCapital, cash_growthRate, cash_saveAmountPerMonth);
            var savingPhaseSimStocks = await GetSavingPhaseSimulationAsync(ageFrom, ageTo, stocks_startCapital, stocks_growthRate, stocks_saveAmountPerMonth);
            var savingPhaseSimMetals = await GetSavingPhaseSimulationAsync(ageFrom, ageTo, metals_startCapital, metals_growthRate, metals_saveAmountPerMonth);

            protocolWriter.LogBalanceYearBegin(ageFrom, cash_startCapital, stocks_startCapital, metals_startCapital);
            for (int age = ageFrom; age < ageTo; age++)
            {
                var cashEntry = savingPhaseSimCash.Entities.Single(x => x.Age == age);
                var metalsEntry = savingPhaseSimMetals.Entities.Single(x => x.Age == age);
                var stocksEntry = savingPhaseSimStocks.Entities.Single(x => x.Age == age);

                protocolWriter.Log(age, new TransactionDetails { cashDeposit = cashEntry.Deposit, cashInterests = cashEntry.Interests });
                protocolWriter.Log(age, new TransactionDetails { stockDeposit = stocksEntry.Deposit, stockInterests = stocksEntry.Interests });
                protocolWriter.Log(age, new TransactionDetails { metalDeposit = metalsEntry.Deposit, metalInterests = metalsEntry.Interests });
            }

            return new SavingPhaseResult
            {
                savingsCash = totalSavingsCash,
                savingsStocks = totalSavingsStocks,
                savingsMetals = totalSavingsMetals,
            };
        }
    }
}
