using Domain;
using Finance.Results;
using Microsoft.AspNetCore.Mvc;
using Protocol;
using SavingPhaseService.Contracts;
using System.Globalization;
using System.Text.Json;

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
            IProtocolWriter protocolWriter, IFinanceMathClient financeMathClient)
        {
            var rentPhaseResult = new RentPhaseResult();

            double factorCash = (double)(savingPhaseResult.savingsCash / savingPhaseResult.SavingsTotal);
            double factorStocks = (double)(savingPhaseResult.savingsStocks / savingPhaseResult.SavingsTotal);
            double factorMetals = (double)(savingPhaseResult.savingsMetals / savingPhaseResult.SavingsTotal);
            decimal rateNeeded = (laterNeedsResult.needsComfort_AgeRentStart_WithInflation_PerMonth - stateRentResult.assumedStateRent_Net_PerMonth) * 12;

            var rentPhaseResultString = await financeMathClient.StartCapitalByNumericalSparkassenformel(
                rateNeeded,
                factorCash,
                growRateCash,
                factorStocks,
                growRateStocks,
                factorMetals,
                growRateMetals,
                0,
                ageStart,
                ageEnd);

            var rentPhaseResultJson = JsonDocument.Parse(rentPhaseResultString);
            foreach (JsonElement o in rentPhaseResultJson.RootElement.EnumerateArray())
            {
                int age = o.GetProperty("Age").GetInt32();
                decimal yearBegin_cash = o.GetProperty("YearBegin").GetProperty("Cash").GetDecimal();
                decimal yearBegin_stocks = o.GetProperty("YearBegin").GetProperty("Stocks").GetDecimal();
                decimal yearBegin_metals = o.GetProperty("YearBegin").GetProperty("Metals").GetDecimal();
                decimal rate_cash = o.GetProperty("Rates").GetProperty("Cash").GetDecimal();
                decimal rate_stocks = o.GetProperty("Rates").GetProperty("Stocks").GetDecimal();
                decimal rate_metals = o.GetProperty("Rates").GetProperty("Metals").GetDecimal();
                decimal zins_cash = o.GetProperty("Zins").GetProperty("Cash").GetDecimal();
                decimal zins_stocks = o.GetProperty("Zins").GetProperty("Stocks").GetDecimal();
                decimal zins_metals = o.GetProperty("Zins").GetProperty("Metals").GetDecimal();

                // this if is only necessary because we dont have a stopwork phase. Delete it once all phases are integrated in the correct order.
                if (age == ageStart)
                {
                    protocolWriter.LogBalanceYearBegin(age, yearBegin_cash, yearBegin_stocks, yearBegin_metals);
                }
                protocolWriter.Log(age, new TransactionDetails { cashDeposit = -rate_cash, cashInterests = zins_cash });
                protocolWriter.Log(age, new TransactionDetails { stockDeposit = -rate_stocks, stockInterests = zins_stocks });
                protocolWriter.Log(age, new TransactionDetails { metalDeposit = -rate_metals, metalInterests = zins_metals });
            }

            rentPhaseResult.rate_perMonth = -protocolWriter.Protocol.Single(x => x.age == ageStart).TotalDeposits / 12m;

            return rentPhaseResult;
        }
    }
}
