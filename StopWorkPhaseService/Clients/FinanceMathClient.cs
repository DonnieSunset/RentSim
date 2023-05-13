using StopWorkPhaseService.DTOs;
using System.Globalization;

namespace StopWorkPhaseService.Clients
{
    public class FinanceMathClient : IFinanceMathClient
    {
        public string myUrl { get; set; }

        public FinanceMathClient(string url)
        {
            myUrl = url;
        }

        public async Task<StopWorkPhaseServiceResultDTO> RateByNumericalSparkassenformel(StopWorkPhaseServiceInputDTO input)
        {
            var ub = new UriBuilder(myUrl);
            ub.Path = "FinanceMath/RateByNumericalSparkassenformel";

            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(ub.ToString(), input);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Http response error: {response.Content}.");
                }

                var stopWorkPhaseResult = await response.Content.ReadFromJsonAsync<StopWorkPhaseServiceResultDTO>();
                return stopWorkPhaseResult;
            }
        }
    }
}
