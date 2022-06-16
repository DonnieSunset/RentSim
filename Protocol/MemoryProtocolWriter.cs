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

        public void LogBalanceYearBegin(int age, decimal amountCash, decimal amountStocks)
        {
            var affectedResultRow = GetOrCreateRow(age);
            affectedResultRow.cashYearBegin = amountCash;
            affectedResultRow.stocksYearBegin = amountStocks;
        }

        public void Log(int age, TransactionDetails transactionDetails)
        {
            var affectedResultRow = GetOrCreateRow(age);

            affectedResultRow.cashDeposits += transactionDetails.cashDeposits;
            affectedResultRow.stocksDeposits += transactionDetails.stockDeposits;

            affectedResultRow.stocksTaxes += transactionDetails.stockTaxes;

            affectedResultRow.cashInterests += transactionDetails.cashInterests;
            affectedResultRow.stocksInterests += transactionDetails.stockInterests;

            //calculate year end
            affectedResultRow.cashYearEnd = affectedResultRow.cashYearBegin - affectedResultRow.cashDeposits + affectedResultRow.cashInterests;
            affectedResultRow.stocksYearEnd = affectedResultRow.stocksYearBegin - affectedResultRow.stocksDeposits + affectedResultRow.stocksInterests - affectedResultRow.stocksTaxes;

            //calculate year begin of next row
            var affectedResultRowNext = GetOrCreateRow(age + 1);
            affectedResultRowNext.cashYearBegin = affectedResultRow.cashYearEnd;
            affectedResultRowNext.stocksYearBegin = affectedResultRow.stocksYearEnd;
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
