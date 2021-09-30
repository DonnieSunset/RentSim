using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processing;
using Processing.Assets;
using System;

namespace RentSim_iTest
{
    [TestClass]
    public class PortfolioTests
    {
        [TestMethod]
        public void ProcessTest_UniformWithdrawal_CalculatedRentRateEqualsCalculatedWithdrawalRates()
        {
            //todo: make this test with different inputs

            //todo: make this test with complete processing

            Input input = new Input();
            input.ageCurrent = 41;
            input.ageStopWork = 60;
            input.ageRentStart = 67;
            input.ageEnd = 80;

            input.interestRateType = InterestRateType.Relativ;

            input.stocks = 60000;
            input.stocksGrowthRate = 7;
            input.stocksMonthlyInvestAmount = 700;

            input.cash = 60000;
            input.cashGrowthRate = 0;
            input.cashMonthlyInvestAmount = 350;

            input.metals = 20000;
            input.metalsGrowthRate = 1;
            input.metalsMonthlyInvestAmount = 0;

            input.netStateRentFromCurrentAge = 762;
            input.netStateRentFromRentStartAge = 2015;

            var portfolio = new Portfolio(input);
            portfolio.Process();

            int index = input.ageStopWork - input.ageCurrent;
            double TotalSavingStopWorkAge = portfolio.Total.protocol[index].yearBegin;  //resultRows[index].total.yearBegin;
            double approxStopWorkAgeNetRent = RentSimMath.RentStopWorkAgeApproximation(
                input.ageCurrent,
                input.ageStopWork,
                input.ageRentStart,
                input.netStateRentFromCurrentAge,
                input.netStateRentFromRentStartAge);

            double ratePhaseStopWork = portfolio.WithdrawalStrategy.GetWithdrawalAmount(input.ageStopWork);
            double ratePhaseRent = ratePhaseStopWork - (approxStopWorkAgeNetRent * 12);

            portfolio.Process2();

            var resultSet = new ResultSet(input, portfolio.Cash, portfolio.Stocks, portfolio.Metals, portfolio.Total);
            var resultRows = resultSet.ProcessAssets();

            var resultRowIndex = input.ageRentStart - input.ageCurrent - 1;
            var resultRow = resultRows[resultRowIndex];
            Console.WriteLine("SH1: " + resultRow.age);

            var sumOfAllWithdrawals = resultRow.total.invests;
            Console.WriteLine("SH2: " + sumOfAllWithdrawals);

            var sumOfCalculatedRates = ratePhaseStopWork * 12;
            Console.WriteLine("SH3: " + sumOfCalculatedRates);

            Assert.AreEqual(sumOfAllWithdrawals, sumOfCalculatedRates, 0.1);
        }
    }
}
