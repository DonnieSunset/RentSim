using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace SavingPhaseService.Clients
{
    public class FinanceMathClient : IFinanceMathClient
    {
        public string myUrl { get; set; }

        public FinanceMathClient(string url)
        {
            myUrl = url;
        }


        public async Task<decimal> GetSparkassenFormelAsync(decimal anfangskapital, decimal rateProJahr, double zinsFaktor, int anzahlJahre)
        {
            var ub = new UriBuilder(myUrl);
            ub.Path = "FinanceMath/Sparkassenformel";
            ub.Query = $"?anfangskapital={anfangskapital}" +
                $"&rateProJahr={rateProJahr}" +
                $"&zinsFaktor={zinsFaktor.ToString(CultureInfo.InvariantCulture)}" +
                $"&anzahlJahre={anzahlJahre}";

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
    }
}
