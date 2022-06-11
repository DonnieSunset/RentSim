using Domain;
using Finance;
using NUnit.Framework;


namespace Finance_uTests
{
    [TestFixture]
    public class FinanceCalculatorTests
    {
        [Test, TestCaseSource(nameof(rentPhaseInputDataList))]
        public void CalculateRentPhaseResult_ValidScenario(RentPhaseInputData rentPhase)
        {
            RentPhaseResult rentPhaseResult = FinanceCalculator.CalculateRentPhaseResult(
                rentPhase.InterestRate_Stocks_GoodCase,
                rentPhase.InterestRate_Stocks_BadCase,
                rentPhase.InterestRate_Cash,
                rentPhase.DurationInYears,
                rentPhase.NeedsComfort_PerYear,
                rentPhase.NeedsMinimum_PerYear,
                rentPhase.CrashFactor_Stocks_BadCase,
                rentPhase.TaxFactor_Stocks
                );
            Console.WriteLine(rentPhaseResult);

            Assert.Multiple(() =>
            {
                Assert.That(rentPhaseResult.rate_Cash + rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                    Is.EqualTo(rentPhase.NeedsComfort_PerYear).Within(1),
                    "Both good-case rates should sum up to the comfort needs per year.");

                Assert.That(rentPhaseResult.rate_Cash + rentPhaseResult.rateStocks_ExcludedTaxes_BadCase,
                    Is.EqualTo(rentPhase.NeedsMinimum_PerYear).Within(1),
                    "Both bad-case rates should sum up to the minimum needs per year.");

                SimulateRentPhase(
                    rentPhaseResult.total_Cash,
                    rentPhaseResult.total_Stocks,
                    rentPhaseResult.rate_Cash,
                    rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                    rentPhase.InterestRate_Stocks_GoodCase,
                    rentPhase.InterestRate_Cash,
                    rentPhase.DurationInYears,
                    rentPhaseResult.taxesPerYear_GoodCase);

                SimulateRentPhase(
                    rentPhaseResult.total_Cash,
                    rentPhaseResult.total_Stocks * rentPhase.CrashFactor_Stocks_BadCase,
                    rentPhaseResult.rate_Cash,
                    rentPhaseResult.rateStocks_ExcludedTaxes_BadCase,
                    rentPhase.InterestRate_Stocks_BadCase,
                    rentPhase.InterestRate_Cash,
                    rentPhase.DurationInYears,
                    rentPhaseResult.taxesPerYear_BadCase);
            });
        }

        private static void SimulateRentPhase(decimal total_Cash, decimal total_Stocks, decimal rate_Cash, decimal rateStocks_ExcludedTaxes, decimal interestRate_Stocks, decimal interestRate_Cash, int durationInYears, decimal taxesPerYear)
        {
            Console.WriteLine(Environment.NewLine);
            for (int i = 0; i < durationInYears; i++)
            {
                Console.WriteLine($"State Begin Year {i}: Cash: {total_Cash:F2} Stocks: {total_Stocks:F2} Total: {total_Stocks + total_Cash:F2}");

                var interests_Cash = total_Cash * interestRate_Cash;
                var interests_Stocks = total_Stocks * interestRate_Stocks;
                total_Cash += interests_Cash;
                total_Stocks += interests_Stocks;
                Console.WriteLine($"\tInterests: Cash: {interests_Cash:F2} Stocks: {interests_Stocks:F2} Total: {interests_Cash + interests_Stocks:F2}");

                total_Cash -= rate_Cash;
                total_Stocks -= rateStocks_ExcludedTaxes;
                Console.WriteLine($"\tWithdrawal: Cash: {interestRate_Cash:F2} Stocks: {rateStocks_ExcludedTaxes:F2} Total: {rate_Cash + rateStocks_ExcludedTaxes:F2}");

                total_Stocks -= taxesPerYear;
                Console.WriteLine($"\tWithdrawal: taxes: {taxesPerYear:F2}");

                Console.WriteLine($"\tEnd Year {i}: Cash: {total_Cash:F2} Stocks: {total_Stocks:F2} Total: {total_Stocks + total_Cash:F2}");
            }

            Assert.That(total_Cash, Is.EqualTo(0).Within(1), $"{nameof(total_Cash)} after Simulation.");
            Assert.That(total_Stocks, Is.EqualTo(0).Within(1), $"{nameof(total_Stocks)} after Simulation.");
        }

        private static RentPhaseInputData[] rentPhaseInputDataList = new RentPhaseInputData[]
        {
            new RentPhaseInputData(
                ageCurrent:42,
                ageRentStart:67,
                ageEnd:80,
                inflationRate:1.03d,
                needsCurrentAgeMinimal_perMonth:1900,
                needsCurrentAgeComfort_perMonth:2600,
                interestRate_Cash:0m,
                interestRate_Stocks_GoodCase:0.06m,
                interestRate_Stocks_BadCase:0.0m,
                crashFactor_Stocks_BadCase:0.5m,
                assumedRent_perMonth:2025,
                taxFactor_Stocks:1.26m
                ),
        };
    }
}