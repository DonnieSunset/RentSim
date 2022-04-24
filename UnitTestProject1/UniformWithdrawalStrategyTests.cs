using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processing;
using Processing.Assets;

namespace Processing_uTest
{
    [TestClass]
    public class UniformWithdrawalStrategyTests
    {
        //[TestMethod]
        //public void SimulateTaxesAtWithdrawal_ValidAssets_TaxesCalculatedCorrectly()
        //{
        //    var withdrawalAmount = 1000;

        //    var input = new Input
        //    {
        //        stocks = 10000,
        //        cash = 20000,
        //        metals = 50000,

        //        ageCurrent = 41,
        //        ageStopWork = 60,
        //        ageRentStart = 68,
        //        ageEnd = 80
        //    };
        //    var stocksFraction = (double) input.stocks / (input.stocks + input.cash + input.metals);

        //    Portfolio p = new Portfolio(input);
        //    p.Process();
        //    var taxes = p.WithdrawalStrategy.SimulateTaxesAtWithdrawal(input.ageStopWork, withdrawalAmount);

        //    Assert.AreEqual(withdrawalAmount * stocksFraction * 0.26d, taxes, 0.01);
        //}

        //[TestMethod]
        //public void GetWithdrawalAmount_UniformWithdrawalAndEqualAssets_EqualWithdrawalRates()
        //{
        //    Input input = new Input();
        //    input.ageCurrent = 40;
        //    input.ageStopWork = 60;
        //    input.ageRentStart = 70;
        //    input.ageEnd = 80;

        //    input.interestRateType = InterestRateType.Relativ;

        //    input.stocks = 100000;
        //    input.stocksGrowthRate = 10;
        //    input.stocksMonthlyInvestAmount = 0;

        //    input.cash = 100000;
        //    input.cashGrowthRate = 10;
        //    input.cashMonthlyInvestAmount = 0;

        //    input.metals = 100000;
        //    input.metalsGrowthRate = 10;
        //    input.metalsMonthlyInvestAmount = 0;

        //    input.netStateRentFromCurrentAge = 762;
        //    input.netStateRentFromRentStartAge = 2015;

        //    var portfolio = new Portfolio(input);
        //    portfolio.Process();
        //}
    }
}
