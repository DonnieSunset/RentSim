using Domain;
using Finance.Results;
using Protocol;
using System.Net.Http.Json;

namespace Finance
{
    public class PhaseIntegrator2
    {
        public static async Task<PhaseIntegratorResult> Calculate(LifeAssumptions lifeAssumptions)
        {
            var result = await GetSavingPhaseResultAsync(lifeAssumptions);

            return new PhaseIntegratorResult()
            {
                ageStopWork = 63,

                overAmount_goodCase = 0,
                overAmount_badCase = 0,                       

                savingPhaseResult = new SavingPhaseResult
                { 
                    savingsCash = result,
                    savingsMetals = 100000,
                    savingsStocks = 100000
                },
            };
        }

        public static void Simulate(
            LifeAssumptions lifeAssumptions,
            PhaseIntegratorResult phaseIntegratorResult,
            IProtocolWriter protocolWriterGoodCase,
            IProtocolWriter protocolWriterBadCase)
        {

            // call the save phase service

            // write the result somehow in a proptocolWriter 

            
        }

        private static async Task<decimal> GetSavingPhaseResultAsync(LifeAssumptions lifeAssumptions)
        {
            var ub = new UriBuilder("https://localhost:44324");
            ub.Path = "SavingPhase/Calculate";
            ub.Query = $"?ageCurrent={lifeAssumptions.ageCurrent}" +
                $"&ageStopWork={63}" +
                $"&startCapital={lifeAssumptions.cash}" +
                $"&growthRate={lifeAssumptions.cashGrowthRate}" +
                $"&saveAmountPerMonth={lifeAssumptions.cashSaveAmountPerMonth}";

            string ss = ub.ToString();

            using (var httpClient = new HttpClient())
            {
                
                HttpResponseMessage response = await httpClient.GetAsync(ub.ToString());
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Http response error: {response.Content}.");
                }
                
                var stringResponse = await response.Content.ReadFromJsonAsync<string?>();

                if (stringResponse == null) 
                {
                    throw new Exception($"{nameof(stringResponse)} is null.");
                }

                var result = Decimal.Parse(stringResponse);
                return result;
            }

        }
    }
}
