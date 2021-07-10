using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing
{
    public class Snapshot
    {
        public Input input;

        public Snapshot(Input _input)
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

        public Snapshot ApplyStockInvests()
        {
            this.stocksInvests = input.stocksMonthlyInvestAmount * 12;
            this.stocksYearEnd += this.stocksInvests;
            return this;
        }

        public Snapshot ApplyStocksGrowth()
        {
            this.stocksGrowth = stocksYearEnd * ((double)input.stocksGrowthRate / 100f);
            this.stocksYearEnd += this.stocksGrowth;
            return this;
        }

        public double InterestPerYearToInterestPerMonth(double interestPerYear)
        {
            //https://www.haushaltsfinanzen.de/finanzmathematik/konformer_zinssatz.php?Konformer-Zinssatz-mit-online-Rechner
            double interestPerMonth = Math.Pow(1f + (interestPerYear / 100f), 1f / 12f) - 1;

            return interestPerMonth * 100;
        }
    }
}
