using FinanceMathService;
using FinanceMathService.DTOs;
using NUnit.Framework;

namespace FinanceMathService_Tests
{
    public class FinanceMathControllerTests
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
            var input = new RateByNumericalSparkassenformelInputDTO
            {
                AgeFrom = yearStart,
                AgeTo = yearEnd,

                StartCapitalCash = new CAmount { FromDeposits = betrag_cash },
                StartCapitalStocks = new CAmount { FromDeposits = betrag_stocks },
                StartCapitalMetals = new CAmount { FromDeposits = betrag_metals },

                GrowthRateCash = zins_cash,
                GrowthRateStocks = zins_stocks,
                GrowthRateMetals = zins_metals,

                EndCapitalTotal = endBetrag
            };

            var result = myFinanceMath.RateByNumericalSparkassenformel(input);

            decimal restbetrag = betrag_cash + betrag_stocks + betrag_metals;
            for (int i = 0; i < yearEnd - yearStart; i++)
            {
                // rate runter
                restbetrag += result.Entities[i].Deposits.Cash;
                restbetrag += result.Entities[i].Deposits.Stocks;
                restbetrag += result.Entities[i].Deposits.Metals;

                // zinsen drauf
                restbetrag += result.Entities[i].Interests.Cash;
                restbetrag += result.Entities[i].Interests.Stocks;
                restbetrag += result.Entities[i].Interests.Metals;

                //steuern runter
                restbetrag += result.Entities[i].Taxes.Cash;
                restbetrag += result.Entities[i].Taxes.Stocks;
                restbetrag += result.Entities[i].Taxes.Metals;
            }

            Assert.That(restbetrag, Is.EqualTo(endBetrag).Within(0.001), "end amount should be reached.");
        }


        // todo: das hier wäre ein kandidat für data driven tests, da gibts doch frameworks dafür
        [TestCase(-20000, 10000, 300000, 20000, 0.1, 8, 2, 0, 50, 60)]
        public void Test_StartCapitalByNumericalSparkassenformel(decimal totalRatePerYear, decimal betrag_cash, decimal betrag_stocks, decimal betrag_metals, decimal zins_cash, decimal zins_stocks, decimal zins_metals, decimal endBetrag, int yearStart, int yearEnd)
        {
            var input = new StartCapitalByNumericalSparkassenformelInputDTO
            {
                AgeFrom = yearStart,
                AgeTo = yearEnd,

                StartCapitalCash = new CAmount() { FromDeposits = betrag_cash },
                StartCapitalStocks = new CAmount() { FromDeposits = betrag_stocks },
                StartCapitalMetals = new CAmount() { FromDeposits = betrag_metals },

                GrowthRateCash = zins_cash,
                GrowthRateStocks = zins_stocks,
                GrowthRateMetals = zins_metals,

                TotalRateNeeded_PerYear = totalRatePerYear,
            };
            input.Validate();

            var result = myFinanceMath.StartCapitalByNumericalSparkassenformel(input);

            decimal restbetrag = result.FirstYearBeginValues.Cash + result.FirstYearBeginValues.Stocks + result.FirstYearBeginValues.Metals;
            for (int i = 0; i < yearEnd - yearStart; i++)
            {
                // rate runter
                restbetrag += result.Entities[i].Deposits.Cash;
                restbetrag += result.Entities[i].Deposits.Stocks;
                restbetrag += result.Entities[i].Deposits.Metals;

                // zinsen drauf
                restbetrag += result.Entities[i].Interests.Cash;
                restbetrag += result.Entities[i].Interests.Stocks;
                restbetrag += result.Entities[i].Interests.Metals;

                // steuern runter
                restbetrag += result.Entities[i].Taxes.Cash;
                restbetrag += result.Entities[i].Taxes.Stocks;
                restbetrag += result.Entities[i].Taxes.Metals;
            }

            Assert.That(restbetrag, Is.EqualTo(endBetrag).Within(0.001), "End amount should be reached.");
        }
    }
}