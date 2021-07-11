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
        }

        // For testability
        internal RentSimResultRow()
        {
        }

        public int age;
        public double stocksYearBegin;
        public double stocksInvests;
        public double stocksGrowth;
        public double stocksYearEnd;

        public RentSimResultRow ApplyStockInvests(double invest)
        {
            this.stocksInvests += invest;
            this.stocksYearEnd += invest;
            return this;
        }

        public RentSimResultRow ApplyStocksGrowth(double growthRate)
        {
            double thisMonthGrowth = stocksYearEnd * (growthRate / 100d);
            this.stocksGrowth += thisMonthGrowth;
            this.stocksYearEnd += thisMonthGrowth;
            return this;
        }
    }
}
