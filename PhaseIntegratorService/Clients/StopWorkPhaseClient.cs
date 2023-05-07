using Protocol;
using PhaseIntegratorService.DTOs;

namespace PhaseIntegratorService.Clients
{
    public class StopWorkPhaseClient : IStopWorkPhaseClient
    {
        public string myUrl { get; set; }
        private readonly IHttpClientFactory myClientFactory;
        private readonly IConfiguration myConfiguration;

        public StopWorkPhaseClient(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            myClientFactory = clientFactory;
            myConfiguration = configuration;

            myUrl = myConfiguration.GetValue<string>("StopWorkPhaseService:url");
        }

        public async Task<StopWorkPhaseServiceResultDTO> GetStopWorkPhaseSimulationAsync(StopWorkPhaseServiceInputDTO input)
        {
            var ub = new UriBuilder(myUrl);
            ub.Path = "StopWorkPhase/Simulate";
            
            var httpClient = myClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(ub.ToString(), input);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Http response error: {response.Content}.");
            }

            var jsonResponse = await response.Content.ReadFromJsonAsync<StopWorkPhaseServiceResultDTO>();
            if (jsonResponse == null)
            {
                throw new Exception($"{nameof(jsonResponse)} is null.");
            }

            return jsonResponse; ;
        }

        public void LogStopWorkPhaseResult(StopWorkPhaseServiceResultDTO result, IProtocolWriter protocolWriter)
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
