using SavingPhaseService.Contracts;
using SavingPhaseService.Controllers;

namespace SavingPhaseService
{
    internal class SavingPhaseService
    {
        public static async Task<decimal> Calculate(
           int ageFrom,
           int ageTo,
           decimal startCapital,
           int growthRate,
           decimal saveAmountPerMonth)
        {
            int duration = ageTo - ageFrom;

            double interestFactor = 1 + (growthRate / 100d);
            //decimal result = FinanceCalculator.SparkassenFormel(startCapital, saveAmountPerMonth * 12, interestFactor, duration);
            decimal result = await FinanceMathController.GetSparkassenFormelAsync(startCapital, saveAmountPerMonth * 12, interestFactor, duration);

            return result;
        }

        public static SimulationResult Simulate(
            int ageFrom,
            int ageTo,
            decimal startCapital,
            int growthRate,
            decimal saveAmountPerMonth)
        {
            SimulationResult result = new();
            decimal currentCapital = startCapital;

            for (int age = ageFrom; age < ageTo; age++)
            {
                decimal interests = currentCapital * growthRate / 100m;

                currentCapital += interests;
                decimal savings = saveAmountPerMonth * 12;
                currentCapital += savings;

                result.Entities.Add(new SimulationResult.Entity() { Age = age, Interests = interests, Deposit = savings });
            }

            return result;
        }
    }
}
