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
        public void CalculateRentPhaseResult_SimulateRentPhase_SavingsEndAtZero(LifeAssumptions lifeAssumptions)
        {
            var (stateRentResult, laterNeedsResult, rentPhaseResult) = CalculateResults(lifeAssumptions);
            Console.WriteLine(rentPhaseResult);

            Assert.Multiple(() =>
            {
                Assert.That(rentPhaseResult.rate_Cash + rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                    Is.EqualTo(laterNeedsResult.needsComfort_AgeRentStart_WithInflation_PerYear).Within(1),
                    "Both good-case rates should sum up to the comfort needs per year.");

                Assert.That(rentPhaseResult.rate_Cash + rentPhaseResult.rateStocks_ExcludedTaxes_BadCase,
                    Is.EqualTo(laterNeedsResult.needsMinimum_AgeRentStart_WithInflation_PerYear).Within(1),
                    "Both bad-case rates should sum up to the minimum needs per year.");

                //good scenario
                SimulateRentPhase(
                    rentPhaseResult.total_Cash,
                    rentPhaseResult.total_Stocks,
                    rentPhaseResult.rate_Cash,
                    rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                    lifeAssumptions.rentPhase_InterestRate_Cash,
                    lifeAssumptions.ageEnd - lifeAssumptions.ageRentStart,
                    rentPhaseResult.taxesPerYear_GoodCase);

                //crash scenario
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

        //[Test, TestCaseSource(nameof(lifeAssumptionsList))]
        //public void CalculateRentPhaseResult_ChangeAgeRentStart_ResultsChangeInRightDirection(LifeAssumptions lifeAssumptions)
        //{

        //    if ((lifeAssumptions.ageStopWork + 1 >= lifeAssumptions.ageRentStart) ||
        //            (lifeAssumptions.ageEnd - 1 <= lifeAssumptions.ageRentStart))
        //    {
        //        throw new Exception($"Cannot execute test with the followin age settings: " +
        //            $"{nameof(lifeAssumptions.ageStopWork)}: {lifeAssumptions.ageStopWork}, " +
        //            $"{nameof(lifeAssumptions.ageRentStart)}: {lifeAssumptions.ageRentStart}, " +
        //            $"{nameof(lifeAssumptions.ageEnd)}: {lifeAssumptions.ageEnd}");
        //    }

        //    var (stateRentResult, laterNeedsResult, rentPhaseResult) = CalculateResults(lifeAssumptions);
        //    lifeAssumptions.ageRentStart -= 1;
        //    var (stateRentResult_minus1, laterNeedsResult_minus1, rentPhaseResult_minus1) = CalculateResults(lifeAssumptions);
        //    lifeAssumptions.ageRentStart += 2;
        //    var (stateRentResult_plus1, laterNeedsResult_plus1, rentPhaseResult_plus1) = CalculateResults(lifeAssumptions);

        //    // if i have to work longer until rent start
        //    Assert.Multiple(() =>
        //    {
        //        // I will get more net state rent
        //        Assert.That(stateRentResult_minus1.assumedStateRent_FromStopWorkAge_PerMonth, Is.LessThan(stateRentResult.assumedStateRent_FromStopWorkAge_PerMonth));
        //        Assert.That(stateRentResult_plus1.assumedStateRent_FromStopWorkAge_PerMonth, Is.GreaterThan(stateRentResult.assumedStateRent_FromStopWorkAge_PerMonth));

        //        // I will need more money per month at rent start because inflation kicks more
        //        Assert.That(laterNeedsResult_minus1.needsMinimum_AgeRentStart_WithInflation_PerMonth, Is.LessThan(laterNeedsResult.needsMinimum_AgeRentStart_WithInflation_PerMonth));
        //        Assert.That(laterNeedsResult_plus1.needsMinimum_AgeRentStart_WithInflation_PerMonth, Is.GreaterThan(laterNeedsResult.needsMinimum_AgeRentStart_WithInflation_PerMonth));
        //        Assert.That(laterNeedsResult_minus1.needsComfort_AgeRentStart_WithInflation_PerMonth, Is.LessThan(laterNeedsResult.needsComfort_AgeRentStart_WithInflation_PerMonth));
        //        Assert.That(laterNeedsResult_plus1.needsComfort_AgeRentStart_WithInflation_PerMonth, Is.GreaterThan(laterNeedsResult.needsComfort_AgeRentStart_WithInflation_PerMonth));

        //        // I will need more savings a
        //    });

        //}

        private (StateRentResult, LaterNeedsResult, RentPhaseResult) CalculateResults(LifeAssumptions lifeAssumptions)
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

            return (stateRentResult, laterNeedsResult, rentPhaseResult);
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