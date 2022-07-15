using Domain;
using Finance.Results;
using Protocol;

namespace Finance
{
    public class PhaseIntegrator
    {
        public static PhaseIntegratorResult Calculate(LifeAssumptions lifeAssumptions)
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
                    lifeAssumptions.ageEnd - lifeAssumptions.ageRentStart,
                    laterNeedsResult.NeedsComfort_AgeRentStart_WithInflation_PerYear,
                    laterNeedsResult.NeedsMinimum_AgeRentStart_WithInflation_PerYear,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_BadCase,
                    lifeAssumptions.rentPhase_InterestRate_Cash,
                    lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                    lifeAssumptions.taxFactor_Stocks
                    );

                var rentPhaseResult_WithNeedsFromStopWorkPhase = RentPhaseCalculator.CalculateResult(
                    lifeAssumptions.ageEnd - lifeAssumptions.ageRentStart,
                    laterNeedsResult.NeedsComfort_AgeStopWork_WithInflation_PerYear,
                    laterNeedsResult.NeedsMinimum_AgeStopWork_WithInflation_PerYear,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_BadCase,
                    lifeAssumptions.rentPhase_InterestRate_Cash,
                    lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                    lifeAssumptions.taxFactor_Stocks
                    );

                var stopWorkPhaseResult = StopWorkPhaseCalculator.Calculate(
                    ageStopWorkAssumed,
                    lifeAssumptions.ageRentStart,
                    //rentPhaseResult.total_Cash,
                    //rentPhaseResult.total_Stocks,
                    //rentPhaseResult.rate_Cash,
                    //rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                    //rentPhaseResult.rateStocks_ExcludedTaxes_BadCase,
                    rentPhaseResult.neededPhaseBegin_Cash,
                    rentPhaseResult.neededPhaseBegin_Stocks,
                    rentPhaseResult_WithNeedsFromStopWorkPhase.rate_Cash,
                    rentPhaseResult_WithNeedsFromStopWorkPhase.rateStocks_ExcludedTaxes_GoodCase,
                    rentPhaseResult_WithNeedsFromStopWorkPhase.rateStocks_ExcludedTaxes_BadCase,
                    lifeAssumptions.rentPhase_InterestRate_Cash,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_BadCase,
                    lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                    lifeAssumptions.taxFactor_Stocks
                    );

                if (savingPhaseResult.SavingsTotal >= stopWorkPhaseResult.NeededPhaseBegin_Total)
                {
                    //Console.WriteLine(savingPhaseResult);
                    //Console.WriteLine(stopWorkPhaseResult);
                    //Console.WriteLine($"{Environment.NewLine}");

                    laterNeedsResult.Print();
                    rentPhaseResult.Print();
                    rentPhaseResult_WithNeedsFromStopWorkPhase.Print();


                    return new PhaseIntegratorResult()
                    {
                        ageStopWork = ageStopWorkAssumed,
                        overAmount = savingPhaseResult.SavingsTotal - stopWorkPhaseResult.NeededPhaseBegin_Total,

                        savingPhaseResult = savingPhaseResult,
                        stateRentResult = stateRentResult,
                        laterNeedsResult = laterNeedsResult,
                        rentPhaseResult = rentPhaseResult,
                        stopWorkPhaseResult = stopWorkPhaseResult
                    };
                };
            }

            throw new Exception("Could not determine stop work age");
        }

        public static void Simulate(
            LifeAssumptions lifeAssumptions,
            PhaseIntegratorResult phaseIntegratorResult,
            IProtocolWriter protocolWriter)
        {
            StopWorkPhaseResult stopWorkPhaseResult = phaseIntegratorResult.stopWorkPhaseResult;
            RentPhaseResult rentPhaseResult = phaseIntegratorResult.rentPhaseResult;

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
                phaseIntegratorResult.ageStopWork - 1, // <-- todo: what happens here if currentAge==stopWorkAge?
                phaseIntegratorResult.overAmount,
                stopWorkPhaseResult.neededPhaseBegin_Cash,
                stopWorkPhaseResult.neededPhaseBegin_Stocks,
                taxesStocks,
                protocolWriter
                );

            StopWorkPhaseCalculator.Simulate(
                stopWorkPhaseResult.ageStopWork,
                lifeAssumptions.ageRentStart,
                stopWorkPhaseResult.neededPhaseBegin_Cash,
                stopWorkPhaseResult.neededPhaseBegin_Stocks,
                stopWorkPhaseResult.rate_Cash,
                stopWorkPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                stopWorkPhaseResult.rateStocks_ExcludedTaxes_BadCase,
                lifeAssumptions.rentPhase_InterestRate_Cash,
                lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
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
                rentPhaseResult.rateStocks_ExcludedTaxes_GoodCase,
                lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                lifeAssumptions.rentPhase_InterestRate_Cash,
                rentPhaseResult.taxesPerYear_GoodCase,
                protocolWriter
            );
        }
    }
}
