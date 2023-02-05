using Finance.Results;
using Protocol;
using RentSimS.DTOs;
using System.Globalization;

namespace RentSimS.Clients
{
    public class RentPhaseClient : IRentPhaseClient
    {
        public string myUrl { get; set; }

        public RentPhaseClient(string url)
        {
            myUrl = url;
        }

        public async Task<decimal> ApproxStateRent(
            int ageCurrent,
            decimal netRentAgeCurrent,
            int ageRentStart,
            decimal netRentAgeRentStart,
            int ageInQuestion)
        {
            var ub = new UriBuilder(myUrl);
            ub.Path = "RentPhase/ApproxStateRent";
            ub.Query = $"?ageCurrent={ageCurrent}" +
                $"&netRentAgeCurrent={netRentAgeCurrent}" +
                $"&ageRentStart={ageRentStart}" +
                $"&netRentAgeRentStart={netRentAgeRentStart}" +
                $"&ageInQuestion={ageInQuestion}";

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

        public async Task<RentPhaseResult> GetAndLogRentPhase(
            int ageStart, int ageEnd,
            double growRateCash, double growRateStocks, double growRateMetals,
            SavingPhaseResult savingPhaseResult,
            LaterNeedsResult laterNeedsResult,
            StateRentResult stateRentResult,
            IProtocolWriter protocolWriter)
        {
            decimal rateNeeded = (laterNeedsResult.needsComfort_AgeRentStart_WithInflation_PerMonth - stateRentResult.assumedStateRent_Net_PerMonth) * 12;

            var ub = new UriBuilder(myUrl);
            ub.Path = "RentPhase/Simulate";
            ub.Query = 
                $"?ageFrom={ageStart}" +
                $"&ageTo={ageEnd}" +
                $"&totalRateNeeded_perYear={rateNeeded.ToString(CultureInfo.InvariantCulture)}" +
                $"&capitalCash={savingPhaseResult.savingsCash.ToString(CultureInfo.InvariantCulture)}" +
                $"&growthRateCash={growRateCash.ToString(CultureInfo.InvariantCulture)}" +
                $"&capitalStocks={savingPhaseResult.savingsStocks.ToString(CultureInfo.InvariantCulture)}" +
                $"&growthRateStocks={growRateStocks.ToString(CultureInfo.InvariantCulture)}" +
                $"&capitalMetals={savingPhaseResult.savingsMetals.ToString(CultureInfo.InvariantCulture)}" +
                $"&growthRateMetals={growRateMetals.ToString(CultureInfo.InvariantCulture)}";

            SimulationResultDTO objResponse;
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(ub.ToString());
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Http response error: {response.Content}.");
                }

                objResponse = await response.Content.ReadFromJsonAsync<SimulationResultDTO>();
            }

            foreach (var entry in objResponse.Entities) 
            {
                if (entry.Age == ageStart)
                {
                    protocolWriter.LogBalanceYearBegin(entry.Age, entry.YearBegin.Cash, entry.YearBegin.Stocks, entry.YearBegin.Metals);
                }

                protocolWriter.Log(entry.Age, new TransactionDetails { cashDeposit = -entry.Rates.Cash, cashInterests = entry.Zins.Cash, cashTaxes = entry.Taxes.Cash });
                protocolWriter.Log(entry.Age, new TransactionDetails { stockDeposit = -entry.Rates.Stocks, stockInterests = entry.Zins.Stocks, stockTaxes = entry.Taxes.Stocks });
                protocolWriter.Log(entry.Age, new TransactionDetails { metalDeposit = -entry.Rates.Metals, metalInterests = entry.Zins.Metals, metalTaxes = entry.Taxes.Metals });
            }

            var rentPhaseResult = new RentPhaseResult();
            rentPhaseResult.rate_perMonth = -protocolWriter.Protocol.Single(x => x.age == ageStart).TotalDeposits / 12m;

            return rentPhaseResult;
        }
    }
}
