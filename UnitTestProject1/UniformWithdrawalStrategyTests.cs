using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processing;
using Processing.Assets;

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

            Portfolio p = new Portfolio(input);
            var taxes = p.WithdrawalStrategy.SimulateTaxesAtWithdrawal(withdrawalAmount);

            Assert.AreEqual(withdrawalAmount * stocksFraction * 0.26d, taxes, 0.01);
        }
    }
}
