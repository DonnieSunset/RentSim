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
        private int myDummyAgeStopWork = 60;
        /// <summary>
        /// This is not a real test because it uses product code in order to validate product code.
        /// </summary>
        [Test, TestCaseSource(nameof(inputData))]
        public void CalculateStopWorkPhaseResult_SimulateStopWorkPhase(LifeAssumptions lifeAssumptions, SavingPhaseResult savingPhaseResult, RentPhaseResult rentPhaseResult)
        {
            var stopWorkPhaseResult = StopWorkPhaseCalculator.Calculate(
                myDummyAgeStopWork,
                lifeAssumptions.ageRentStart,
                rentPhaseResult.neededPhaseBegin_Cash,
                rentPhaseResult.neededPhaseBegin_Stocks,
                rentPhaseResult.rate_Cash,
                rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                rentPhaseResult.rateStocks_ExcludedTaxes_BadCase,
                lifeAssumptions.rentPhase_InterestRate_Cash,
                lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                lifeAssumptions.rentPhase_InterestRate_Stocks_BadCase,
                lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                lifeAssumptions.taxFactor_Stocks
                );

            //todo: make this below 2 assert, according to rent phase tests
            //Assert.That(rentPhaseResult.rate_Cash + rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
            //       Is.EqualTo(laterNeedsResult.NeedsComfort_AgeRentStart_WithInflation_PerYear).Within(1),
            //       "Both good-case rates should sum up to the comfort needs per year.");

            //Assert.That(rentPhaseResult.rate_Cash + rentPhaseResult.rateStocks_ExcludedTaxes_BadCase,
            //    Is.EqualTo(laterNeedsResult.NeedsMinimum_AgeRentStart_WithInflation_PerYear).Within(1),
            //    "Both bad-case rates should sum up to the minimum needs per year.");

            MemoryProtocolWriter protoWriter = new();
            StopWorkPhaseCalculator.Simulate(
                stopWorkPhaseResult.ageStopWork,
                lifeAssumptions.ageRentStart,
                stopWorkPhaseResult.neededPhaseBegin_Cash,
                stopWorkPhaseResult.neededPhaseBegin_Stocks,
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

            Assert.That(protocolEntryResult.cashYearEnd, Is.EqualTo(rentPhaseResult.neededPhaseBegin_Cash).Within(0.1));
            Assert.That(protocolEntryResult.stocksYearEnd, Is.EqualTo(rentPhaseResult.neededPhaseBegin_Stocks).Within(0.1));
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
                    neededPhaseBegin_Cash = 200000,
                    neededPhaseBegin_Stocks = 200000,
                    rate_Cash = 10000,
                    rateStocks_ExcludedTaxes_GoodCase = 10000,
                    rateStocks_ExcludedTaxes_BadCase = 5000,
                }
            },
        };
    }
}