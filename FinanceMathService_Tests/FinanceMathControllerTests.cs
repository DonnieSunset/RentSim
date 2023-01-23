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

        [TestCase(2000, 1.03, 1000, 1.15, 1000, 1.02, 500, 50, 60)]
        public void Test_RateByNumericalSparkassenformel(double betrag1, double zins1, double betrag2, double zins2, double betrag3, double zins3, double endBetrag, int yearStart, int yearEnd)
        {
            int anzahlJahre = yearEnd - yearStart;
            double gesamtBetrag = betrag1 + betrag2 + betrag3;

            double rate = myFinanceMath.RateByNumericalSparkassenformel(
                new List<double> { betrag1, betrag2, betrag3 },
                new List<double> { zins1, zins2, zins3 },
                endBetrag, yearStart, yearEnd);

            Console.WriteLine($"{nameof(gesamtBetrag)}: {gesamtBetrag}");
            Console.WriteLine($"{nameof(rate)}: {rate}");

            double faktor1 = betrag1 / gesamtBetrag;
            double faktor2 = betrag2 / gesamtBetrag;
            double faktor3 = betrag3 / gesamtBetrag;

            Console.WriteLine($"{nameof(faktor1)}: {faktor1}");
            Console.WriteLine($"{nameof(faktor1)}: {faktor1}");
            Console.WriteLine($"{nameof(faktor1)}: {faktor1}");

            double restbetrag = gesamtBetrag;
            for (int i = 0; i < anzahlJahre; i++)
            {
                // rate runter
                restbetrag -= rate;

                // zinsen drauf
                double anteilBetrag1 = faktor1 * restbetrag;
                double anteilBetrag2 = faktor2 * restbetrag;
                double anteilBetrag3 = faktor3 * restbetrag;
                Assert.That(anteilBetrag1 + anteilBetrag2 + anteilBetrag3, Is.EqualTo(restbetrag).Within(0.001));

                anteilBetrag1 *= zins1;
                anteilBetrag2 *= zins2;
                anteilBetrag3 *= zins3;

                restbetrag = anteilBetrag1 + anteilBetrag2 + anteilBetrag3;
            }

            Assert.That(restbetrag, Is.EqualTo(endBetrag).Within(0.001));
        }


        [TestCase(200, 1/7d, 3, 2/7d, 1.5, 4/7d, 2, 5000, 50, 60)]
        public void Test_StartCapitalByNumericalSparkassenformel(decimal totalRatePerYear, double faktorCash, double zinsCash, double faktorStocks, double zinsStocks, double faktorMetals, double zinsMetals, decimal endBetrag, int yearStart, int yearEnd)
        {
            int anzahlJahre = yearEnd - yearStart;

            decimal startCapital = myFinanceMath.StartCapitalByNumericalSparkassenformel(
                totalRatePerYear,
                faktorCash, faktorStocks, faktorMetals,
                zinsCash, zinsStocks, zinsMetals,
                endBetrag,
                yearStart, yearEnd,
                out _);

            decimal restbetrag = startCapital;
            for (int i = 0; i < anzahlJahre; i++)
            {
                // rate runter
                restbetrag -= totalRatePerYear;

                // zinsen drauf
                decimal anteilBetrag1 = (decimal)faktorCash * restbetrag;
                decimal anteilBetrag2 = (decimal)faktorStocks * restbetrag;
                decimal anteilBetrag3 = (decimal)faktorMetals * restbetrag;
                Assert.That(anteilBetrag1 + anteilBetrag2 + anteilBetrag3, Is.EqualTo(restbetrag).Within(0.001), "all part amounts should sum up to total amount.");

                anteilBetrag1 *= (decimal)(1 + (zinsCash / 100d));
                anteilBetrag2 *= (decimal)(1 + (zinsStocks / 100d));
                anteilBetrag3 *= (decimal)(1 + (zinsMetals / 100d));

                restbetrag = anteilBetrag1 + anteilBetrag2 + anteilBetrag3;

                //faktoren neu berechnen da sich durch die unterschiedlichen zinssätze die asset zusammensetzung geändert hat
                faktorCash = (double)(anteilBetrag1 / restbetrag);
                faktorStocks = (double)(anteilBetrag2 / restbetrag);
                faktorMetals = (double)(anteilBetrag3 / restbetrag);
            }

            Assert.That(restbetrag, Is.EqualTo(endBetrag).Within(0.001), "end amount should be reached.");
        }
    }
}