using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing.Assets
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
}
