using Processing.Withdrawal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing.Assets
{
    public class Portfolio
    {
        private Input myInput;

        public IWithdrawalStrategy WithdrawalStrategy
        {
            get;
            private set;
        }

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

        public Total Total
        {
            get;
            private set;
        }

        public Portfolio(Input input)
        {
            myInput = input;

            Cash = new Cash(myInput);
            Stocks = new Stocks(myInput);
            Metals = new Metals(myInput);
            Total = new Total(myInput, new List<Asset> { Cash, Stocks, Metals });

            WithdrawalStrategy = new UniformWithdrawalStrategy(this);
        }

        public List<Asset> GetAssets()
        {
            return new List<Asset>() { Cash, Stocks, Metals };
        }

        public void Process()
        {
            Cash.Process();
            Stocks.Process();
            Metals.Process();
            Total.Process();
        }

        public void Process2()
        {
            Cash.Process2();
            Stocks.Process2();
            Metals.Process2();
            Total.Process2();
        }
    }
}
