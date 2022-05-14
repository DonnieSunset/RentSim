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

        public void Log(Age age, TransactionDetails transactionDetails)
        {
            var affectedResultRow = myProtocol.SingleOrDefault(x => x.ageByIndex == age.Index);

            if (affectedResultRow == null)
            {
                affectedResultRow = new ResultRow() 
                { 
                    ageByIndex = age.Index 
                };
                myProtocol.Add(affectedResultRow);
            }

            affectedResultRow.taxes += transactionDetails.cashTaxes + transactionDetails.stockTaxes + transactionDetails.metalTaxes;
            affectedResultRow.interests += transactionDetails.cashInterests + transactionDetails.stockInterests + transactionDetails.metalInterests;
            affectedResultRow.deposits += transactionDetails.cashDeposits + transactionDetails.stockDeposits + transactionDetails.metalDeposits;

            //todo: calculate year end

            //todo: calculate year begin
        }
    }
}
