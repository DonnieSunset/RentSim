using Finance.Results;
using Protocol;

namespace Finance
{
    public class SavingPhaseCalculator
    {
        public static SavingPhaseResult CalculateResult(
            int ageCurrent,
            int ageStopWork,
            decimal cashStartCapital,
            int cashGrowthRate,
            decimal cashSaveAmountPerMonth,
            decimal stocksStartCapital,
            int stocksGrowthRate,
            decimal stocksSaveAmountPerMonth,
            decimal metalsStartCapital,
            int metalsGrowthRate,
            decimal metalsSaveAmountPerMonth
            )
        {
            var result = new SavingPhaseResult();

            int duration = ageStopWork - ageCurrent;

            double cashInterestFactor = 1 + (cashGrowthRate / 100d);
            result.savingsCash = FinanceCalculator.SparkassenFormel(cashStartCapital, cashSaveAmountPerMonth * 12, cashInterestFactor, duration);

            double stocksInterestFactor = 1 + (stocksGrowthRate / 100d);
            result.savingsStocks = FinanceCalculator.SparkassenFormel(stocksStartCapital, stocksSaveAmountPerMonth * 12, stocksInterestFactor, duration);

            double metalsInterestFactor = 1 + (metalsGrowthRate / 100d);
            result.savingsMetals = FinanceCalculator.SparkassenFormel(metalsStartCapital, metalsSaveAmountPerMonth * 12, metalsInterestFactor, duration);

            return result;
        }

        public static void Simulate(
            int savingPhaseStartAge,
            int savingPhaseEndAge,
            decimal cashStartCapital,
            int cashGrowthRate,
            decimal cashSaveAmountPerMonth,
            decimal stocksStartCapital,
            int stocksGrowthRate,
            decimal stocksSaveAmountPerMonth,
            decimal metalsStartCapital,
            int metalsGrowthRate,
            decimal metalsSaveAmountPerMonth,
            IProtocolWriter protocolWriter)
        {
            decimal cashCurrent = cashStartCapital;
            decimal stocksCurrent = stocksStartCapital;
            decimal metalsCurrent = metalsStartCapital;

            for (int i = savingPhaseStartAge; i < savingPhaseEndAge; i++)
            {
                protocolWriter.LogBalanceYearBegin(i, cashCurrent, stocksCurrent, metalsCurrent);

                decimal interestsCash = cashCurrent * cashGrowthRate / 100m;
                decimal interestsStocks = stocksCurrent * stocksGrowthRate / 100m;
                decimal interestsMetals = metalsCurrent * metalsGrowthRate / 100m;

                cashCurrent += interestsCash;
                stocksCurrent += interestsStocks;
                metalsCurrent += interestsMetals;
                protocolWriter.Log(i, new TransactionDetails() { cashInterests = interestsCash, stockInterests = interestsStocks, metalInterests = interestsMetals });

                decimal savingsCash = cashSaveAmountPerMonth * 12;
                decimal savingsStocks = stocksSaveAmountPerMonth * 12;
                decimal savingsMetals = metalsSaveAmountPerMonth * 12;

                cashCurrent += savingsCash;
                stocksCurrent += savingsStocks;
                metalsCurrent += savingsMetals;
                protocolWriter.Log(i, new TransactionDetails() { cashDeposit = savingsCash, stockDeposit = savingsStocks, metalDeposit = savingsMetals });
            }
        }

        public static void RebalanceForStopWorkPhase(
            int savingPhaseEndAge,
            decimal overPlusAmount,
            decimal cashTotalStopWorkPhase,
            decimal stocksTotalStopWorkPhase,
            Frac stocksTaxes,
            decimal crashFactor_Stocks_BadCase,
            IProtocolWriter protocolWriter)
        {
            var savingPhaseEndRow = protocolWriter.Protocol.Single(x => x.age == savingPhaseEndAge);


            // Step 1: sell all metals
            var totalMetals = savingPhaseEndRow.metalsYearEnd;
            protocolWriter.Log(savingPhaseEndAge, new TransactionDetails() { metalDeposit = -totalMetals, cashDeposit = totalMetals });



            // Step 2: withdraw overplus amount.
            // Strategy: from end of saving phase, search which asset is bigger than the asset at stop work phase.
            // from those we have to sell anyway in the subsequent rebalancing.
            var restAmountToBeSold = overPlusAmount;
            var diffStocks = savingPhaseEndRow.stocksYearEnd - stocksTotalStopWorkPhase;
            var diffCash = savingPhaseEndRow.cashYearEnd - cashTotalStopWorkPhase;

            if (restAmountToBeSold > 0 && diffCash > 0)
            {
                var cashAmountToSell = Math.Min(diffCash, restAmountToBeSold);
                restAmountToBeSold -= cashAmountToSell;

                protocolWriter.Log(savingPhaseEndAge, new TransactionDetails() { cashDeposit = -cashAmountToSell });
            }

            if (restAmountToBeSold > 0 && diffStocks > 0)
            {
                var stocksAmountToSell = Math.Min(diffStocks, restAmountToBeSold);
                restAmountToBeSold -= stocksAmountToSell;

                //consider taxes
                (var withdrawal, var taxes) = stocksTaxes.FromTotal(stocksAmountToSell);

                protocolWriter.Log(savingPhaseEndAge, new TransactionDetails() { stockDeposit = -withdrawal, stockTaxes = -taxes });
            }

            if (Decimal.Round(restAmountToBeSold, 3) != 0)
            {
                throw new Exception($"Could not sell complete nameof {overPlusAmount}. Rest is {restAmountToBeSold}.");
            }


            // step 3: re-balance stocks and cash to match stopWorkPhase
            var stopWorkPhaseTotal = cashTotalStopWorkPhase + stocksTotalStopWorkPhase;
            var savingPhaseEnd_Total = savingPhaseEndRow.cashYearEnd + savingPhaseEndRow.stocksYearEnd;
            if (Decimal.Round(stopWorkPhaseTotal, 3) != Decimal.Round(savingPhaseEnd_Total, 3))
            {
                throw new Exception($"{nameof(stopWorkPhaseTotal)} ({stopWorkPhaseTotal}) is not equal to {nameof(savingPhaseEnd_Total)} ({savingPhaseEnd_Total}).");
            }

            var cashDiff = savingPhaseEndRow.cashYearEnd - cashTotalStopWorkPhase;
            if (cashDiff >= 0) // i have more cash than i need -> buy stocks
            {
                protocolWriter.Log(savingPhaseEndAge, new TransactionDetails() { cashDeposit = -cashDiff, stockDeposit = cashDiff });
            }
            else // i need more cash than i have -> sell stocks
            {
                //consider taxes
                (var stocksWithdrawal, var taxes) = stocksTaxes.FromTotal(-cashDiff);

                protocolWriter.Log(savingPhaseEndAge, new TransactionDetails() { cashDeposit = -cashDiff, stockDeposit = -stocksWithdrawal, stockTaxes = -taxes });
            }


            //step 4: stock market crash
            var totalStocks = savingPhaseEndRow.stocksYearEnd;
            if (crashFactor_Stocks_BadCase != 1)
            {
                var stocksCrashAmount = totalStocks * crashFactor_Stocks_BadCase;
                totalStocks -= stocksCrashAmount;
                protocolWriter.Log(savingPhaseEndAge, new TransactionDetails() { stockInterests = -stocksCrashAmount });
            }
        }
    }
}
