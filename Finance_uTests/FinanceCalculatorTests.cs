using Domain;
using Finance;
using NUnit.Framework;


namespace Finance_uTests
{
    [TestFixture]
    public class FinanceCalculatorTests
    {
        [Test]
        public void Blubb()
        {
            //theorie das man die steuer dinger berechnet indem man einmal ohne und einemal mit steuern berechnet und dann die differenz die steuer ist
            //hmm doch nicht, weil ´sich hier die cash rate auch mit ändert und die sollte ja gleich sein. 
            // wie könnte man die festzurren?

            var lifeAssumptions = new LifeAssumptions();
            var rentPhase = lifeAssumptions.RentPhase;

            Console.WriteLine($"** NeedsMinimum_PerYear: {rentPhase.NeedsMinimum_PerYear:F2}, NeedsComfort_PerYear: {rentPhase.NeedsComfort_PerYear:F2}");
            BlaResult blaResult = FinanceCalculator.BlaCalculate(
                rentPhase.InterestRate_Stocks_GoodCase,
                rentPhase.InterestRate_Stocks_BadCase,
                rentPhase.InterestRate_Cash,
                rentPhase.DurationInYears,
                rentPhase.NeedsComfort_PerYear,
                rentPhase.NeedsMinimum_PerYear,
                rentPhase.CrashFactor_Stocks_BadCase
                //1.26m
                );

            //warum ist das negativ?
            blaResult.total_Cash = -blaResult.total_Cash;
            blaResult.total_Stocks = -blaResult.total_Stocks;

            //was brauche ich an (Good case scenario)
            decimal neededStocks = rentPhase.NeedsComfort_PerYear - blaResult.total_Stocks;

            decimal rate2 = FinanceCalculator.Sparkassenformel(neededStocks, (rentPhase.InterestRate_Stocks_GoodCase * 1.26m) + 1, rentPhase.DurationInYears);
            Console.WriteLine($"** stocks rate2: {rate2:F2}");

            decimal taxesPerYear = blaResult.rate_Stocks_Max - rate2;

            //jetzt noch ausprobieren obs klappt
            for (int i = 0; i < rentPhase.DurationInYears; i++)
            {
                Console.WriteLine($"State Begin Year {i}: Cash: {blaResult.total_Cash:F2} Stocks: {blaResult.total_Stocks:F2} Total: {blaResult.total_Stocks + blaResult.total_Cash:F2}");

                var interests_Cash = blaResult.total_Cash * rentPhase.InterestRate_Cash;
                var interests_Stocks = blaResult.total_Stocks * rentPhase.InterestRate_Stocks_GoodCase;
                blaResult.total_Cash += interests_Cash;
                blaResult.total_Stocks += interests_Stocks;
                Console.WriteLine($"\tInterests: Cash: {interests_Cash:F2} Stocks: {interests_Stocks:F2} Total: {interests_Cash + interests_Stocks:F2}");

                blaResult.total_Cash -= blaResult.rate_Cash;
                //blaResult.total_Stocks -= blaResult.rate_Stocks_Max;
                blaResult.total_Stocks -= rate2;
                Console.WriteLine($"\tWithdrawal: Cash: {blaResult.rate_Cash:F2} Stocks: {blaResult.rate_Stocks_Max:F2} Total: {blaResult.rate_Cash + blaResult.rate_Stocks_Max:F2}");

                blaResult.total_Stocks -= taxesPerYear;
                Console.WriteLine($"\tWithdrawal: taxes: {taxesPerYear:F2}");

                Console.WriteLine($"\tEnd Year {i}: Cash: {blaResult.total_Cash:F2} Stocks: {blaResult.total_Stocks:F2} Total: {blaResult.total_Stocks + blaResult.total_Cash:F2}");
            }

            Assert.That(blaResult.total_Cash, Is.EqualTo(0).Within(1));
            Assert.That(blaResult.total_Stocks, Is.EqualTo(0).Within(1));

        }

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

            Assert.That(blaResult.rate_Cash + blaResult.rate_Stocks_Max, Is.EqualTo(rentPhase.NeedsComfort_PerYear).Within(1));

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

                //new for taxes
                //var nettoAbhebung = blaResult.rate_Cash;
                //blaResult.total_Stocks -= blaResult.rate_Stocks_Max;

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

            Assert.That(blaResult.rate_Cash + blaResult.rate_Stocks_Min, Is.EqualTo(rentPhase.NeedsMinimum_PerYear).Within(1));

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