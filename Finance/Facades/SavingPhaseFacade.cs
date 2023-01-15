using Domain;
using Finance.Results;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net.Http.Json;

namespace Finance.Facades
{
    public class SavingPhaseFacade
    {
        private readonly IHttpClientFactory _clientFactory;

        public SavingPhaseFacade(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<decimal> GetSavingPhaseResultAsync(int ageFrom, int ageTo, decimal startCapital, int growthRate, decimal saveAmountPerMonth)
        {
            var ub = new UriBuilder("https://localhost:44324");
            ub.Path = "SavingPhase/Calculate";
            ub.Query = $"?ageFrom={ageFrom}" +
                $"&ageTo={ageTo}" +
                $"&startCapital={startCapital}" +
                $"&growthRate={growthRate}" +
                $"&saveAmountPerMonth={saveAmountPerMonth}";

            var httpClient = _clientFactory.CreateClient("SavingPhaseService");
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

        public async Task<SavingPhaseResult> GetSavingPhaseSimulationAsync(int ageFrom, int ageTo, decimal startCapital, int growthRate, decimal saveAmountPerMonth)
        {
            var ub = new UriBuilder("https://localhost:44324");
            ub.Path = "SavingPhase/Simulate";
            ub.Query = $"?ageFrom={ageFrom}" +
                $"&ageTo={ageTo}" +
                $"&startCapital={startCapital}" +
                $"&growthRate={growthRate}" +
                $"&saveAmountPerMonth={saveAmountPerMonth}";

            var httpClient = _clientFactory.CreateClient("SavingPhaseService");
            HttpResponseMessage response = await httpClient.GetAsync(ub.ToString());
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Http response error: {response.Content}.");
            }

            var jsonResponse = await response.Content.ReadFromJsonAsync<SavingPhaseResult>();
            if (jsonResponse == null)
            {
                throw new Exception($"{nameof(jsonResponse)} is null.");
            }

            return jsonResponse;
        }
    }
}
