using SavingPhaseService.DTOs;

namespace SavingPhaseService
{
    internal class SavingPhase : ISavingPhase
    {
        public const decimal CAPITAL_YIELDS_TAX_FACTOR = 0.26m;
        public const decimal TAX_FREE_AMOUNT_PER_YEAR = 1000;

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

                // pay taxes
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
            result.FinalSavings = currentCapital;

            return result;
        }
    }
}
