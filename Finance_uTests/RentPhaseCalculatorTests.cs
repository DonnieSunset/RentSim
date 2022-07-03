using Domain;
using Finance;
using Protocol;
using Finance.Results;
using NUnit.Framework;


namespace Finance_uTests
{
    [TestFixture]
    public class RentPhaseCalculatorTests
    {
        /// <summary>
        /// This is not a real test because it uses product code in order to validate product code.
        /// </summary>
        [Test, TestCaseSource(nameof(lifeAssumptionsList))]
        public void CalculateRentPhaseResult_SimulateRentPhase_SavingsEndAtZero(LifeAssumptions lifeAssumptions)
        {
            var (stateRentResult, laterNeedsResult, rentPhaseResult) = CalculateResults(lifeAssumptions);
            Console.WriteLine(rentPhaseResult);

            Assert.Multiple(() =>
            {
                Assert.That(rentPhaseResult.rate_Cash + rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                    Is.EqualTo(laterNeedsResult.NeedsComfort_AgeRentStart_WithInflation_PerYear).Within(1),
                    "Both good-case rates should sum up to the comfort needs per year.");

                Assert.That(rentPhaseResult.rate_Cash + rentPhaseResult.rateStocks_ExcludedTaxes_BadCase,
                    Is.EqualTo(laterNeedsResult.NeedsMinimum_AgeRentStart_WithInflation_PerYear).Within(1),
                    "Both bad-case rates should sum up to the minimum needs per year.");

                MemoryProtocolWriter protoWriterGoodCase = new();

                //good scenario
                RentPhaseCalculator.Simulate(
                    lifeAssumptions.ageRentStart,
                    lifeAssumptions.ageEnd,
                    rentPhaseResult.total_Cash,
                    rentPhaseResult.total_Stocks,
                    rentPhaseResult.rate_Cash,
                    rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                    lifeAssumptions.rentPhase_InterestRate_Cash,
                    rentPhaseResult.taxesPerYear_GoodCase,
                    protoWriterGoodCase);

                var finalRowGoodCase = protoWriterGoodCase.Protocol.Single(x => x.age == lifeAssumptions.ageEnd-1);
                Assert.That(finalRowGoodCase.cashYearEnd, Is.EqualTo(0).Within(1), $"{nameof(finalRowGoodCase.cashYearEnd)} after Simulation.");
                Assert.That(finalRowGoodCase.stocksYearEnd, Is.EqualTo(0).Within(1), $"{nameof(finalRowGoodCase.stocksYearEnd)} after Simulation.");

                MemoryProtocolWriter protoWriterBadCase = new();

                //crash scenario
                RentPhaseCalculator.Simulate(
                    lifeAssumptions.ageRentStart,
                    lifeAssumptions.ageEnd,
                    rentPhaseResult.total_Cash,
                    rentPhaseResult.total_Stocks * lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                    rentPhaseResult.rate_Cash,
                    rentPhaseResult.rateStocks_ExcludedTaxes_BadCase,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_BadCase,
                    lifeAssumptions.rentPhase_InterestRate_Cash,
                    rentPhaseResult.taxesPerYear_BadCase,
                    protoWriterBadCase);

                var finalRowBadCase = protoWriterBadCase.Protocol.Single(x => x.age == lifeAssumptions.ageEnd-1);
                Assert.That(finalRowBadCase.cashYearEnd, Is.EqualTo(0).Within(1), $"{nameof(finalRowBadCase.cashYearEnd)} after Simulation.");
                Assert.That(finalRowBadCase.stocksYearEnd, Is.EqualTo(0).Within(1), $"{nameof(finalRowBadCase.stocksYearEnd)} after Simulation.");

                Console.Write("");
                //todo assert protocol
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
            var stateRentResult = RentPhaseCalculator.ApproxStateRent(
                lifeAssumptions.ageCurrent,
                lifeAssumptions.netStateRentFromCurrentAge_perMonth,
                lifeAssumptions.ageRentStart,
                lifeAssumptions.netStateRentFromRentStartAge_perMonth,
                lifeAssumptions.ageStopWork
            );

            var laterNeedsResult = RentPhaseCalculator.CalculateLaterNeeds(
                lifeAssumptions.ageCurrent,
                lifeAssumptions.ageStopWork,
                lifeAssumptions.ageRentStart,
                lifeAssumptions.inflationRate,
                lifeAssumptions.needsCurrentAgeMinimal_perMonth,
                lifeAssumptions.needsCurrentAgeComfort_perMonth,
                stateRentResult.assumedStateRent_FromStopWorkAge_PerMonth
            );

            var rentPhaseResult = RentPhaseCalculator.CalculateResult(
                lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                lifeAssumptions.rentPhase_InterestRate_Stocks_BadCase,
                lifeAssumptions.rentPhase_InterestRate_Cash,
                lifeAssumptions.ageEnd - lifeAssumptions.ageRentStart,
                laterNeedsResult.NeedsComfort_AgeRentStart_WithInflation_PerYear,
                laterNeedsResult.NeedsMinimum_AgeRentStart_WithInflation_PerYear,
                lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                lifeAssumptions.taxFactor_Stocks
                );

            return (stateRentResult, laterNeedsResult, rentPhaseResult);
        }

        private static LifeAssumptions[] lifeAssumptionsList = new LifeAssumptions[]
{
            new LifeAssumptions() {
                ageCurrent = 42,
                ageRentStart = 67,
                ageEnd = 80,
                inflationRate = 0.03d,
                needsCurrentAgeMinimal_perMonth = 1900,
                needsCurrentAgeComfort_perMonth = 2600,
                rentPhase_InterestRate_Cash = 0m,
                rentPhase_InterestRate_Stocks_GoodCase = 0.06m,
                rentPhase_InterestRate_Stocks_BadCase = 0.0m,
                rentPhase_CrashFactor_Stocks_BadCase = 0.5m,
                taxFactor_Stocks = 1.26m
            }
        };
    }
}