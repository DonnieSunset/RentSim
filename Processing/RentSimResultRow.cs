using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing
{
    public class RentSimResultRow
    {
        public Input input;

        public RentSimResultRow(Input _input)
        {
            input = _input;

            stocksGrowthMonthly = stocksGrowth / 12;
        }

        public int age;
        public double stocksYearBegin;
        public double stocksInvests;
        public double stocksGrowth;
        public double stocksYearEnd;

        public double stocksGrowthMonthly;

        public RentSimResultRow ApplyStockInvests(double invest)
        {
            this.stocksInvests = invest;
            this.stocksYearEnd += invest;
            return this;
        }

        public RentSimResultRow ApplyStocksGrowth(double growthRate)
        {
            this.stocksGrowth = stocksYearEnd * (growthRate / 100f);
            this.stocksYearEnd += this.stocksGrowth;
            return this;
        }


    }
}
