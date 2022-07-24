using Finance.Results;
using Protocol;

namespace Finance
{
    public class StopWorkPhaseCalculator
    {
        public static StopWorkPhaseResult Calculate(
            int ageStopWork,
            int ageRentStart,
            decimal amountCashRentStartAge,
            decimal amountStocksRentStartAge,
            decimal rate_Cash_perYear,
            decimal rate_Stocks_ExcludedTaxes_perYear,
            decimal interestRate_Cash,
            decimal interestRate_Stocks,
            decimal crashFactor_Stocks_BadCase,
            decimal stocks_taxFactor
            )
        {
            //calculation by reverse-simulation
            decimal totalCash = amountCashRentStartAge;
            decimal totalStocks = amountStocksRentStartAge;
            decimal taxesPerYear = rate_Stocks_ExcludedTaxes_perYear * (stocks_taxFactor - 1);

            ////stock market crash
            //if (crashFactor_Stocks_BadCase != 1)
            //{
            //    var stocksCrashAmount = totalStocks * crashFactor_Stocks_BadCase;
            //    totalStocks -= stocksCrashAmount;
            //}

            // For reverse calculation we also need the reverse interest factor, otherwise the 
            // interest calculation is based on the wrong amount. Example with 7% interests:
            // Forward: 100.000 * 1.07 = 107.000
            // Reverse: 107.000 * x = 100.000
            //                    x = 100.000 / 107.000 = 0.9345
            // this is the same as 1 / 1.07
            // I couldnt figure out to calculate this only with interest FACTOR, not with interest RATE.
            decimal interestFactor_Cash_Reverse = 1 / (1 + interestRate_Cash);
            decimal interestFactor_Stocks_GoodCase_Reverse = 1 / (1 + interestRate_Stocks);

            for (int age = ageRentStart - 1; age >= ageStopWork; age--)
            {
                // pay taxes - reverse
                totalStocks += taxesPerYear;

                // withdraw rate - reverse
                totalCash += rate_Cash_perYear;
                totalStocks += rate_Stocks_ExcludedTaxes_perYear;

                // get interests - reverse
                totalCash = totalCash * interestFactor_Cash_Reverse;
                totalStocks = totalStocks * interestFactor_Stocks_GoodCase_Reverse;
            }

            //stock market crash
            totalStocks *= crashFactor_Stocks_BadCase;

            return new StopWorkPhaseResult()
            {
                ageStopWork = ageStopWork,

                neededPhaseBegin_Cash = totalCash,
                neededPhaseBegin_Stocks = totalStocks,

                rate_Cash = rate_Cash_perYear,
                rateStocks_ExcludedTaxes_GoodCase = rate_Stocks_ExcludedTaxes_perYear,
                taxesPerYear_GoodCase = taxesPerYear,
                rateStocks_IncludedTaxes_GoodCase = rate_Stocks_ExcludedTaxes_perYear + taxesPerYear
            };
        }

        /// <param name="overplusAmount">
        /// The result of the integration of all phases. 
        /// Since the granularity of the phases are years, 
        /// there is typically an overplus from the savings
        /// which is not needed, in order to sum up
        /// all calculations at the end to zero.
        /// </param>
        public static void Simulate(
            int ageStopWork,
            int ageRentStart,
            decimal amountCashStopWorkAge,
            decimal amountStocksStopWorkAge,
            decimal rate_Cash_perYear,
            decimal rate_Stocks_ExcludedTaxes_perYear,
            decimal interestRate_Cash,
            decimal interestRate_Stocks,
            decimal crashFactor_Stocks_BadCase,
            decimal stocks_taxFactor,
            IProtocolWriter protocolWriter)
        {
            decimal totalCash = amountCashStopWorkAge;
            decimal totalStocks = amountStocksStopWorkAge;
            decimal taxesPerYear = rate_Stocks_ExcludedTaxes_perYear * (stocks_taxFactor - 1);

            ////stock market crash
            //if (crashFactor_Stocks_BadCase != 1)
            //{
            //    var stocksCrashAmount = totalStocks * crashFactor_Stocks_BadCase;
            //    totalStocks -= stocksCrashAmount;
            //    protocolWriter.Log(ageStopWork, new TransactionDetails() { stockInterests = -stocksCrashAmount });
            //}

            for (int age = ageStopWork; age < ageRentStart; age++)
            {
                protocolWriter.LogBalanceYearBegin(age, totalCash, totalStocks, 0);

                // get interests
                var interests_Cash = totalCash * interestRate_Cash;
                var interests_Stocks = totalStocks * interestRate_Stocks;
                totalCash += interests_Cash;
                totalStocks += interests_Stocks;
                protocolWriter.Log(age, new TransactionDetails() { cashInterests = interests_Cash, stockInterests = interests_Stocks });

                // withdraw rate
                totalCash += -rate_Cash_perYear;
                totalStocks += -rate_Stocks_ExcludedTaxes_perYear;
                protocolWriter.Log(age, new TransactionDetails() { cashDeposit = -rate_Cash_perYear, stockDeposit = -rate_Stocks_ExcludedTaxes_perYear });

                // pay taxes
                totalStocks += -taxesPerYear;
                protocolWriter.Log(age, new TransactionDetails() { stockTaxes = -taxesPerYear });
            }
        }
    }
}
