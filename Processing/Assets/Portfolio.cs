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

            Cash = new Cash(myInput, this);
            Stocks = new Stocks(myInput, this);
            Metals = new Metals(myInput, this);
            Total = new Total(myInput, new List<Asset> { Cash, Stocks, Metals }, this);

            WithdrawalStrategy = new UniformWithdrawalStrategy(this);
        }

        public List<Asset> GetAssets()
        {
            return new List<Asset>() { Cash, Stocks, Metals };
        }

        /// <summary>
        /// Gets the fraction of a given asset type
        /// compared to the sum of all assets.
        /// </summary>
        /// <param name="assetType">The asset type.</param>
        /// <returns>The fraction of the asset.</returns>
        public double GetAssetFraction(Type assetType)
        {
            double total = Cash.protocol.Last().yearEnd
                + Stocks.protocol.Last().yearEnd
                + Metals.protocol.Last().yearEnd;

            if (assetType == typeof(Cash))
                return Cash.protocol.Last().yearEnd / total;
            else if (assetType == typeof(Stocks))
                return Stocks.protocol.Last().yearEnd / total;
            else if (assetType == typeof(Metals))
                return Metals.protocol.Last().yearEnd / total;
            else
                throw new Exception($"Unknown asset type <{assetType}>.");
        }

        /// <summary>
        /// Gets the average growth rate over all assets.
        /// </summary>
        /// <returns>The average growth rate over all assets.</returns>
        public double GetAverageGrowthRate()
        {
            double result = GetAssetFraction(typeof(Cash)) * myInput.cashGrowthRate +
                GetAssetFraction(typeof(Stocks)) * myInput.stocksGrowthRate +
                GetAssetFraction(typeof(Metals)) * myInput.metalsGrowthRate;

            return result;
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
