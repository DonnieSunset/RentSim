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

        public async Task<SavingPhaseServiceResult> GetSavingPhaseSimulationAsync(SavingPhaseServiceInputDTO input)
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

                var jsonResponse = await response.Content.ReadFromJsonAsync<SavingPhaseServiceResult>();
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

                protocolWriter.Log(age, new TransactionDetails { cashDeposit = entry.DepositCash, cashInterests = entry.InterestsCash, cashTaxes = entry.TaxesCash });
                protocolWriter.Log(age, new TransactionDetails { stockDeposit = entry.DepositStocks, stockInterests = entry.InterestsStocks, stockTaxes = entry.TaxesStocks });
                protocolWriter.Log(age, new TransactionDetails { metalDeposit = entry.DepositMetals, metalInterests = entry.InterestsMetals, metalTaxes = entry.TaxesMetals });
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
