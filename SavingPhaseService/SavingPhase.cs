using SavingPhaseService.Clients;
using SavingPhaseService.Contracts;
using SavingPhaseService.Controllers;

namespace SavingPhaseService
{
    internal class SavingPhase : ISavingPhase
    {
        public const decimal CAPITAL_YIELDS_TAX_FACTOR = 0.26m;
        public const decimal TAX_FREE_AMOUNT_PER_YEAR = 1000;

        public async Task<decimal> Calculate(
           int ageFrom,
           int ageTo,
           decimal startCapital,
           int growthRate,
           decimal saveAmountPerMonth,
           bool capitalYieldsTax,
           IFinanceMathClient financeMathClient
           )
        {
            int duration = ageTo - ageFrom;
            if (duration == 0)
            {
                return startCapital;
            }

            double interestFactor = 1 + (growthRate / 100d);
            decimal result = await financeMathClient.GetSparkassenFormelAsync(startCapital, saveAmountPerMonth * 12, interestFactor, duration);

            return result;
        }

        public SavingPhaseServiceResult Simulate(
            int ageFrom,
            int ageTo,
            decimal startCapital,
            decimal growthRate,
            decimal saveAmountPerMonth,
            bool capitalYieldsTax
            )
        {
            SavingPhaseServiceResult result = new();
            decimal currentCapital = startCapital;

            for (int age = ageFrom; age < ageTo; age++)
            {
                // get interests
                decimal interests = currentCapital * growthRate / 100m;
                currentCapital += interests;

                // add savings
                decimal savings = saveAmountPerMonth * 12;
                currentCapital += savings;

                //pay taxes
                decimal taxes = 0;
                if (capitalYieldsTax)
                {
                    decimal taxRelevantInterests = interests - TAX_FREE_AMOUNT_PER_YEAR;
                    if (taxRelevantInterests > 0)
                    { 
                        taxes = taxRelevantInterests * CAPITAL_YIELDS_TAX_FACTOR;
                        taxes = -taxes;
                    }
                }
                currentCapital += taxes;

                result.Entities.Add(new SavingPhaseServiceResult.Entity() { Age = age, Interests = interests, Deposit = savings, Taxes = taxes });
            }

            return result;
        }
    }
}
