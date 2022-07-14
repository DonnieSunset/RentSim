using Finance;
using NUnit.Framework;

namespace Finance_uTests
{
    [TestFixture]
    internal class FracTests
    {
        [TestCase(1)]
        [TestCase(1.05)]
        [TestCase(2)]
        public void FromFactor_ValidFactor_ReturnsCorrectObject(decimal factor)
        {
            var frac = Frac.FromFactor(factor);
            Assert.That(frac, Is.Not.Null);
            Assert.That(frac.Rate, Is.EqualTo(factor-1));
        }

        [TestCase(-1)]
        [TestCase(2.05)]
        [TestCase(0.9999)]
        public void FromFactor_InvalidFactor_ThrowsException(decimal factor)
        {
            Assert.That(() => Frac.FromFactor(factor), Throws.Exception);
        }

        [TestCase(0)]
        [TestCase(0.05)]
        [TestCase(1)]
        public void FromRate_ValidRate_ReturnsCorrectObject(decimal rate)
        {
            var frac = Frac.FromRate(rate);
            Assert.That(frac, Is.Not.Null);
            Assert.That(frac.Rate, Is.EqualTo(rate));
        }

        [TestCase(1.05)]
        [TestCase(-0.0001)]
        public void FromRate_InvalidRate_ThrowsException(decimal rate)
        {
            Assert.That(() => Frac.FromRate(rate), Throws.Exception);
        }

        [TestCase(1.25, 100, 25)]
        [TestCase(1, 100, 0)]
        [TestCase(2, 100, 100)]
        [TestCase(1.33, 0, 0)]
        public void ForAmount_ValidAmount_ReturnsCorrectTaxes(decimal factor, decimal amount, decimal expectedTaxes)
        {
            var frac = Frac.FromFactor(factor);
            var taxes = frac.ForAmount(amount);

            Assert.That(taxes, Is.EqualTo(expectedTaxes));
        }

        [TestCase(2, 0, 0, 0)]
        [TestCase(2, 100, 50, 50)]
        [TestCase(1, 100, 100, 0)]
        [TestCase(1.25, 100, 80, 20)]
        public void FromTotal_ValidAmount_ReturnsCorrectTaxesAndAmount(decimal factor, decimal total, decimal expectedAmount, decimal expectedTaxes)
        {
            var frac = Frac.FromFactor(factor);
            (var amount, var taxes) = frac.FromTotal(total);

            Assert.That(amount + taxes, Is.EqualTo(total));
            Assert.That(amount, Is.EqualTo(expectedAmount));
            Assert.That(taxes, Is.EqualTo(expectedTaxes));
        }
    }
}
