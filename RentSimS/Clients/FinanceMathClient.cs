using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace RentSimS.Clients
{
    public class FinanceMathClient : IFinanceMathClient
    {
        public string myUrl { get; set; }

        public FinanceMathClient(string url)
        {
            myUrl = url;
        }

        public async Task<decimal> GetAmountWithInflationAsync(int ageStart, int ageEnd, decimal amount, double inflationRate)
        {
            var ub = new UriBuilder(myUrl);
            ub.Path = "FinanceMath/AmountWithInflation";
            ub.Query = $"?ageStart={ageStart}" +
                $"&ageEnd={ageEnd}" +
                $"&amount={amount.ToString(CultureInfo.InvariantCulture)}" +
                $"&inflationRate={inflationRate.ToString(CultureInfo.InvariantCulture)}";

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
