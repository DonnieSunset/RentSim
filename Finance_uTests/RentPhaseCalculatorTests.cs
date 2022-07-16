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
        private int myDummyAgeStopWork = 60;

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
                    rentPhaseResult.neededPhaseBegin_Cash,
                    rentPhaseResult.neededPhaseBegin_Stocks,
                    rentPhaseResult.rate_Cash,
                    rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                    lifeAssumptions.rentPhase_InterestRate_Cash,
                    1,
                    rentPhaseResult.taxesPerYear_GoodCase,
                    protoWriterGoodCase);

                var finalRowGoodCase = protoWriterGoodCase.Protocol.Single(x => x.age == lifeAssumptions.ageEnd-1);
                Assert.That(finalRowGoodCase.cashYearEnd, Is.EqualTo(0).Within(1), $"{nameof(finalRowGoodCase.cashYearEnd)} after Simulation, good case.");
                Assert.That(finalRowGoodCase.stocksYearEnd, Is.EqualTo(0).Within(1), $"{nameof(finalRowGoodCase.stocksYearEnd)} after Simulation, good case.");

                MemoryProtocolWriter protoWriterBadCase = new();

                //crash scenario
                RentPhaseCalculator.Simulate(
                    lifeAssumptions.ageRentStart,
                    lifeAssumptions.ageEnd,
                    rentPhaseResult.neededPhaseBegin_Cash,
                    rentPhaseResult.neededPhaseBegin_Stocks,
                    rentPhaseResult.rate_Cash,
                    rentPhaseResult.rateStocks_ExcludedTaxes_BadCase,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_BadCase,
                    lifeAssumptions.rentPhase_InterestRate_Cash,
                    lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                    rentPhaseResult.taxesPerYear_BadCase,
                    protoWriterBadCase);

                var finalRowBadCase = protoWriterBadCase.Protocol.Single(x => x.age == lifeAssumptions.ageEnd-1);
                Assert.That(finalRowBadCase.cashYearEnd, Is.EqualTo(0).Within(1), $"{nameof(finalRowBadCase.cashYearEnd)} after Simulation, bad case.");
                Assert.That(finalRowBadCase.stocksYearEnd, Is.EqualTo(0).Within(1), $"{nameof(finalRowBadCase.stocksYearEnd)} after Simulation, bad case.");
            });
        }

        private (StateRentResult, LaterNeedsResult, RentPhaseResult) CalculateResults(LifeAssumptions lifeAssumptions)
        {
            var stateRentResult = RentPhaseCalculator.ApproxStateRent(
                lifeAssumptions.ageCurrent,
                lifeAssumptions.netStateRentFromCurrentAge_perMonth,
                lifeAssumptions.ageRentStart,
                lifeAssumptions.netStateRentFromRentStartAge_perMonth,
                myDummyAgeStopWork
            );

            var laterNeedsResult = RentPhaseCalculator.CalculateLaterNeeds(
                lifeAssumptions.ageCurrent,
                myDummyAgeStopWork,
                lifeAssumptions.ageRentStart,
                lifeAssumptions.inflationRate,
                lifeAssumptions.needsCurrentAgeMinimal_perMonth,
                lifeAssumptions.needsCurrentAgeComfort_perMonth,
                stateRentResult.assumedStateRent_FromStopWorkAge_PerMonth
            );

            var rentPhaseResult = RentPhaseCalculator.CalculateResult(
                lifeAssumptions.ageEnd - lifeAssumptions.ageRentStart,
                laterNeedsResult.NeedsComfort_AgeRentStart_WithInflation_PerYear,
                laterNeedsResult.NeedsMinimum_AgeRentStart_WithInflation_PerYear,
                lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                lifeAssumptions.rentPhase_InterestRate_Stocks_BadCase,
                lifeAssumptions.rentPhase_InterestRate_Cash,
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