using Microsoft.AspNetCore.Mvc;
using SavingPhaseService.Contracts;
using System.Globalization;

namespace RentSimS.Clients
{
    public class RentPhaseClient : IRentPhaseClient
    {
        public string myUrl { get; set; }

        public RentPhaseClient(string url)
        {
            myUrl = url;
        }

        public async Task<decimal> ApproxStateRent(
            int ageCurrent,
            decimal netRentAgeCurrent,
            int ageRentStart,
            decimal netRentAgeRentStart,
            int ageInQuestion)
        {
            var ub = new UriBuilder(myUrl);
            ub.Path = "RentPhase/ApproxStateRent";
            ub.Query = $"?ageCurrent={ageCurrent}" +
                $"&netRentAgeCurrent={netRentAgeCurrent}" +
                $"&ageRentStart={ageRentStart}" +
                $"&netRentAgeRentStart={netRentAgeRentStart}" +
                $"&ageInQuestion={ageInQuestion}";

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
