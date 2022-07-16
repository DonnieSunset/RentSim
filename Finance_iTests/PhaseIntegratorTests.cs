using Domain;
using Finance;
using Finance.Results;
using NUnit.Framework;
using Protocol;

namespace Finance_iTests
{
    public class PhaseIntegratorTests
    {
        public LifeAssumptions lifeAssumptions;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            lifeAssumptions = new LifeAssumptions()
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
        }

        private IReadOnlyCollection<ResultRow> SimulateGoodCase(PhaseIntegratorResult phaseIntegratorResult)
        {
            var savingPhaseResult = phaseIntegratorResult.savingPhaseResult;
            var rentPhaseResult = phaseIntegratorResult.rentPhaseResult;
            var stopWorkPhaseResult_goodCase = phaseIntegratorResult.stopWorkPhaseResult_goodCase;

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

            Frac taxesStocks = Frac.FromFactor(lifeAssumptions.taxFactor_Stocks);
            SavingPhaseCalculator.RebalanceForStopWorkPhase(
                phaseIntegratorResult.ageStopWork - 1,
                phaseIntegratorResult.overAmount,
                stopWorkPhaseResult_goodCase.neededPhaseBegin_Cash,
                stopWorkPhaseResult_goodCase.neededPhaseBegin_Stocks,
                taxesStocks,
                lifeAssumptions.rentPhase_CrashFactor_Stocks_GoodCase,
                protocolWriter
                );

            StopWorkPhaseCalculator.Simulate(
                stopWorkPhaseResult_goodCase.ageStopWork,
                lifeAssumptions.ageRentStart,
                stopWorkPhaseResult_goodCase.neededPhaseBegin_Cash,
                stopWorkPhaseResult_goodCase.neededPhaseBegin_Stocks,
                stopWorkPhaseResult_goodCase.rate_Cash,
                stopWorkPhaseResult_goodCase.rateStocks_ExcludedTaxes,
                lifeAssumptions.rentPhase_InterestRate_Cash,
                lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                lifeAssumptions.rentPhase_CrashFactor_Stocks_GoodCase,
                lifeAssumptions.taxFactor_Stocks,
                protocolWriter
            );

            RentPhaseCalculator.Simulate(
                lifeAssumptions.ageRentStart,
                lifeAssumptions.ageEnd,
                rentPhaseResult.neededPhaseBegin_Cash,
                rentPhaseResult.neededPhaseBegin_Stocks,
                rentPhaseResult.rate_Cash,
                rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                lifeAssumptions.rentPhase_InterestRate_Cash,
                lifeAssumptions.rentPhase_CrashFactor_Stocks_GoodCase,
                rentPhaseResult.taxesPerYear_GoodCase,
                protocolWriter
            );

            return protocolWriter.Protocol;
        }

        private IReadOnlyCollection<ResultRow> SimulateBadCase(PhaseIntegratorResult phaseIntegratorResult)
        {
            var savingPhaseResult = phaseIntegratorResult.savingPhaseResult;
            var rentPhaseResult = phaseIntegratorResult.rentPhaseResult;
            var stopWorkPhaseResult_badCase = phaseIntegratorResult.stopWorkPhaseResult_badCase;

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

            Frac taxesStocks = Frac.FromFactor(lifeAssumptions.taxFactor_Stocks);
            SavingPhaseCalculator.RebalanceForStopWorkPhase(
                phaseIntegratorResult.ageStopWork - 1,
                phaseIntegratorResult.overAmount,
                stopWorkPhaseResult_badCase.neededPhaseBegin_Cash,
                stopWorkPhaseResult_badCase.neededPhaseBegin_Stocks,
                taxesStocks,
                lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                protocolWriter
                );

            StopWorkPhaseCalculator.Simulate(
                stopWorkPhaseResult_badCase.ageStopWork,
                lifeAssumptions.ageRentStart,
                stopWorkPhaseResult_badCase.neededPhaseBegin_Cash,
                stopWorkPhaseResult_badCase.neededPhaseBegin_Stocks,
                stopWorkPhaseResult_badCase.rate_Cash,
                stopWorkPhaseResult_badCase.rateStocks_ExcludedTaxes,
                lifeAssumptions.rentPhase_InterestRate_Cash,
                lifeAssumptions.rentPhase_InterestRate_Stocks_BadCase,
                lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                lifeAssumptions.taxFactor_Stocks,
                protocolWriter
            );

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
                protocolWriter
            );

            return protocolWriter.Protocol;
        }

        [Test]
        public void ResultRowValidator_DefaultLifeAssumptions_GoodCase_ValidationSucceeds()
        {

            var phaseIntegratorResult = PhaseIntegrator.Calculate(lifeAssumptions);
            phaseIntegratorResult.Print();

            var resultRows = SimulateGoodCase(phaseIntegratorResult);

            Assert.That(phaseIntegratorResult.overAmount, Is.Not.GreaterThan(phaseIntegratorResult.laterNeedsResult.NeedsComfort_AgeStopWork_WithInflation_PerYear), 
                "Overamount must not be greater than yearly needs, otherwise stop work age could be even earlier.");

            Assert.That(() => ResultRowValidator.ValidateAll(resultRows, lifeAssumptions.ageCurrent, phaseIntegratorResult.ageStopWork, lifeAssumptions.ageEnd, lifeAssumptions.taxFactor_Stocks),
                Throws.Nothing);
        }

        [Test]
        public void ResultRowValidator_DefaultLifeAssumptions_BadCase_ValidationSucceeds()
        {

            var phaseIntegratorResult = PhaseIntegrator.Calculate(lifeAssumptions);
            phaseIntegratorResult.Print();

            var resultRows = SimulateBadCase(phaseIntegratorResult);

            Assert.That(phaseIntegratorResult.overAmount, Is.Not.GreaterThan(phaseIntegratorResult.laterNeedsResult.NeedsMinimum_AgeStopWork_WithInflation_PerYear),
                "Overamount must not be greater than yearly needs, otherwise stop work age could be even earlier.");

            Assert.That(() => ResultRowValidator.ValidateAll(resultRows, lifeAssumptions.ageCurrent, phaseIntegratorResult.ageStopWork, lifeAssumptions.ageEnd, lifeAssumptions.taxFactor_Stocks),
                Throws.Nothing);
        }
    }
}