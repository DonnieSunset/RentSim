using Finance;
using NUnit.Framework;

namespace Finance_uTests
{
    [TestFixture]
    public class FinanceCalculatorTests
    {
        [TestCase(1000, 1000, 70, 35, 35)]
        [TestCase(1000, 0, 70, 70, 0)]
        [TestCase(0, 1000, 70, 0, 70)]
        [TestCase(2000, 1000, 120, 80, 40)]
        public void WithdrawUniformFromTwoAmounts_TwoValidSets_ReturnsCorrectResults(decimal amount1, decimal amount2, decimal withdrawalAmount, decimal expectedPartAmount1, decimal expectedPartAmount2)
        {
            (decimal result1, decimal result2) = FinanceCalculator.WithdrawUniformFromTwoAmounts(amount1, amount2, withdrawalAmount);

            Assert.That(result1, Is.EqualTo(expectedPartAmount1).Within(0.000000000000001));
            Assert.That(result2, Is.EqualTo(expectedPartAmount2).Within(0.000000000000001));
        }

        [Test]
        public void Blubb()
        {
            double stocks_interestRate_goodCase = 1.07;
            double cash_interestRate_goodCase = 1.01;
            double stocks_interestRate_badCase = 1.0;
            double cash_interestRate_badCase = 1.0;
            int years = 13;

            double stocks_crashFactor_badCase = 0.5;

            double minimum_total_needed = 2000 * 12 * years;
            double comfort_total_needed = 3000 * 12 * years;

            double rent = 1690 * 12 * years;

            double cash_needed;
            double stocks_needed;

            double stocks_interestFactor_goodCase = Math.Pow(stocks_interestRate_goodCase, 13);
            double cash_interestFactor_goodCase = Math.Pow(cash_interestRate_goodCase, 13);
            double stocks_interestFactor_badCase = Math.Pow(stocks_interestRate_badCase, 13);
            double cash_interestFactor_badCase = Math.Pow(cash_interestRate_badCase, 13);

            //(1) 3000 = rente + (festgeld * 1.00^13) + (aktien * 1.00^13 * 0.5)
            //(2) 5000 = rente + (festgeld * 1.01^13) + (aktien * 1.08^13)

            //(1) minimum_total_needed = rent + (cash_needed * cash_interestFactor_badCase) + (stocks_needed * stocks_interestFactor_badCase * stocks_crashFactor_badCase)
            //(2) comfort_total_needed = rent + (cash_needed * cash_interestFactor_goodCase) + (stocks_needed * stocks_interestFactor_goodCase)

            // py https://live.sympy.org/
            // minimum_total_needed, rent, cash_needed, cash_interestFactor_badCase, stocks_needed, stocks_interestFactor_badCase, stocks_crashFactor_badCase, comfort_total_needed, cash_interestFactor_goodCase, stocks_interestFactor_goodCase = symbols('minimum_total_needed rent cash_needed cash_interestFactor_badCase stocks_needed stocks_interestFactor_badCase stocks_crashFactor_badCase comfort_total_needed cash_interestFactor_goodCase stocks_interestFactor_goodCase')
            // eq1 = Eq(rent + (cash_needed * cash_interestFactor_badCase) + (stocks_needed * stocks_interestFactor_badCase * stocks_crashFactor_badCase), minimum_total_needed)
            // eq2 = Eq(rent + (cash_needed * cash_interestFactor_goodCase) + (stocks_needed * stocks_interestFactor_goodCase), comfort_total_needed)
            // sol = solve((eq1, eq2),(cash_needed, stocks_needed))
            //
            // 

            cash_needed = (-stocks_crashFactor_badCase * stocks_interestFactor_badCase * (comfort_total_needed - rent) + stocks_interestFactor_goodCase * (minimum_total_needed - rent)) / (cash_interestFactor_badCase * stocks_interestFactor_goodCase - cash_interestFactor_goodCase * stocks_crashFactor_badCase * stocks_interestFactor_badCase);
            stocks_needed = (cash_interestFactor_badCase*(comfort_total_needed - rent) - cash_interestFactor_goodCase*(minimum_total_needed - rent)) / (cash_interestFactor_badCase*stocks_interestFactor_goodCase - cash_interestFactor_goodCase*stocks_crashFactor_badCase*stocks_interestFactor_badCase);

            Console.WriteLine($"cash_needed: {cash_needed}");
            Console.WriteLine($"stocks_needed: {stocks_needed}");

            Assert.True(true);
        }
    }
}