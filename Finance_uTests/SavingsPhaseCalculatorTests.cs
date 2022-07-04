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
        private int myDummyAgeStopWork = 60;

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
                MemoryProtocolWriter protoWriter = new();

                //good scenario
                SavingPhaseCalculator.Simulate(
                    lifeAssumptions.ageCurrent,
                    myDummyAgeStopWork,
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

                var finalRowGoodCase = protoWriter.Protocol.Single(x => x.age == myDummyAgeStopWork - 1);
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
                myDummyAgeStopWork,
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