using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processing;
using Processing.Assets;
using Processing.Withdrawal;
using System;
using System.Collections.Generic;

namespace Processing_uTest
{
    [TestClass]
    public class UniformWithdrawalStrategyTests
    {
        [TestMethod]
        public void SimulateTaxesAtWithdrawal_ValidAssets_TaxesCalculatedCorrectly()
        {
            var withdrawalAmount = 1000;

            var input = new Input
            {
                stocks = 10000,
                cash = 20000,
                metals = 50000
            };
            var stocksFraction = (double) input.stocks / (input.stocks + input.cash + input.metals);

            var assets = new List<Asset>() {
                new Cash(input),
                new Stocks(input),
                new Metals(input)
            };


//            var uws = new UniformWithdrawalStrategy();
            Portfolio p = new Portfolio(input);

            var taxes = p.WithdrawalStrategy.SimulateTaxesAtWithdrawal(withdrawalAmount);
            Assert.AreEqual(withdrawalAmount * stocksFraction * 0.26d, taxes, 0.01);
        }
    }
}
