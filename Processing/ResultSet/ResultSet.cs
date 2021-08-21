using Processing.Assets;
using System;
using System.Collections.Generic;

namespace Processing
{
    public class ResultSet
    {
        private Input input;

        private Cash cash;
        private Stocks stocks;
        private Metals metals;
        private Total total;

        public List<ResultRow> resultSet;

        public ResultSet(Input input, Cash cash, Stocks stocks, Metals metals, Total total)
        {
            this.input = input;
            this.cash = cash;
            this.stocks = stocks;
            this.metals = metals;
            this.total = total;
        }

        public List<ResultRow> ProcessAssets()
        {
            resultSet = new List<ResultRow>();

            for (int i = 0; i < input.ageStopWork - input.ageCurrent; i++)
            {
                ResultRow row = new ResultRow();

                row.age = input.ageCurrent + i;
                row.cash = cash.protocol[i];
                row.stocks = stocks.protocol[i];
                row.metals = metals.protocol[i];
                row.total = total.protocol[i];

                if (row.age != row.cash.age
                    || row.age != row.stocks.age
                    || row.age != row.metals.age
                    || row.age != row.total.age)
                {
                    throw new Exception($"Age {row.age} not consisten in protocol entries.");
                }

                resultSet.Add(row);
            }

            return resultSet;
        }
    }
}
