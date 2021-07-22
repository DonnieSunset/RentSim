using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing
{
    public class Asset
    {
        public double yearBegin;
        public double invests;
        public double growth;
        public double yearEnd;

        public Asset ApplyInvests(double invest)
        {
            this.invests += invest;
            this.yearEnd += invest;
            return this;
        }

        public Asset ApplyGrowth(double growthRate)
        {
            double thisMonthGrowth = yearEnd * (growthRate / 100d);
            this.growth += thisMonthGrowth;
            this.yearEnd += thisMonthGrowth;
            return this;
        }
    }



    public class RentSimResultRow
    {
        //public Input input;

        public int age;

        public Asset cash;
        public Asset stocks;
        public Asset metals;

        public RentSimResultRow(Input _input)
        {
            cash = stocks = metals = new Asset();
            stocks.yearBegin = stocks.yearEnd =_input.stocks;
        }

        // For testability
        internal RentSimResultRow()
        {
            cash = stocks = metals = new Asset();
            stocks.yearEnd = stocks.yearBegin;
            //sad
        }
    }
}
