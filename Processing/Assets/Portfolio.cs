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
        public Input Input
        {
            get;
            private set;
        }

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
            Input = input;

            Cash = new Cash(Input, this);
            Stocks = new Stocks(Input, this);
            Metals = new Metals(Input, this);
            Total = new Total(Input, new List<Asset> { Cash, Stocks, Metals }, this);

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
        public double GetAssetFraction(int age, Type assetType)
        {
            //SCheisse: das hier darf nur aufgerufen werden wenn ALLE asset klassen schon prozessiert wurden sonst gibts ungleichgewichte!

            int index = age - Input.ageCurrent;

            double total = Cash.Protocol[index].yearEnd
                + Stocks.Protocol[index].yearEnd
                + Metals.Protocol[index].yearEnd;

            double result;
            if (assetType == typeof(Cash))
                result = Cash.Protocol[index].yearEnd / total;
            else if (assetType == typeof(Stocks))
                result = Stocks.Protocol[index].yearEnd / total;
            else if (assetType == typeof(Metals))
                result = Metals.Protocol[index].yearEnd / total;
            else
                throw new Exception($"Unknown asset type <{assetType}>.");

            return result;
        }

        /// <summary>
        /// Gets the average growth rate over all assets.
        /// </summary>
        /// <returns>The average growth rate over all assets.</returns>
        public double GetAverageGrowthRate(int age)
        {
            var assetFractionCash = GetAssetFraction(age, typeof(Cash)) * Input.cashGrowthRate;
            var assetFractionStocks = GetAssetFraction(age, typeof(Stocks)) * Input.stocksGrowthRate;
            var assetFractionMetals = GetAssetFraction(age, typeof(Metals)) * Input.metalsGrowthRate;

            var result = assetFractionCash + assetFractionStocks + assetFractionMetals;

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
