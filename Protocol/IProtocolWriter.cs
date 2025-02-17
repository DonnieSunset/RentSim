﻿using Domain;
using System.Collections.ObjectModel;

namespace Protocol
{
    public interface IProtocolWriter
    {
        //public ReadOnlyCollection<ResultRow> Protocol { get; }
        public List<ResultRow> Protocol { get; set; }
        void Log(int age, TransactionDetails transactionDetails);
        void LogBalanceYearBegin(int age, decimal amountCash, decimal amountStocks, decimal amountMetals);
    }
}