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
    }
}
