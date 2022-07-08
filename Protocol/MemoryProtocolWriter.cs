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
            affectedResultRow.cashYearBegin = amountCash;
            affectedResultRow.stocksYearBegin = amountStocks;
            affectedResultRow.metalsYearBegin = amountMetals;
        }

        public void Log(int age, TransactionDetails transactionDetails)
        {
            var affectedResultRow = GetOrCreateRow(age);

            if (transactionDetails.cashDeposit != 0) 
                affectedResultRow.cashDeposits.Add(transactionDetails.cashDeposit);
            if (transactionDetails.stockDeposit != 0)
                affectedResultRow.stocksDeposits.Add(transactionDetails.stockDeposit);
            if (transactionDetails.metalDeposit != 0)
                affectedResultRow.metalsDeposits.Add(transactionDetails.metalDeposit);

            if (transactionDetails.cashWithdrawal != 0)
                affectedResultRow.cashWithdrawals.Add(transactionDetails.cashWithdrawal);
            if (transactionDetails.stockWithdrawal != 0)
                affectedResultRow.stocksWithdrawals.Add(transactionDetails.stockWithdrawal);
            if (transactionDetails.metalWithdrawal != 0)
                affectedResultRow.metalsWithdrawals.Add(transactionDetails.metalWithdrawal);

            affectedResultRow.cashTaxes += transactionDetails.cashTaxes;
            affectedResultRow.stocksTaxes += transactionDetails.stockTaxes;
            affectedResultRow.metalsTaxes += transactionDetails.metalTaxes;

            affectedResultRow.cashInterests += transactionDetails.cashInterests;
            affectedResultRow.stocksInterests += transactionDetails.stockInterests;
            affectedResultRow.metalsInterests += transactionDetails.metalInterests;

            //calculate year end
            affectedResultRow.cashYearEnd = affectedResultRow.cashYearBegin + affectedResultRow.cashDeposits.Sum() + affectedResultRow.cashInterests - affectedResultRow.cashTaxes - affectedResultRow.cashWithdrawals.Sum();
            affectedResultRow.stocksYearEnd = affectedResultRow.stocksYearBegin + affectedResultRow.stocksDeposits.Sum() + affectedResultRow.stocksInterests - affectedResultRow.stocksTaxes - affectedResultRow.stocksWithdrawals.Sum();
            affectedResultRow.metalsYearEnd = affectedResultRow.metalsYearBegin + affectedResultRow.metalsDeposits.Sum() + affectedResultRow.metalsInterests - affectedResultRow.metalsTaxes - affectedResultRow.metalsWithdrawals.Sum();

            //calculate year begin of next row
            //var affectedResultRowNext = GetOrCreateRow(age + 1);
            //affectedResultRowNext.cashYearBegin = affectedResultRow.cashYearEnd;
            //affectedResultRowNext.stocksYearBegin = affectedResultRow.stocksYearEnd;
        }

        private ResultRow GetOrCreateRow(int age)
        {
            var row = myProtocol.SingleOrDefault(x => x.age == age);

            if (row == null)
            {
                row = new ResultRow()
                {
                    age = age,
                    //ageAbsolute = Age.NewByIndexAge(index).Absolut,
                };
                myProtocol.Add(row);
                myProtocol.Sort();
            }

            return row;
        }
    }
}
