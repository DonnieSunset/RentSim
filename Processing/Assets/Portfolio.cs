using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing.Assets
{
    public class Portfolio
    {
        public Cash Cash
        {
            get;
            private set;
        }

        public Stocks Stocks
        {
            get;
            private set;
        }

        public Metals Metals
        {
            get;
            private set;
        }

        public Portfolio(Cash cash, Stocks stocks, Metals metals)
        {
            Cash = cash;
            Stocks = stocks;
            Metals = metals;
        }



    }
}
