using Domain;
using Finance;
using Finance.Results;
using NUnit.Framework;


namespace Finance_uTests
{
    [TestFixture]
    public class FinanceCalculatorTests
    {
        [Test, TestCaseSource(nameof(lifeAssumptionsList))]
        public void CalculateRentPhaseResult_ValidScenario(LifeAssumptions lifeAssumptions)
        {
            var stateRentResult = RentCalculator.ApproxStateRent(
                lifeAssumptions.ageCurrent,
                lifeAssumptions.netStateRentFromCurrentAge_perMonth,
                lifeAssumptions.ageRentStart,
                lifeAssumptions.netStateRentFromRentStartAge_perMonth,
                lifeAssumptions.ageStopWork
            );

            var laterNeedsResult = RentCalculator.CalculateLaterNeeds(
                lifeAssumptions.ageCurrent,
                lifeAssumptions.ageRentStart,
                lifeAssumptions.inflationRate,
                lifeAssumptions.needsCurrentAgeMinimal,
                lifeAssumptions.needsCurrentAgeComfort,
                stateRentResult.assumedStateRent_FromStopWorkAge_PerMonth
            );

            var rentPhaseResult = FinanceCalculator.CalculateRentPhaseResult(
                lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                lifeAssumptions.rentPhase_InterestRate_Stocks_BadCase,
                lifeAssumptions.rentPhase_InterestRate_Cash,
                lifeAssumptions.ageEnd - lifeAssumptions.ageRentStart,
                laterNeedsResult.needsComfort_AgeRentStart_WithInflation_PerYear,
                laterNeedsResult.needsMinimum_AgeRentStart_WithInflation_PerYear,
                lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                lifeAssumptions.taxFactor_Stocks
                );

            Console.WriteLine(rentPhaseResult);

            Assert.Multiple(() =>
            {
                Assert.That(rentPhaseResult.rate_Cash + rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                    Is.EqualTo(laterNeedsResult.needsComfort_AgeRentStart_WithInflation_PerYear).Within(1),
                    "Both good-case rates should sum up to the comfort needs per year.");

                Assert.That(rentPhaseResult.rate_Cash + rentPhaseResult.rateStocks_ExcludedTaxes_BadCase,
                    Is.EqualTo(laterNeedsResult.needsMinimum_AgeRentStart_WithInflation_PerYear).Within(1),
                    "Both bad-case rates should sum up to the minimum needs per year.");

                SimulateRentPhase(
                    rentPhaseResult.total_Cash,
                    rentPhaseResult.total_Stocks,
                    rentPhaseResult.rate_Cash,
                    rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                    lifeAssumptions.rentPhase_InterestRate_Cash,
                    lifeAssumptions.ageEnd - lifeAssumptions.ageRentStart,
                    rentPhaseResult.taxesPerYear_GoodCase);

                SimulateRentPhase(
                    rentPhaseResult.total_Cash,
                    rentPhaseResult.total_Stocks * lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                    rentPhaseResult.rate_Cash,
                    rentPhaseResult.rateStocks_ExcludedTaxes_BadCase,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_BadCase,
                    lifeAssumptions.rentPhase_InterestRate_Cash,
                    lifeAssumptions.ageEnd - lifeAssumptions.ageRentStart,
                    rentPhaseResult.taxesPerYear_BadCase);
            });
        }

        private static void SimulateRentPhase(decimal totalCash, decimal totalStocks, decimal rateCash_perYear, decimal rateStocks_ExcludedTaxes_perYear, decimal interestRate_Stocks, decimal interestRate_Cash, int durationInYears, decimal taxesPerYear)
        {
            Console.WriteLine(Environment.NewLine);
            for (int i = 0; i < durationInYears; i++)
            {
                Console.WriteLine($"State Begin Year {i}: Cash: {totalCash:F2} Stocks: {totalStocks:F2} Total: {totalStocks + totalCash:F2}");

                var interests_Cash = totalCash * interestRate_Cash;
                var interests_Stocks = totalStocks * interestRate_Stocks;
                totalCash += interests_Cash;
                totalStocks += interests_Stocks;
                Console.WriteLine($"\tInterests: Cash: {interests_Cash:F2} Stocks: {interests_Stocks:F2} Total: {interests_Cash + interests_Stocks:F2}");

                totalCash -= rateCash_perYear;
                totalStocks -= rateStocks_ExcludedTaxes_perYear;
                Console.WriteLine($"\tWithdrawal: Cash: {interestRate_Cash:F2} Stocks: {rateStocks_ExcludedTaxes_perYear:F2} Total: {rateCash_perYear + rateStocks_ExcludedTaxes_perYear:F2}");

                totalStocks -= taxesPerYear;
                Console.WriteLine($"\tWithdrawal: taxes: {taxesPerYear:F2}");

                Console.WriteLine($"\tEnd Year {i}: Cash: {totalCash:F2} Stocks: {totalStocks:F2} Total: {totalStocks + totalCash:F2}");
            }

            Assert.That(totalCash, Is.EqualTo(0).Within(1), $"{nameof(totalCash)} after Simulation.");
            Assert.That(totalStocks, Is.EqualTo(0).Within(1), $"{nameof(totalStocks)} after Simulation.");
        }

        //private static RentPhaseInputData[] rentPhaseInputDataList = new RentPhaseInputData[]
        //{
        //    new RentPhaseInputData(
        //        ageCurrent:42,
        //        ageRentStart:67,
        //        ageEnd:80,
        //        inflationRate:0.03d,
        //        needsCurrentAgeMinimal_perMonth:1900,
        //        needsCurrentAgeComfort_perMonth:2600,
        //        interestRate_Cash:0m,
        //        interestRate_Stocks_GoodCase:0.06m,
        //        interestRate_Stocks_BadCase:0.0m,
        //        crashFactor_Stocks_BadCase:0.5m,
        //        assumedRent_perMonth:2025,
        //        taxFactor_Stocks:1.26m
        //        ),
        //};

        private static LifeAssumptions[] lifeAssumptionsList = new LifeAssumptions[]
{
            new LifeAssumptions() {
                ageCurrent = 42,
                ageRentStart = 67,
                ageEnd = 80,
                inflationRate = 0.03d,
                needsCurrentAgeMinimal = 1900,
                needsCurrentAgeComfort = 2600,
                rentPhase_InterestRate_Cash = 0m,
                rentPhase_InterestRate_Stocks_GoodCase = 0.06m,
                rentPhase_InterestRate_Stocks_BadCase = 0.0m,
                rentPhase_CrashFactor_Stocks_BadCase = 0.5m,
                taxFactor_Stocks = 1.26m
            }
        };
    }
}