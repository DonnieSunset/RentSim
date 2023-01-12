using FinanceMathService;
using NUnit.Framework;

namespace FinanceMathService_Tests
{
    public class FinanceMath_uTests
    {
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
            double lowRiskAMount = FinanceMath.NonRiskAssetsNeededInCaseOfRiskAssetCrash(totalAmount, stocksCrashFactor, totalAmountMinNeededAfterCrash);
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
            Assert.That(() => FinanceMath.NonRiskAssetsNeededInCaseOfRiskAssetCrash(totalAmount, stocksCrashFactor, totalAmountMinNeededAfterCrash), Throws.ArgumentException);
        }

        [TestCase(2000, 1.03, 1000, 1.15, 1000, 1.02, 500, 10)]
        public void Test_RateByNumericalSparkassenformel(double betrag1, double zins1, double betrag2, double zins2, double betrag3, double zins3, double endBetrag, int anzahlJahre)
        {
            double gesamtBetrag = betrag1 + betrag2 + betrag3;

            double rate = FinanceMath.RateByNumericalSparkassenformel(
                new List<double> { betrag1, betrag2, betrag3 },
                new List<double> { zins1, zins2, zins3 },
                endBetrag, anzahlJahre);

            Console.WriteLine($"{nameof(gesamtBetrag)}: {gesamtBetrag}");
            Console.WriteLine($"{nameof(rate)}: {rate}");

            double faktor1 = betrag1 / gesamtBetrag;
            double faktor2 = betrag2 / gesamtBetrag;
            double faktor3 = betrag3 / gesamtBetrag;

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
    }
}