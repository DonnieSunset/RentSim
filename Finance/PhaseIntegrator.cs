using Domain;
using Finance.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance
{
    public class PhaseIntegrator
    {
        public static PhaseIntegratorResult Doit(LifeAssumptions lifeAssumptions)
        {
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

                var stopWorkPhaseResult = StopWorkPhaseCalculator.Calculate(
                    ageStopWorkAssumed,
                    lifeAssumptions.ageRentStart,
                    rentPhaseResult.total_Cash,
                    rentPhaseResult.total_Stocks,
                    rentPhaseResult.rate_Cash,
                    rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                    rentPhaseResult.rateStocks_ExcludedTaxes_BadCase,
                    //rentPhaseResult_WithNeedsFromStopWorkPhase.rate_Cash,
                    //rentPhaseResult_WithNeedsFromStopWorkPhase.rateStocks_ExcludedTaxes_GoodCase,
                    //rentPhaseResult_WithNeedsFromStopWorkPhase.rateStocks_ExcludedTaxes_BadCase,
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

                    return new PhaseIntegratorResult()
                    {
                        ageStopWork = ageStopWorkAssumed,
                        overAmount = savingPhaseResult.SavingsTotal - stopWorkPhaseResult.NeededTotal,

                        savingPhaseResult = savingPhaseResult,
                        stateRentResult = stateRentResult,
                        laterNeedsResult = laterNeedsResult,
                        rentPhaseResult = rentPhaseResult,
                        //rentPhaseResult = rentPhaseResult_WithNeedsFromStopWorkPhase,
                        stopWorkPhaseResult = stopWorkPhaseResult
                    };
                };
            }

            throw new Exception("Could not determine stop work age");
        }
    }
}
