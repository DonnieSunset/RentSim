using Domain;
using Finance;
using NUnit.Framework;


namespace Finance_uTests
{
    [TestFixture]
    public class FinanceCalculatorTests
    {
        [Test]
        public void Blubb_Comfort()
        {
            var lifeAssumptions = new LifeAssumptions();
            var rentPhase = lifeAssumptions.RentPhase;

            BlaResult blaResult = FinanceCalculator.BlaCalculate(
                rentPhase.InterestRate_Stocks_GoodCase,
                rentPhase.InterestRate_Stocks_BadCase,
                rentPhase.InterestRate_Cash,
                rentPhase.DurationInYears,
                rentPhase.NeedsComfort_PerYear,
                rentPhase.NeedsMinimum_PerYear,
                rentPhase.CrashFactor_Stocks_BadCase
                );

            //warum ist das negativ?
            blaResult.total_Cash = -blaResult.total_Cash;
            blaResult.total_Stocks = -blaResult.total_Stocks;

            for (int i = 0; i < rentPhase.DurationInYears; i++)
            {
                Console.WriteLine($"State Begin Year {i}: Cash: {blaResult.total_Cash:F2} Stocks: {blaResult.total_Stocks:F2} Total: {blaResult.total_Stocks + blaResult.total_Cash:F2}");

                var interests_Cash = blaResult.total_Cash * rentPhase.InterestRate_Cash;
                var interests_Stocks = blaResult.total_Stocks * rentPhase.InterestRate_Stocks_GoodCase;
                blaResult.total_Cash += interests_Cash;
                blaResult.total_Stocks += interests_Stocks;
                Console.WriteLine($"\tInterests: Cash: {interests_Cash:F2} Stocks: {interests_Stocks:F2} Total: {interests_Cash + interests_Stocks:F2}");

                blaResult.total_Cash -= blaResult.rate_Cash;
                blaResult.total_Stocks -= blaResult.rate_Stocks_Max;
                Console.WriteLine($"\tWithdrawal: Cash: {blaResult.rate_Cash:F2} Stocks: {blaResult.rate_Stocks_Max:F2} Total: {blaResult.rate_Cash + blaResult.rate_Stocks_Max:F2}");

                Console.WriteLine($"\tEnd Year {i}: Cash: {blaResult.total_Cash:F2} Stocks: {blaResult.total_Stocks:F2} Total: {blaResult.total_Stocks + blaResult.total_Cash:F2}");
            }

            Assert.That(blaResult.total_Cash, Is.EqualTo(0).Within(1));
            Assert.That(blaResult.total_Stocks, Is.EqualTo(0).Within(1));
        }

        [Test]
        public void Blubb_Crash()
        {
            var lifeAssumptions = new LifeAssumptions();
            var rentPhase = lifeAssumptions.RentPhase;

            BlaResult blaResult = FinanceCalculator.BlaCalculate(
                rentPhase.InterestRate_Stocks_GoodCase,
                rentPhase.InterestRate_Stocks_BadCase,
                rentPhase.InterestRate_Cash,
                rentPhase.DurationInYears,
                rentPhase.NeedsComfort_PerYear,
                rentPhase.NeedsMinimum_PerYear,
                rentPhase.CrashFactor_Stocks_BadCase
                );

            //warum ist das negativ?
            blaResult.total_Cash = -blaResult.total_Cash;
            blaResult.total_Stocks = -blaResult.total_Stocks;

            //stock market crash
            blaResult.total_Stocks *= rentPhase.CrashFactor_Stocks_BadCase;

            for (int i = 0; i < rentPhase.DurationInYears; i++)
            {
                Console.WriteLine($"State Begin Year {i}: Cash: {blaResult.total_Cash:F2} Stocks: {blaResult.total_Stocks:F2} Total: {blaResult.total_Stocks + blaResult.total_Cash:F2}");

                var interests_Cash = blaResult.total_Cash * rentPhase.InterestRate_Cash;
                var interests_Stocks = blaResult.total_Stocks * rentPhase.InterestRate_Stocks_BadCase;
                blaResult.total_Cash += interests_Cash;
                blaResult.total_Stocks += interests_Stocks;
                Console.WriteLine($"\tInterests: Cash: {interests_Cash:F2} Stocks: {interests_Stocks:F2} Total: {interests_Cash + interests_Stocks:F2}");

                blaResult.total_Cash -= blaResult.rate_Cash;
                blaResult.total_Stocks -= blaResult.rate_Stocks_Min;
                Console.WriteLine($"\tWithdrawal: Cash: {blaResult.rate_Cash:F2} Stocks: {blaResult.rate_Stocks_Min:F2} Total: {blaResult.rate_Cash + blaResult.rate_Stocks_Min:F2}");

                Console.WriteLine($"\tEnd Year {i}: Cash: {blaResult.total_Cash:F2} Stocks: {blaResult.total_Stocks:F2} Total: {blaResult.total_Stocks + blaResult.total_Cash:F2}");
            }

            Assert.That(blaResult.total_Cash, Is.EqualTo(0).Within(1));
            Assert.That(blaResult.total_Stocks, Is.EqualTo(0).Within(1));
        }
    }
}