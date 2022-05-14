using Portfolio;
using System.Collections.ObjectModel;

namespace Protocol
{
    public interface IProtocolWriter
    {
        public ReadOnlyCollection<ResultRow> Protocol { get; }
        void Log(Age age, TransactionDetails transactionDetails);
    }
}