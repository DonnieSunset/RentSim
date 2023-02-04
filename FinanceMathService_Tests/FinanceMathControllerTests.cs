using FinanceMathService;
using NUnit.Framework;

namespace FinanceMathService_Tests
{
    public class FinanceMath_uTests
    {
        private FinanceMath myFinanceMath = new FinanceMath();

        [TestCase(2500, 1, 2000, 0)]
        [TestCase(2500, 0.9, 2000, 0)]
        [TestCase(2500, 0.8, 2000, 0)]
        [TestCase(2500, 0.7, 2000, 833.33333)]
        [TestCase(2500, 0.6, 2000, 1250)]
        [TestCase(2500, 0.5, 2000, 1500)]
        [TestCase(2500, 0.4, 2000, 1666.66666)]
        [TestCase(2500, 0.3, 2000, 1785.71428)]
        [TestCase(2500, 0.2, 2000, 1875)]
        [TestCase(2500, 0.1, 2000, 1944.44444)]
        [TestCase(2500, 0, 2000, 2000)]
        public void NonRiskAssetsNeededInCaseOfRiskAssetCrash_ValidInput(double totalAmount, double stocksCrashFactor, double totalAmountMinNeededAfterCrash, double expectedLowRiskAmount)
        {
            double lowRiskAMount = myFinanceMath.NonRiskAssetsNeededInCaseOfRiskAssetCrash(totalAmount, stocksCrashFactor, totalAmountMinNeededAfterCrash);
            double highRiskAmount = totalAmount - lowRiskAMount;

            //Console.WriteLine($"{nameof(lowRiskAMount)}: {lowRiskAMount}");
            //Console.WriteLine($"{nameof(highRiskAmount)}: {highRiskAmount}");

            //Console.WriteLine($"{nameof(totalAmount)}: {totalAmount} / {nameof(stocksCrashFactor)}: {stocksCrashFactor} / {nameof(totalAmountMinNeededAfterCrash)}: {totalAmountMinNeededAfterCrash} / {nameof(lowRiskAMount)}: {lowRiskAMount}");

            Assert.That(lowRiskAMount, Is.GreaterThanOrEqualTo(0));
            Assert.That(highRiskAmount, Is.GreaterThanOrEqualTo(0));
            Assert.That(lowRiskAMount + highRiskAmount, Is.EqualTo(totalAmount).Within(0.001));
            Assert.That(lowRiskAMount + (highRiskAmount * stocksCrashFactor), Is.GreaterThanOrEqualTo(totalAmountMinNeededAfterCrash).Within(0.001));

            Assert.That(lowRiskAMount, Is.EqualTo(expectedLowRiskAmount).Within(0.001));
        }

        [TestCase(1000, 0.5, 2000, 2000)]
        [TestCase(-2000, 0.6, 1000, 0)]
        [TestCase(2000, 0.6, -1000, 0)]
        [TestCase(2000, -0.6, 1000, 0)]
        public void NonRiskAssetsNeededInCaseOfRiskAssetCrash_InvalidInput(double totalAmount, double stocksCrashFactor, double totalAmountMinNeededAfterCrash, double expectedLowRiskAmount)
        {
            Assert.That(() => myFinanceMath.NonRiskAssetsNeededInCaseOfRiskAssetCrash(totalAmount, stocksCrashFactor, totalAmountMinNeededAfterCrash), Throws.ArgumentException);
        }

        [TestCase(2000, 1000, 1000, 3, 1.5, 2, 500, 50, 60)]
        public void Test_RateByNumericalSparkassenformel(decimal betrag_cash, decimal betrag_stocks, decimal betrag_metals, decimal zins_cash, decimal zins_stocks, decimal zins_metals, decimal endBetrag, int yearStart, int yearEnd)
        {
            int anzahlJahre = yearEnd - yearStart;
            decimal gesamtBetrag = betrag_cash + betrag_stocks + betrag_metals;

            decimal rate = myFinanceMath.RateByNumericalSparkassenformel(
                betrag_cash, betrag_stocks, betrag_metals ,
                zins_cash, zins_stocks, zins_metals,
                endBetrag, yearStart, yearEnd,
                out _);

            Console.WriteLine($"{nameof(gesamtBetrag)}: {gesamtBetrag}");
            Console.WriteLine($"{nameof(rate)}: {rate}");

            decimal faktorCash = betrag_cash / gesamtBetrag;
            decimal faktorStocks = betrag_stocks / gesamtBetrag;
            decimal faktorMetals = betrag_metals / gesamtBetrag;

            Console.WriteLine($"{nameof(faktorCash)}: {faktorCash}");
            Console.WriteLine($"{nameof(faktorCash)}: {faktorCash}");
            Console.WriteLine($"{nameof(faktorCash)}: {faktorCash}");

            decimal restbetrag = gesamtBetrag;
            for (int i = 0; i < anzahlJahre; i++)
            {
                // rate runter
                restbetrag -= rate;

                // zinsen drauf
                decimal anteilBetrag_cash = faktorCash * restbetrag;
                decimal anteilBetrag_stocks = faktorStocks * restbetrag;
                decimal anteilBetrag_metals = faktorMetals * restbetrag;
                Assert.That(anteilBetrag_cash + anteilBetrag_stocks + anteilBetrag_metals, Is.EqualTo(restbetrag).Within(0.001));

                anteilBetrag_cash *= (1 + (zins_cash / 100m));
                anteilBetrag_stocks *= (1 + (zins_stocks / 100m)); ;
                anteilBetrag_metals *= (1 + (zins_metals / 100m)); ;

                restbetrag = anteilBetrag_cash + anteilBetrag_stocks + anteilBetrag_metals;

                //faktoren neu berechnen da sich durch die unterschiedlichen zinssätze die asset zusammensetzung geändert hat
                faktorCash = anteilBetrag_cash / restbetrag;
                faktorStocks = anteilBetrag_stocks / restbetrag;
                faktorMetals = anteilBetrag_metals / restbetrag;
            }

            Assert.That(restbetrag, Is.EqualTo(endBetrag).Within(0.001));
        }


        // todo: das hier wäre ein kandidat für data driven tests, da gibts doch frameworks dafür
        [TestCase(200, 7000, 12000, 2000, 0.1, 8, 2, 5000, 50, 60)]
        public void Test_StartCapitalByNumericalSparkassenformel(decimal totalRatePerYear, decimal betrag_cash, decimal betrag_stocks, decimal betrag_metals, decimal zins_cash, decimal zins_stocks, decimal zins_metals, decimal endBetrag, int yearStart, int yearEnd)
        {
            int anzahlJahre = yearEnd - yearStart;

            decimal startCapital = myFinanceMath.StartCapitalByNumericalSparkassenformel(
                totalRatePerYear,
                betrag_cash, betrag_stocks, betrag_metals,
                zins_cash, zins_stocks, zins_metals,
                endBetrag,
                yearStart, yearEnd,
                out _);

            decimal restbetrag = startCapital;

            decimal gesamtBetrag = betrag_cash + betrag_stocks + betrag_metals;
            decimal faktorCash = betrag_cash / gesamtBetrag;
            decimal faktorStocks = betrag_stocks / gesamtBetrag;
            decimal faktorMetals = betrag_metals / gesamtBetrag;

            for (int i = 0; i < anzahlJahre; i++)
            {
                // rate runter
                restbetrag -= totalRatePerYear;

                // zinsen drauf
                decimal anteilBetrag1 = (decimal)faktorCash * restbetrag;
                decimal anteilBetrag2 = (decimal)faktorStocks * restbetrag;
                decimal anteilBetrag3 = (decimal)faktorMetals * restbetrag;
                Assert.That(anteilBetrag1 + anteilBetrag2 + anteilBetrag3, Is.EqualTo(restbetrag).Within(0.001), "all part amounts should sum up to total amount.");

                anteilBetrag1 *= 1 + (zins_cash / 100m);
                anteilBetrag2 *= 1 + (zins_stocks / 100m);
                anteilBetrag3 *= 1 + (zins_metals / 100m);

                restbetrag = anteilBetrag1 + anteilBetrag2 + anteilBetrag3;

                //faktoren neu berechnen da sich durch die unterschiedlichen zinssätze die asset zusammensetzung geändert hat
                faktorCash = anteilBetrag1 / restbetrag;
                faktorStocks = anteilBetrag2 / restbetrag;
                faktorMetals = anteilBetrag3 / restbetrag;
            }

            Assert.That(restbetrag, Is.EqualTo(endBetrag).Within(0.001), "end amount should be reached.");
        }
    }
}