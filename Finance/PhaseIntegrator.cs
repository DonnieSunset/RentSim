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
                    stateRentResult.assumedStateRent_Net_PerMonth
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

                var stopWorkPhaseResult_goodCase = StopWorkPhaseCalculator.Calculate(
                    ageStopWorkAssumed,
                    lifeAssumptions.ageRentStart,
                    rentPhaseResult.neededPhaseBegin_Cash,
                    rentPhaseResult.neededPhaseBegin_Stocks,
                    rentPhaseResult_WithNeedsFromStopWorkPhase.rate_Cash,
                    rentPhaseResult_WithNeedsFromStopWorkPhase.rateStocks_ExcludedTaxes_GoodCase,
                    lifeAssumptions.rentPhase_InterestRate_Cash,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                    1,
                    lifeAssumptions.taxFactor_Stocks
                    );

                var stopWorkPhaseResult_badCase = StopWorkPhaseCalculator.Calculate(
                    ageStopWorkAssumed,
                    lifeAssumptions.ageRentStart,
                    rentPhaseResult.neededPhaseBegin_Cash,
                    rentPhaseResult.neededPhaseBegin_Stocks,
                    //rentPhaseResult_WithNeedsFromStopWorkPhase.neededPhaseBegin_Cash,
                    //rentPhaseResult_WithNeedsFromStopWorkPhase.neededPhaseBegin_Stocks,
                    rentPhaseResult_WithNeedsFromStopWorkPhase.rate_Cash,
                    rentPhaseResult_WithNeedsFromStopWorkPhase.rateStocks_ExcludedTaxes_BadCase,
                    lifeAssumptions.rentPhase_InterestRate_Cash,
                    lifeAssumptions.rentPhase_InterestRate_Stocks_BadCase,
                    lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                    //1, wäre eine idee. wir haben ja shcon die raten angepasst, damit kommen wir ja schon auf die niedrigeeren werte, daher brauchen wir nicht auf noch den stock market crash simulieren, das wäre dann doppelt gemoppelt.
                    lifeAssumptions.taxFactor_Stocks
                    );

                var overAmount_goodCase = savingPhaseResult.SavingsTotal - stopWorkPhaseResult_goodCase.NeededPhaseBegin_Total;
                var overAmount_badCase = savingPhaseResult.savingsCash + savingPhaseResult.savingsMetals + (savingPhaseResult.savingsStocks * lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase) - stopWorkPhaseResult_badCase.NeededPhaseBegin_Total;

                Console.WriteLine("NEWWW: overAmount_goodCase: " + overAmount_goodCase);
                Console.WriteLine("NEWWW: overAmount_badCase: " + overAmount_badCase);

                if (overAmount_goodCase >= 0 && overAmount_badCase >= 0)
                {
                    //Console.WriteLine(savingPhaseResult);
                    //Console.WriteLine(stopWorkPhaseResult);
                    //Console.WriteLine($"{Environment.NewLine}");

                    //laterNeedsResult.Print();
                    //rentPhaseResult.Print();
                    //rentPhaseResult_WithNeedsFromStopWorkPhase.Print();


                    //increase the monthly rate until overamount is back to 0 
                    var newRate_Cash = rentPhaseResult_WithNeedsFromStopWorkPhase.rate_Cash;
                    var newRate_Stocks = rentPhaseResult_WithNeedsFromStopWorkPhase.rateStocks_ExcludedTaxes_BadCase;
                    while (overAmount_goodCase > 0)
                    {
                        newRate_Cash *= 1.01m;
                        newRate_Stocks *= 1.01m;
                        Console.WriteLine("NEW: newRate_Cash: " + newRate_Cash);
                        Console.WriteLine("NEW: newRate_Stocks: " + newRate_Stocks);

                        var stopWorkPhaseResult_goodCase_new = StopWorkPhaseCalculator.Calculate(
                        ageStopWorkAssumed,
                        lifeAssumptions.ageRentStart,
                        rentPhaseResult.neededPhaseBegin_Cash,
                        rentPhaseResult.neededPhaseBegin_Stocks,
                        newRate_Cash,
                        newRate_Stocks,
                        lifeAssumptions.rentPhase_InterestRate_Cash,
                        lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                        1,
                        lifeAssumptions.taxFactor_Stocks
                        );

                        overAmount_goodCase = savingPhaseResult.SavingsTotal - stopWorkPhaseResult_goodCase_new.NeededPhaseBegin_Total;
                        Console.WriteLine("NEW: overAmount_goodCase: " + overAmount_goodCase);
                    }




                    return new PhaseIntegratorResult()
                    {
                        ageStopWork = ageStopWorkAssumed,
                        //overAmount_goodCase = savingPhaseResult.SavingsTotal - stopWorkPhaseResult_goodCase.NeededPhaseBegin_Total,
                        //overAmount_badCase = savingPhaseResult.SavingsTotal - stopWorkPhaseResult_badCase.NeededPhaseBegin_Total, //is this really needed? the overamount in bad case is anyway complicated becasue the stocks market crash has to be considered.

                        overAmount_goodCase = overAmount_goodCase,
                        overAmount_badCase = overAmount_badCase,                       

                        savingPhaseResult = savingPhaseResult,
                        stateRentResult = stateRentResult,
                        laterNeedsResult = laterNeedsResult,
                        rentPhaseResult = rentPhaseResult,
                        stopWorkPhaseResult_goodCase = stopWorkPhaseResult_goodCase,
                        stopWorkPhaseResult_badCase = stopWorkPhaseResult_badCase
                    };
                };
            }

            throw new Exception("Could not determine stop work age");
        }

        public static void Simulate(
            LifeAssumptions lifeAssumptions,
            PhaseIntegratorResult phaseIntegratorResult,
            IProtocolWriter protocolWriterGoodCase,
            IProtocolWriter protocolWriterBadCase)
        {
            StopWorkPhaseResult stopWorkPhaseResult_goodCase = phaseIntegratorResult.stopWorkPhaseResult_goodCase;
            StopWorkPhaseResult stopWorkPhaseResult_badCase = phaseIntegratorResult.stopWorkPhaseResult_badCase;
            RentPhaseResult rentPhaseResult = phaseIntegratorResult.rentPhaseResult;

            foreach (var protocolWriter in new[] { protocolWriterGoodCase, protocolWriterBadCase })
            {
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
            }

            Frac taxesStocks = Frac.FromFactor(lifeAssumptions.taxFactor_Stocks);

            SavingPhaseCalculator.RebalanceForStopWorkPhase(
                phaseIntegratorResult.ageStopWork - 1, // <-- todo: what happens here if currentAge==stopWorkAge?
                phaseIntegratorResult.overAmount_goodCase,
                stopWorkPhaseResult_goodCase.neededPhaseBegin_Cash,
                stopWorkPhaseResult_goodCase.neededPhaseBegin_Stocks,
                taxesStocks,
                1,
                protocolWriterGoodCase
                );

            SavingPhaseCalculator.RebalanceForStopWorkPhase(
                phaseIntegratorResult.ageStopWork - 1, // <-- todo: what happens here if currentAge==stopWorkAge?
                phaseIntegratorResult.overAmount_badCase,
                stopWorkPhaseResult_badCase.neededPhaseBegin_Cash,
                stopWorkPhaseResult_badCase.neededPhaseBegin_Stocks,
                taxesStocks,
                lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                protocolWriterBadCase
                );





            StopWorkPhaseCalculator.Simulate(
                stopWorkPhaseResult_goodCase.ageStopWork,
                lifeAssumptions.ageRentStart,
                stopWorkPhaseResult_goodCase.neededPhaseBegin_Cash,
                stopWorkPhaseResult_goodCase.neededPhaseBegin_Stocks,
                stopWorkPhaseResult_goodCase.rate_Cash,
                stopWorkPhaseResult_goodCase.rateStocks_ExcludedTaxes_GoodCase,
                lifeAssumptions.rentPhase_InterestRate_Cash,
                lifeAssumptions.rentPhase_InterestRate_Stocks_GoodCase,
                1,
                lifeAssumptions.taxFactor_Stocks,
                protocolWriterGoodCase
            );

            StopWorkPhaseCalculator.Simulate(
                stopWorkPhaseResult_badCase.ageStopWork,
                lifeAssumptions.ageRentStart,
                stopWorkPhaseResult_badCase.neededPhaseBegin_Cash,
                stopWorkPhaseResult_badCase.neededPhaseBegin_Stocks,
                stopWorkPhaseResult_badCase.rate_Cash,
                stopWorkPhaseResult_badCase.rateStocks_ExcludedTaxes_GoodCase,
                lifeAssumptions.rentPhase_InterestRate_Cash,
                lifeAssumptions.rentPhase_InterestRate_Stocks_BadCase,
                lifeAssumptions.rentPhase_CrashFactor_Stocks_BadCase,
                lifeAssumptions.taxFactor_Stocks,
                protocolWriterBadCase
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
                1,
                rentPhaseResult.taxesPerYear_GoodCase,
                protocolWriterGoodCase
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
                protocolWriterBadCase
            );
        }
    }
}
