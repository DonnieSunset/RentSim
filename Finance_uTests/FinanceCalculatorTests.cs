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
    }
}