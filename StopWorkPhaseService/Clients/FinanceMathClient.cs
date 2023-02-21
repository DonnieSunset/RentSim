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

        public async Task<StopWorkPhaseServiceResultDTO> RateByNumericalSparkassenformel(decimal betrag_cash, decimal betrag_stocks, decimal betrag_metals, decimal zins_cash, decimal zins_stocks, decimal zins_metals, decimal endbetrag, int yearStart, int yearEnd)
        {
            var ub = new UriBuilder(myUrl);
            ub.Path = "FinanceMath/RateByNumericalSparkassenformel";
            ub.Query =
                $"?betrag_cash={betrag_cash.ToString(CultureInfo.InvariantCulture)}" +
                $"&betrag_stocks={betrag_stocks.ToString(CultureInfo.InvariantCulture)}" +
                $"&betrag_metals={betrag_metals.ToString(CultureInfo.InvariantCulture)}" +
                $"&zins_cash={zins_cash.ToString(CultureInfo.InvariantCulture)}" +
                $"&zins_stocks={zins_stocks.ToString(CultureInfo.InvariantCulture)}" +
                $"&zins_metals={zins_metals.ToString(CultureInfo.InvariantCulture)}" +
                $"&endbetrag={endbetrag.ToString(CultureInfo.InvariantCulture)}" +
                $"&yearStart={yearStart}" +
                $"&yearEnd={yearEnd}"
                ;

            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(ub.ToString());
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Http response error: {response.Content}.");
                }

                var objResponse = await response.Content.ReadFromJsonAsync<StopWorkPhaseServiceResultDTO>();
                return objResponse;
            }
        }
    }
}
