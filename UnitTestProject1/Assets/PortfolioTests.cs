using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processing;
using Processing.Assets;
using System;
using System.Linq;

namespace Processing_uTest.Assets
{
    [TestClass]
    public class PortfolioTests
    {
        [TestMethod]
        public void GetAssetFraction_ValidAssetType_ReturnsCorrectFraction()
        {
            Input i = GetFakeInput();
            Portfolio p = new Portfolio(i);
            p.Process();

            Assert.AreEqual(180000, p.Cash.protocol.Last().yearEnd);
            Assert.AreEqual(190000, p.Stocks.protocol.Last().yearEnd);
            Assert.AreEqual(10000, p.Metals.protocol.Last().yearEnd);

            var total = 180000 + 190000 + 10000; // 380.000

            var cashFraction = p.GetAssetFraction(typeof(Cash));
            var StocksFraction = p.GetAssetFraction(typeof(Stocks));
            var metalsFraction = p.GetAssetFraction(typeof(Metals));

            Assert.AreEqual(180d/380d, cashFraction);
            Assert.AreEqual(190d/380d, StocksFraction);
            Assert.AreEqual(10d/380d, metalsFraction);
        }

        [TestMethod]
        public void GetAssetFraction_ZeroAssetType_ReturnsZero()
        {
            Input i = GetFakeInput();
            i.cash = 0;
            i.cashMonthlyInvestAmount = 0;

            Portfolio p = new Portfolio(i);
            p.Process();

            var cashFraction = p.GetAssetFraction(typeof(Cash));

            Assert.AreEqual(0, cashFraction);
        }

        [TestMethod]
        public void GetAssetFraction_InvalidAssetType_ThrowsException() 
        {
            Input i = GetFakeInput();
            Portfolio p = new Portfolio(i);
            p.Process();

            Assert.ThrowsException<Exception>(() => p.GetAssetFraction(typeof(Input)));
        }

        private Input GetFakeInput()
        {
            return new Input()
            {
                ageCurrent = 40,
                ageStopWork = 50,
                ageRentStart = 67,
                ageEnd = 80,

                cash = 60000,
                cashGrowthRate = 0,
                cashMonthlyInvestAmount = 1000,

                stocks = 70000,
                stocksGrowthRate = 0,
                stocksMonthlyInvestAmount = 1000,

                metals = 10000,
                metalsGrowthRate = 0,
                metalsMonthlyInvestAmount = 0
            };
        }
    }
}
