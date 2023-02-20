using RentPhaseService.DTOs;
using System.Globalization;

namespace RentPhaseService.Clients
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

        public async Task<RentPhaseServiceResultDTO> RateByNumericalSparkassenformel(decimal betrag_cash, decimal betrag_stocks, decimal betrag_metals, decimal zins_cash, decimal zins_stocks, decimal zins_metals, decimal endbetrag, int yearStart, int yearEnd)
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

                //var stringResponse = await response.Content.ReadAsStringAsync();
                //if (stringResponse == null)
                //{
                //    throw new Exception($"{nameof(stringResponse)} is null.");
                //}

                //return stringResponse;
                var objResponse = await response.Content.ReadFromJsonAsync<RentPhaseServiceResultDTO>();
                return objResponse;
            }
        }

        public async Task<RentPhaseServiceResultDTO> StartCapitalByNumericalSparkassenformel(decimal rateTotal_perYear, decimal betrag_cash, decimal zins_cash, decimal betrag_stocks, decimal zins_stocks, decimal betrag_metals, decimal zins_metals, decimal endbetrag, int yearStart, int yearEnd)
        {
            var ub = new UriBuilder(myUrl);
            ub.Path = "FinanceMath/StartCapitalByNumericalSparkassenformel";
            ub.Query =
                $"?rateTotal_perYear={rateTotal_perYear.ToString(CultureInfo.InvariantCulture)}" +
                $"&amount_cash={betrag_cash.ToString(CultureInfo.InvariantCulture)}" +
                $"&zins_cash={zins_cash.ToString(CultureInfo.InvariantCulture)}" +
                $"&amount_stocks={betrag_stocks.ToString(CultureInfo.InvariantCulture)}" +
                $"&zins_stocks={zins_stocks.ToString(CultureInfo.InvariantCulture)}" +
                $"&amount_metals={betrag_metals.ToString(CultureInfo.InvariantCulture)}" +
                $"&zins_metals={zins_metals.ToString(CultureInfo.InvariantCulture)}" +
                $"&endbetrag={endbetrag.ToString(CultureInfo.InvariantCulture)}" +
                $"&yearStart={yearStart}" +
                $"&yearEnd={yearEnd}"
                ;

            using (var httpClient = new HttpClient())
            {
                try
                {

                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                }catch(Exception ex)
                {
                    Console.WriteLine();
                }
                HttpResponseMessage response = await httpClient.GetAsync(ub.ToString());

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Http response error: {response.Content}.");
                }

                //var stringResponse = await response.Content.ReadAsStringAsync();
                //File.WriteAllText(@"C:\temp\blubb.txt", stringResponse);
                ////var stringResponse = await response.Content.ReadFromJsonAsync< string>();
                //if (stringResponse == null)
                //{
                //    throw new Exception($"{nameof(stringResponse)} is null.");
                //}

                //return stringResponse;

                var objResponse = await response.Content.ReadFromJsonAsync<RentPhaseServiceResultDTO>();
                return objResponse;
            }
        }
    }
}
