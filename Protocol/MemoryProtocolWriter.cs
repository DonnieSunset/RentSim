using Domain;
using System.Collections.ObjectModel;

namespace Protocol
{
    public class MemoryProtocolWriter : IProtocolWriter
    {
        private readonly List<ResultRow> myProtocol = new();

        public ReadOnlyCollection<ResultRow> Protocol
        { 
            get { return myProtocol.AsReadOnly(); }
        }

        public void LogBalanceYearBegin(int age, decimal amountCash, decimal amountStocks, decimal amountMetals)
        {
            var affectedResultRow = GetOrCreateRow(age);
            affectedResultRow.cash.YearBegin = amountCash;
            affectedResultRow.stocks.YearBegin = amountStocks;
            affectedResultRow.metals.YearBegin = amountMetals;
        }

        public void Log(int age, TransactionDetails transactionDetails)
        {
            var affectedResultRow = GetOrCreateRow(age);

            //if (transactionDetails.cashDeposit != 0) 
            //    affectedResultRow.cash.Deposits.Add(transactionDetails.cashDeposit);
            //if (transactionDetails.stockDeposit != 0)
            //    affectedResultRow.stocks.Deposits.Add(transactionDetails.stockDeposit);
            //if (transactionDetails.metalDeposit != 0)
            //    affectedResultRow.metals.Deposits.Add(transactionDetails.metalDeposit);

            //if (transactionDetails.cashWithdrawal != 0)
            //    affectedResultRow.cashWithdrawals.Add(transactionDetails.cashWithdrawal);
            //if (transactionDetails.stockWithdrawal != 0)
            //    affectedResultRow.stocksWithdrawals.Add(transactionDetails.stockWithdrawal);
            //if (transactionDetails.metalWithdrawal != 0)
            //    affectedResultRow.metalsWithdrawals.Add(transactionDetails.metalWithdrawal);

            affectedResultRow.cash.Deposits += transactionDetails.cashDeposit;
            affectedResultRow.stocks.Deposits += transactionDetails.stockDeposit;
            affectedResultRow.metals.Deposits += transactionDetails.metalDeposit;

            affectedResultRow.cash.Taxes += transactionDetails.cashTaxes;
            affectedResultRow.stocks.Taxes += transactionDetails.stockTaxes;
            affectedResultRow.metals.Taxes += transactionDetails.metalTaxes;

            affectedResultRow.cash.Interests += transactionDetails.cashInterests;
            affectedResultRow.stocks.Interests += transactionDetails.stockInterests;
            affectedResultRow.metals.Interests += transactionDetails.metalInterests;

            //calculate year end --> this is done now in ResultRow itsself
            //affectedResultRow.cash.YearEnd = affectedResultRow.cash.YearBegin + affectedResultRow.cash.Deposits + affectedResultRow.cash.Interests + affectedResultRow.cash.Taxes;
            //affectedResultRow.stocks.YearEnd = affectedResultRow.stocks.YearBegin + affectedResultRow.stocks.Deposits + affectedResultRow.stocks.Interests + affectedResultRow.stocks.Taxes;
            //affectedResultRow.metals.YearEnd = affectedResultRow.metals.YearBegin + affectedResultRow.metals.Deposits + affectedResultRow.metals.Interests + affectedResultRow.metals.Taxes;

            //calculate year begin of next row
            //var affectedResultRowNext = GetOrCreateRow(age + 1);
            //affectedResultRowNext.cashYearBegin = affectedResultRow.cashYearEnd;
            //affectedResultRowNext.stocksYearBegin = affectedResultRow.stocksYearEnd;
        }

        private ResultRow GetOrCreateRow(int age)
        {
            var row = myProtocol.SingleOrDefault(x => x.Age == age);

            if (row == null)
            {
                row = new ResultRow()
                {
                    Age = age,
                };

                var prevRow = myProtocol.SingleOrDefault(x => x.Age == age - 1);
                if (prevRow != null)
                { 
                    row.cash.YearBegin = prevRow.cash.YearEnd;
                    row.stocks.YearBegin = prevRow.stocks.YearEnd;
                    row.metals.YearBegin = prevRow.metals.YearEnd;
                }

                myProtocol.Add(row);
                myProtocol.Sort();
            }

            return row;
        }
    }
}
