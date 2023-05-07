using Domain;
using System.Collections.ObjectModel;

namespace Protocol
{
    public class MemoryProtocolWriter : IProtocolWriter
    {
        //private readonly List<ResultRow> myProtocol = new();

        //public ReadOnlyCollection<ResultRow> Protocol
        //{ 
        //    get { return myProtocol.AsReadOnly(); }
        //}

        //public MemoryProtocolWriter()
        //{ 
        //    Protocol = new List<ResultRow> ();
        //}

        public List<ResultRow> Protocol { get; set; } = new List<ResultRow>();

        public void LogBalanceYearBegin(int age, decimal amountCash, decimal amountStocks, decimal amountMetals)
        {
            var affectedResultRow = GetOrCreateRow(age);

            affectedResultRow.Cash.YearBegin = amountCash;
            affectedResultRow.Stocks.YearBegin = amountStocks;
            affectedResultRow.Metals.YearBegin = amountMetals;
        }

        public void Log(int age, TransactionDetails transactionDetails)
        {
            var affectedResultRow = GetOrCreateRow(age);

            affectedResultRow.Cash.Deposits += transactionDetails.cashDeposit;
            affectedResultRow.Stocks.Deposits += transactionDetails.stockDeposit;
            affectedResultRow.Metals.Deposits += transactionDetails.metalDeposit;

            affectedResultRow.Cash.Taxes += transactionDetails.cashTaxes;
            affectedResultRow.Stocks.Taxes += transactionDetails.stockTaxes;
            affectedResultRow.Metals.Taxes += transactionDetails.metalTaxes;

            affectedResultRow.Cash.Interests += transactionDetails.cashInterests;
            affectedResultRow.Stocks.Interests += transactionDetails.stockInterests;
            affectedResultRow.Metals.Interests += transactionDetails.metalInterests;
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
            var row = Protocol.SingleOrDefault(x => x.Age == age);

            if (row == null)
            {
                row = new ResultRow()
                {
                    Age = age,
                };

                var prevRow = Protocol.SingleOrDefault(x => x.Age == age - 1);
                if (prevRow != null)
                { 
                    row.Cash.YearBegin = prevRow.Cash.YearEnd;
                    row.Stocks.YearBegin = prevRow.Stocks.YearEnd;
                    row.Metals.YearBegin = prevRow.Metals.YearEnd;
                }

                Protocol.Add(row);
                Protocol.Sort();
            }

            return row;
        }
    }
}
