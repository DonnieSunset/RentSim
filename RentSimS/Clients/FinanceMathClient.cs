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

        public async Task<decimal> RateByNumericalSparkassenformel(double betrag1, double zins1, double betrag2, double zins2, double betrag3, double zins3, double endbetrag, int yearStart, int yearEnd)
        {
            var ub = new UriBuilder(myUrl);
            ub.Path = "FinanceMath/RateByNumericalSparkassenformel";
            ub.Query =
                $"?betrag1={betrag1.ToString(CultureInfo.InvariantCulture)}" +
                $"&zins1={zins1.ToString(CultureInfo.InvariantCulture)}" +
                $"&betrag2={betrag2.ToString(CultureInfo.InvariantCulture)}" +
                $"&zins2={zins2.ToString(CultureInfo.InvariantCulture)}" +
                $"&betrag3={betrag3.ToString(CultureInfo.InvariantCulture)}" +
                $"&zins3={zins3.ToString(CultureInfo.InvariantCulture)}" +
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

                var stringResponse = await response.Content.ReadAsStringAsync();
                if (stringResponse == null)
                {
                    throw new Exception($"{nameof(stringResponse)} is null.");
                }

                decimal result = decimal.Parse(stringResponse, CultureInfo.InvariantCulture);

                return result;
            }
        }

//        public JsonResult StartCapitalByNumericalSparkassenformel(decimal rateTotal_perYear, double factor1, double zins1, double factor2, double zins2, double factor3, double zins3, decimal endbetrag, int yearStart, int yearEnd)


        public async Task<decimal> StartCapitalByNumericalSparkassenformel(decimal rateTotal_perYear, double factor1, double zins1, double factor2, double zins2, double factor3, double zins3, decimal endbetrag, int yearStart, int yearEnd)
        {
            var ub = new UriBuilder(myUrl);
            ub.Path = "FinanceMath/StartCapitalByNumericalSparkassenformel";
            ub.Query =
                $"?rateTotal_perYear={rateTotal_perYear.ToString(CultureInfo.InvariantCulture)}" +
                $"&factor1={factor1.ToString(CultureInfo.InvariantCulture)}" +
                $"&zins1={zins1.ToString(CultureInfo.InvariantCulture)}" +
                $"&factor2={factor2.ToString(CultureInfo.InvariantCulture)}" +
                $"&zins2={zins2.ToString(CultureInfo.InvariantCulture)}" +
                $"&factor3={factor3.ToString(CultureInfo.InvariantCulture)}" +
                $"&zins3={zins3.ToString(CultureInfo.InvariantCulture)}" +
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
