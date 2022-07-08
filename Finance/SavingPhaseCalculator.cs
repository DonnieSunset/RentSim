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
            decimal stockTotalStopWorkPhase,
            IProtocolWriter protocolWriter)
        {
            var savingPhaseEndRow = protocolWriter.Protocol.Single(x => x.age == savingPhaseEndAge);


            // Step 1: withdraw overplus amount
            if (savingPhaseEndRow.cashYearEnd < overPlusAmount)
            {
                throw new Exception($"Insufficient cash ({savingPhaseEndRow.cashYearEnd}) in order to withdraw overplus amount ({overPlusAmount})." +
                    $" A more intelligent implementation could help here which considers also the stocks savings.");
            }
            protocolWriter.Log(savingPhaseEndAge, new TransactionDetails() { cashWithdrawal = overPlusAmount });

            // Step 2: sell all metals
            var totalMetals = savingPhaseEndRow.metalsYearEnd;
            protocolWriter.Log(savingPhaseEndAge, new TransactionDetails() { metalWithdrawal = totalMetals, cashDeposit = totalMetals });

            // step 3: re-balance stocks and cash to match stopWorkPhase
            var stopWorkPhaseTotal = cashTotalStopWorkPhase + stockTotalStopWorkPhase;
            var savingPhaseEnd_Total = savingPhaseEndRow.cashYearEnd + savingPhaseEndRow.stocksYearEnd;
            if (Decimal.Round(stopWorkPhaseTotal, 3) != Decimal.Round(savingPhaseEnd_Total, 3))
            {
                throw new Exception($"{nameof(stopWorkPhaseTotal)} ({stopWorkPhaseTotal}) is not equal to {nameof(savingPhaseEnd_Total)} ({savingPhaseEnd_Total}).");
            }

            var cashDiff = savingPhaseEndRow.cashYearEnd - cashTotalStopWorkPhase;
            if (cashDiff >= 0) // i have more than i need
            {
                protocolWriter.Log(savingPhaseEndAge, new TransactionDetails() { cashWithdrawal = cashDiff });
            }
            else // i need more than i have 
            {
                protocolWriter.Log(savingPhaseEndAge, new TransactionDetails() { cashDeposit = -cashDiff });
            }

            var stocksDiff = savingPhaseEndRow.stocksYearEnd - stockTotalStopWorkPhase;
            if (stocksDiff >= 0) // i have more than i need
            {
                protocolWriter.Log(savingPhaseEndAge, new TransactionDetails() { stockWithdrawal = stocksDiff });
            }
            else // i need more than i have 
            {
                protocolWriter.Log(savingPhaseEndAge, new TransactionDetails() { stockDeposit = -stocksDiff });
            }
        }
    }
}
