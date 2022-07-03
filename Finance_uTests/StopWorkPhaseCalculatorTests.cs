using Domain;
using Finance;
using Protocol;
using Finance.Results;
using NUnit.Framework;


namespace Finance_uTests
{
    [TestFixture]
    public class StopWorkPhaseCalculatorTests
    {
        /// <summary>
        /// This is not a real test because it uses product code in order to validate product code.
        /// </summary>
        [Test, TestCaseSource(nameof(inputData))]
        public void CalculateStopWorkPhaseResult_SimulateStopWorkPhase(LifeAssumptions lifeAssumptions, SavingPhaseResult savingPhaseResult, RentPhaseResult rentPhaseResult)
        {
            var stopWorkPhaseResult = StopWorkPhaseCalculator.Calculate(
                lifeAssumptions.ageStopWork,
                lifeAssumptions.ageRentStart,
                rentPhaseResult.total_Cash,
                rentPhaseResult.total_Stocks,
                rentPhaseResult.rate_Cash,
                rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                rentPhaseResult.rateStocks_ExcludedTaxes_BadCase,
                lifeAssumptions.rentPhase_InterestRate_Cash,
                lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                lifeAssumptions.rentPhase_InterestRate_Stocks_BadCase,
                lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                lifeAssumptions.taxFactor_Stocks
                );

            MemoryProtocolWriter protoWriter = new();
            StopWorkPhaseCalculator.Simulate(
                stopWorkPhaseResult.ageStopWork,
                lifeAssumptions.ageRentStart,
                stopWorkPhaseResult.neededCash,
                stopWorkPhaseResult.neededStocks,
                rentPhaseResult.rate_Cash,
                rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                rentPhaseResult.rateStocks_ExcludedTaxes_BadCase,
                lifeAssumptions.rentPhase_InterestRate_Cash,
                lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                lifeAssumptions.rentPhase_InterestRate_Stocks_BadCase,
                lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                lifeAssumptions.taxFactor_Stocks,
                protoWriter
                );

            var protocolEntryResult = protoWriter.Protocol.Single(x => x.age == lifeAssumptions.ageRentStart-1);

            Assert.That(protocolEntryResult.cashYearEnd, Is.EqualTo(rentPhaseResult.total_Cash).Within(0.1));
            Assert.That(protocolEntryResult.stocksYearEnd, Is.EqualTo(rentPhaseResult.total_Stocks).Within(0.1));
        }

        //private static object[] inputData =
        //{
        //    new object[]
        //    {
        //        new LifeAssumptions() {
        //            ageCurrent = 42,
        //            ageStopWork = 64,
        //            ageRentStart = 67,
        //            ageEnd = 80,
        //            inflationRate = 0.03d,
        //            needsCurrentAgeMinimal_perMonth = 1900,
        //            needsCurrentAgeComfort_perMonth = 2600,
        //            rentPhase_InterestRate_Cash = 0m,
        //            rentPhase_InterestRate_Stocks_GoodCase = 0.07m,
        //            rentPhase_InterestRate_Stocks_BadCase = 0.0m,
        //            rentPhase_CrashFactor_Stocks_BadCase = 0.5m,
        //            taxFactor_Stocks = 1.26m
        //        },

        //        new SavingPhaseResult() {
        //            savingsCash = 133600,
        //            savingsStocks = 525729,
        //            savingsMetals = 25597,
        //        },

        //        new RentPhaseResult() {
        //            total_Cash = 238984,
        //            total_Stocks = 297463,
        //            rate_Cash = 18383,
        //            rateStocks_ExcludedTaxes_GoodCase = 26668,
        //            rateStocks_ExcludedTaxes_BadCase = 9080,
                    
        //        }
        //    },
        //};

        private static object[] inputData =
        {
            new object[]
            {
                new LifeAssumptions() {
                    ageCurrent = 42,
                    ageStopWork = 64,
                    ageRentStart = 67,
                    ageEnd = 80,
                    inflationRate = 0.03d,
                    needsCurrentAgeMinimal_perMonth = 1900,
                    needsCurrentAgeComfort_perMonth = 2600,
                    rentPhase_InterestRate_Cash = 0.01m,
                    rentPhase_InterestRate_Stocks_GoodCase = 0.07m,
                    rentPhase_InterestRate_Stocks_BadCase = 0.0m,
                    rentPhase_CrashFactor_Stocks_BadCase = 0.5m,
                    taxFactor_Stocks = 1.26m
                },

                new SavingPhaseResult() {
                    savingsCash = 1000000,
                    savingsStocks = 1000000,
                    savingsMetals = 1000000,
                },

                new RentPhaseResult() {
                    total_Cash = 200000,
                    total_Stocks = 200000,
                    rate_Cash = 10000,
                    rateStocks_ExcludedTaxes_GoodCase = 10000,
                    rateStocks_ExcludedTaxes_BadCase = 5000,

                }
            },
        };
    
    }
}