using Domain;
using Finance;
using NUnit.Framework;

namespace Finance_iTests
{
    public class IntegratorTests
    {
        [Test]
        public void TheIntegrator()
        {
            var lifeAssumptions = new LifeAssumptions()
            {
                ageCurrent = 42,
                //ageStopWork = 64,
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


            for (int ageStopWorkAssumed = lifeAssumptions.ageCurrent; ageStopWorkAssumed < lifeAssumptions.ageRentStart; ageStopWorkAssumed++)
            {
                var savingPhaseResult = SavingPhaseCalculator.CalculateResult(
                    lifeAssumptions.ageCurrent,
                    ageStopWorkAssumed,
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

                var stateRentResult = RentPhaseCalculator.ApproxStateRent(
                    lifeAssumptions.ageCurrent,
                    lifeAssumptions.netStateRentFromCurrentAge_perMonth,
                    lifeAssumptions.ageRentStart,
                    lifeAssumptions.netStateRentFromRentStartAge_perMonth,
                    ageStopWorkAssumed
                );

                var laterNeedsResult = RentPhaseCalculator.CalculateLaterNeeds(
                    lifeAssumptions.ageCurrent,
                    ageStopWorkAssumed,
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

                var rentPhaseResult_WithNeedsFromStopWorkPhase = RentPhaseCalculator.CalculateResult(
                    lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_BadCase,
                    lifeAssumptions.rentPhase_InterestRate_Cash,
                    lifeAssumptions.ageEnd - lifeAssumptions.ageRentStart,
                    laterNeedsResult.NeedsComfort_AgeStopWork_WithInflation_PerYear,
                    laterNeedsResult.NeedsMinimum_AgeStopWork_WithInflation_PerYear,
                    lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                    lifeAssumptions.taxFactor_Stocks
                    );

                //TODO:
                //here we still calculate with the rates from the rent phase.
                //but these rates here have to be much higher becasue we dont get a state rent yet.
                var stopWorkPhaseResult = StopWorkPhaseCalculator.Calculate(
                    ageStopWorkAssumed,
                    lifeAssumptions.ageRentStart,
                    rentPhaseResult.total_Cash,
                    rentPhaseResult.total_Stocks,
                    rentPhaseResult_WithNeedsFromStopWorkPhase.rate_Cash,
                    rentPhaseResult_WithNeedsFromStopWorkPhase.rateStocks_ExcludedTaxes_GoodCase,
                    rentPhaseResult_WithNeedsFromStopWorkPhase.rateStocks_ExcludedTaxes_BadCase,
                    lifeAssumptions.rentPhase_InterestRate_Cash,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_BadCase,
                    lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                    lifeAssumptions.taxFactor_Stocks
                    );

                if (savingPhaseResult.SavingsTotal >= stopWorkPhaseResult.NeededTotal)
                {
                    Console.WriteLine(savingPhaseResult);
                    Console.WriteLine(stopWorkPhaseResult);
                    Console.WriteLine($"{Environment.NewLine}");

                    break;
                }
            }
        }
    }
}