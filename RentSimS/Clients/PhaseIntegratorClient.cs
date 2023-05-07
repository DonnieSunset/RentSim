using Domain;
using RentSimS.DTOs;

namespace RentSimS.Clients
{
    public class PhaseIntegratorClient : IPhaseIntegratorClient
    {
        public string myUrl { get; set; }
        private readonly IHttpClientFactory myClientFactory;
        private readonly IConfiguration myConfiguration;

        public PhaseIntegratorClient(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            myClientFactory = clientFactory;
            myConfiguration = configuration;

            myUrl = myConfiguration.GetValue<string>("PhaseIntegratorService:url");
        }

        public async Task<PhaseIntegratorServiceResultDTO> GetPhaseIntegratorGoodCaseAsync(LifeAssumptions input)
        {
            var ub = new UriBuilder(myUrl);
            ub.Path = "PhaseIntegrator/SimulateGoodCase";
            
            var httpClient = myClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(ub.ToString(), input);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Http response error: {response.Content}.");
            }

            var jsonResponse = await response.Content.ReadFromJsonAsync<PhaseIntegratorServiceResultDTO>();
            if (jsonResponse == null)
            {
                throw new Exception($"{nameof(jsonResponse)} is null.");
            }

            return jsonResponse;
        }
    }
}
