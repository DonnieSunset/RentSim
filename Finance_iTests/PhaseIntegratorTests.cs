using Domain;
using Finance;
using NUnit.Framework;
using Protocol;

namespace Finance_iTests
{
    public class PhaseIntegratorTests
    {
        [Test]
        public void ResultRowValidator_DefaultLifeAssumptions_ValidationSucceeds()
        {
            var lifeAssumptions = new LifeAssumptions()
            {
                ageCurrent = 42,
                ageRentStart = 67,
                ageEnd = 80,
                inflationRate = 0.03d,
                needsCurrentAgeMinimal_perMonth = 1900,
                needsCurrentAgeComfort_perMonth = 2600,
                rentPhase_InterestRate_Cash = 0m,
                rentPhase_InterestRate_Stocks_GoodCase = 0.07m,
                rentPhase_InterestRate_Stocks_BadCase = 0.0m,
                rentPhase_CrashFactor_Stocks_BadCase = 0.5m,
                taxFactor_Stocks = 1.26m
            };

            var phaseIntegratorResult = PhaseIntegrator.Doit(lifeAssumptions);
            phaseIntegratorResult.Print();

            var savingPhaseResult = phaseIntegratorResult.savingPhaseResult;
            var rentPhaseResult = phaseIntegratorResult.rentPhaseResult;
            var stopWorkPhaseResult = phaseIntegratorResult.stopWorkPhaseResult;

            IProtocolWriter protocolWriter = new MemoryProtocolWriter();

            SavingPhaseCalculator.Simulate(
                lifeAssumptions.ageCurrent,
                phaseIntegratorResult.ageStopWork,
                lifeAssumptions.cash,
                lifeAssumptions.cashGrowthRate,
                lifeAssumptions.cashSaveAmountPerMonth,
                lifeAssumptions.stocks,
                lifeAssumptions.stocksGrowthRate,
                lifeAssumptions.stocksSaveAmountPerMonth,
                lifeAssumptions.metals,
                lifeAssumptions.metalsGrowthRate,
                lifeAssumptions.metalsSaveAmountPerMonth,
                protocolWriter
            );

            SavingPhaseCalculator.RebalanceForStopWorkPhase(
                phaseIntegratorResult.ageStopWork-1,
                phaseIntegratorResult.overAmount,
                stopWorkPhaseResult.neededCash,
                stopWorkPhaseResult.neededStocks,
                protocolWriter
                );

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
                phaseIntegratorResult.overAmount,
                protocolWriter
            );

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
                protocolWriter
            );

            var resultRows = protocolWriter.Protocol;

            Assert.That(() => ResultRowValidator.ValidateAll(resultRows, lifeAssumptions.ageCurrent, lifeAssumptions.ageEnd),
                Throws.Nothing);
        }
    }
}