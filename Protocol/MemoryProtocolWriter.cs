using Portfolio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol
{
    public class MemoryProtocolWriter : IProtocolWriter
    {
        private readonly List<ResultRow> myProtocol = new();

        public ReadOnlyCollection<ResultRow> Protocol
        { 
            get { return myProtocol.AsReadOnly(); }
        }

        public void LogBalanceYearBegin(Age age, decimal amount)
        {
            var affectedResultRow = GetOrCreateRow(age.Index);
            affectedResultRow.balanceYearBegin = amount;
        }

        public void Log(Age age, TransactionDetails transactionDetails)
        {
            var affectedResultRow = GetOrCreateRow(age.Index);

            affectedResultRow.deposits += transactionDetails.cashDeposits + transactionDetails.stockDeposits + transactionDetails.metalDeposits;
            affectedResultRow.taxes += transactionDetails.cashTaxes + transactionDetails.stockTaxes + transactionDetails.metalTaxes;
            affectedResultRow.interests += transactionDetails.cashInterests + transactionDetails.stockInterests + transactionDetails.metalInterests;

            //calculate year end
            affectedResultRow.balanceYearEnd = affectedResultRow.balanceYearBegin + affectedResultRow.deposits + affectedResultRow.taxes + affectedResultRow.interests;

            //calculate year begin of next row
            var affectedResultRowNext = GetOrCreateRow(age.Index + 1);
            affectedResultRowNext.balanceYearBegin = affectedResultRow.balanceYearEnd;
        }

        private ResultRow GetOrCreateRow(int index)
        {
            var row = myProtocol.SingleOrDefault(x => x.ageByIndex == index);

            if (row == null)
            {
                row = new ResultRow()
                {
                    ageByIndex = index,
                    ageAbsolute = Age.NewByIndexAge(index).Absolut,
                };
                myProtocol.Add(row);
            }

            return row;
        }
    }
}
