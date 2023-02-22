using Protocol;
using RentSimS.DTOs;

namespace RentSimS.Clients
{
    public class SavingPhaseClient : ISavingPhaseClient
    {
        private string myUrl;
        private readonly IHttpClientFactory myClientFactory;
        private readonly IConfiguration myConfiguration;

        public SavingPhaseClient(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            myClientFactory = clientFactory;
            myConfiguration = configuration;

            myUrl = myConfiguration.GetValue<string>("SavingPhaseService:url");
        }

        public async Task<SavingPhaseServiceResultDTO> GetSavingPhaseSimulationAsync(SavingPhaseServiceInputDTO input)
        {
            var ub = new UriBuilder(myUrl);
            ub.Path = "SavingPhase/Simulate";

            var httpClient = myClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(ub.ToString(), input);

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

        public void LogSavingPhaseResult(SavingPhaseServiceResultDTO result, IProtocolWriter protocolWriter)
        {
            var firstEntry = result.Entities.First();
            protocolWriter.LogBalanceYearBegin(firstEntry.Age, result.FirstYearBeginValues.Cash, result.FirstYearBeginValues.Stocks, result.FirstYearBeginValues.Metals);

            foreach (var entity in result.Entities)
            {
                protocolWriter.Log(entity.Age, new TransactionDetails { cashDeposit = entity.Deposits.Cash, cashInterests = entity.Interests.Cash, cashTaxes = entity.Taxes.Cash });
                protocolWriter.Log(entity.Age, new TransactionDetails { stockDeposit = entity.Deposits.Stocks, stockInterests = entity.Interests.Stocks, stockTaxes = entity.Taxes.Stocks });
                protocolWriter.Log(entity.Age, new TransactionDetails { metalDeposit = entity.Deposits.Metals, metalInterests = entity.Interests.Metals, metalTaxes = entity.Taxes.Metals });
            }
        }
    }
}
