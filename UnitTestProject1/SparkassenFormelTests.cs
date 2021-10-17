using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Processing;
using Processing.Withdrawal;
using System;

namespace Processing_uTest
{
    [TestClass]
    public class SparkassenFormelTests
    {
        // stopWorkAge = 60; rentAge = 67; endAge=75
        // stopWorkPhase = 67-60=7; rentPhase = 75-67=8
        //
        // 60  61  62  63  64  65  66  67  68  69  70  71  72  73  74 75
        // |_1_|_2_|_3_|_4_|_5_|_6_|_7_|_1_|_2_|_3_|_4_|_5_|_6_|_7_|_8_|
        //
        //
        //TODO: das ist jetzt brutto, da muss noch die kapitalertragssteuer weg
        //TODO: das klappt jetzt mit vorschüssigem zins, aber will ich das nicht eigneltich mit nachschüssigem zins? oder konfigurierbar?
        //todo: replace all double with decimal
        //todo: translate to english
        [DataTestMethod]
        [DataRow(600000, 7, 13, 8, 0, 1800)]
        [DataRow(12000, 1, 13, 8, 0, 1000)]
        public void CalculatePayoutRateWithRent_ZeroTaxes_RatesLeadToEndKapital(double startCapital, int yearsStopWorkPhase, int yearsRentPhase, double interestRate, double endCapital, double rent)
        {
            (double rateRent, double rateStopWork) = SparkassenFormel.CalculatePayoutRateWithRent(startCapital, yearsStopWorkPhase, yearsRentPhase, interestRate, endCapital, rent, GetMockedZeroTaxWithdrawalStrategyFunc());
            Assert.AreEqual(rent, rateStopWork - rateRent, 0.1);

            double currentCapital = startCapital;
            for (int i = 1; i <= yearsStopWorkPhase; i++)
            {
                currentCapital -= rateStopWork * 12;
                currentCapital *= 1 + (interestRate / 100d);
                Console.WriteLine($"jahr {i} Aktuelles Kapital: {currentCapital}");
            }
            for (int j = 1; j <= yearsRentPhase; j++)
            {
                currentCapital -= rateRent * 12;
                currentCapital *= 1 + (interestRate / 100d);
                Console.WriteLine($"jahr {j} Aktuelles Kapital: {currentCapital}");
            }

            Assert.AreEqual(endCapital, currentCapital, 0.01);
        }

        [DataTestMethod]
        [DataRow(60000, 7, 13, 8, 0, 1800)]
        [DataRow(12000-1, 1, 13, 8, 0, 1000)]
        public void CalculatePayoutRateWithRent_CapitalTooSmall_ThrowsException(double startCapital, int yearsStopWorkPhase, int yearsRentPhase, double interestRate, double endCapital, double rent)
        {
            Action action = () => SparkassenFormel.CalculatePayoutRateWithRent(startCapital, yearsStopWorkPhase, yearsRentPhase, interestRate, endCapital, rent, GetMockedZeroTaxWithdrawalStrategyFunc());
            
            Assert.ThrowsException<Exception>(action);
        }

        private Func<double, double> GetMockedZeroTaxWithdrawalStrategyFunc()
        {
            var mockWithdrawalStrategy = new Mock<IWithdrawalStrategy>();
            mockWithdrawalStrategy.Setup(o => o.SimulateTaxesAtWithdrawal(It.IsAny<int>(), It.IsAny<double>())).Returns(0);

            Func<double, double> mockedZeroTaxWithdrawalStrategyFunc = (double amount) => mockWithdrawalStrategy.Object.SimulateTaxesAtWithdrawal(0, amount);

            return mockedZeroTaxWithdrawalStrategyFunc;
        }
    }
}
