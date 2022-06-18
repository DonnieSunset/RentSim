using Domain;
using Finance;
using Protocol;
using Finance.Results;
using NUnit.Framework;


namespace Finance_uTests
{
    [TestFixture]
    public class SavingsPhaseCalculatorTests
    {
        /// <summary>
        /// This is not a real test because it uses product code in order to validate product code.
        /// </summary>
        [Test, TestCaseSource(nameof(lifeAssumptionsList))]
        public void CalculateSavingsPhaseResult_SimulateSavingsdPhase_SavingsEndCorrect(LifeAssumptions lifeAssumptions)
        {
            var savingsPhaseResult = CalculateResults(lifeAssumptions);
            Console.WriteLine(savingsPhaseResult);

            Assert.Multiple(() =>
            {
                //Assert.That(rentPhaseResult.rate_Cash + rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                //    Is.EqualTo(laterNeedsResult.needsComfort_AgeRentStart_WithInflation_PerYear).Within(1),
                //    "Both good-case rates should sum up to the comfort needs per year.");

                //Assert.That(rentPhaseResult.rate_Cash + rentPhaseResult.rateStocks_ExcludedTaxes_BadCase,
                //    Is.EqualTo(laterNeedsResult.needsMinimum_AgeRentStart_WithInflation_PerYear).Within(1),
                //    "Both bad-case rates should sum up to the minimum needs per year.");

                MemoryProtocolWriter protoWriter = new();

                //good scenario
                SavingPhaseCalculator.Simulate(
                    lifeAssumptions.ageCurrent,
                    lifeAssumptions.ageStopWork,
                    lifeAssumptions.cash,
                    lifeAssumptions.cashGrowthRate,
                    lifeAssumptions.cashSaveAmountPerMonth,
                    lifeAssumptions.stocks,
                    lifeAssumptions.stocksGrowthRate,
                    lifeAssumptions.stocksSaveAmountPerMonth,
                    lifeAssumptions.metals,
                    lifeAssumptions.metalsGrowthRate,
                    lifeAssumptions.metalsSaveAmountPerMonth,
                    protoWriter);

                var finalRowGoodCase = protoWriter.Protocol.Single(x => x.age == lifeAssumptions.ageStopWork - 1);
                Assert.That(finalRowGoodCase.cashYearEnd, Is.EqualTo(savingsPhaseResult.savingsCash).Within(1), $"{nameof(finalRowGoodCase.cashYearEnd)} after Simulation.");
                Assert.That(finalRowGoodCase.stocksYearEnd, Is.EqualTo(savingsPhaseResult.savingsStocks).Within(1), $"{nameof(finalRowGoodCase.stocksYearEnd)} after Simulation.");
                Assert.That(finalRowGoodCase.metalsYearEnd, Is.EqualTo(savingsPhaseResult.savingsMetals).Within(1), $"{nameof(finalRowGoodCase.metalsYearEnd)} after Simulation.");

                Console.Write("");
                //todo assert protocol
            });
        }

        private SavingPhaseResult CalculateResults(LifeAssumptions lifeAssumptions)
        {
            var savingPhaseResult = SavingPhaseCalculator.CalculateResult(
                lifeAssumptions.ageCurrent,
                lifeAssumptions.ageStopWork,
                lifeAssumptions.cash,
                lifeAssumptions.cashGrowthRate,
                lifeAssumptions.cashSaveAmountPerMonth,
                lifeAssumptions.stocks,
                lifeAssumptions.stocksGrowthRate,
                lifeAssumptions.stocksSaveAmountPerMonth,
                lifeAssumptions.metals,
                lifeAssumptions.metalsGrowthRate,
                lifeAssumptions.metalsSaveAmountPerMonth
                );

            return savingPhaseResult;
        }

        private static LifeAssumptions[] lifeAssumptionsList = new LifeAssumptions[]
{
            new LifeAssumptions() {
                ageCurrent = 42,
                ageStopWork = 60,
                ageRentStart = 67,
                ageEnd = 80,
                
                stocks = 88800,
                stocksGrowthRate = 7,
                stocksSaveAmountPerMonth = 700,

                cash = 58000,
                cashGrowthRate = 0,
                cashSaveAmountPerMonth = 350,

                metals = 21400,
                metalsGrowthRate = 1,
                metalsSaveAmountPerMonth = 0,
            }
        };
    }
}