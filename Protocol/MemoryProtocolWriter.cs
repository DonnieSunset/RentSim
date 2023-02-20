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

            affectedResultRow.cash.Deposits += transactionDetails.cashDeposit;
            affectedResultRow.stocks.Deposits += transactionDetails.stockDeposit;
            affectedResultRow.metals.Deposits += transactionDetails.metalDeposit;

            affectedResultRow.cash.Taxes += transactionDetails.cashTaxes;
            affectedResultRow.stocks.Taxes += transactionDetails.stockTaxes;
            affectedResultRow.metals.Taxes += transactionDetails.metalTaxes;

            affectedResultRow.cash.Interests += transactionDetails.cashInterests;
            affectedResultRow.stocks.Interests += transactionDetails.stockInterests;
            affectedResultRow.metals.Interests += transactionDetails.metalInterests;
        }

        //public void RecalcYearBeginEntries()
        //{
        //    myProtocol.Sort();

        //    //first entry definitely needs year begin from outside!!
        //    for (int i = 1; i < myProtocol.Count; i++)
        //    {
        //        myProtocol[i].cash.YearBegin = myProtocol[i - 1].cash.YearEnd;
        //        myProtocol[i].stocks.YearBegin = myProtocol[i - 1].stocks.YearEnd;
        //        myProtocol[i].metals.YearBegin = myProtocol[i - 1].metals.YearEnd;
        //    }
        //}

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
