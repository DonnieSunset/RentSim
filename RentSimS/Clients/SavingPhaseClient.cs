using Protocol;
using RentSimS.DTOs;

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

        public async Task<SavingPhaseServiceResultDTO> GetSavingPhaseSimulationAsync(SavingPhaseServiceInputDTO input)
        {
            var ub = new UriBuilder(myUrl);
            ub.Path = "SavingPhase/Simulate";

            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = null;
                response = await httpClient.PostAsJsonAsync(ub.ToString(), input);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Http response error: {response.Content}.");
                }

                var jsonResponse = await response.Content.ReadFromJsonAsync<SavingPhaseServiceResultDTO>();
                if (jsonResponse == null)
                {
                    throw new Exception($"{nameof(jsonResponse)} is null.");
                }

                return jsonResponse;
            }
        }

        public async Task<SavingPhaseResult> GetAndLogSavingPhase(SavingPhaseServiceInputDTO input, IProtocolWriter protocolWriter)
        {
            var savingPhaseSim = await GetSavingPhaseSimulationAsync(input);

            protocolWriter.LogBalanceYearBegin(input.AgeFrom, input.StartCapitalCash, input.StartCapitalStocks, input.StartCapitalMetals);
            for (int age = input.AgeFrom; age < input.AgeTo; age++)
            {
                var entry = savingPhaseSim.Entities.Single(x => x.Age == age);

                protocolWriter.Log(age, new TransactionDetails { cashDeposit = entry.Deposits.Cash, cashInterests = entry.Interests.Cash, cashTaxes = entry.Taxes.Cash });
                protocolWriter.Log(age, new TransactionDetails { stockDeposit = entry.Deposits.Stocks, stockInterests = entry.Interests.Stocks, stockTaxes = entry.Taxes.Stocks });
                protocolWriter.Log(age, new TransactionDetails { metalDeposit = entry.Deposits.Metals, metalInterests = entry.Interests.Metals, metalTaxes = entry.Taxes.Metals });
            }

            return new SavingPhaseResult
            {
                savingsCash = savingPhaseSim.FinalSavingsCash,
                savingsStocks = savingPhaseSim.FinalSavingsStocks,
                savingsMetals = savingPhaseSim.FinalSavingsMetals,
            };
        }
    }
}
