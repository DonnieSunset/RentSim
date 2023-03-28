using FinanceMathService.DTOs;
using NUnit.Framework;

namespace FinanceMathService_Tests
{
    [TestFixture]
    internal class CAmount_uTests
    {
        [TestCase(1500, 2000, 300)]
        public void DistributeEquallyTest(decimal fromDeposits, decimal fromInterests, decimal amountToDistribute)
        {
            CAmount cAmount = new CAmount()
            {
                FromDeposits = fromDeposits,
                FromInterests = fromInterests
            };

            cAmount.DistributeEqually(amountToDistribute);

            Assert.That(cAmount.Total, Is.EqualTo(fromDeposits + fromInterests + amountToDistribute));
        }

        [TestCase(800, 200, 1000, 800, 200)]
        [TestCase(800, 200, 2000, 1600, 400)]
        [TestCase(800, 200, 100, 80, 20)]
        public void FromTest(decimal fromDepositsInput, decimal fromInterestsResult, decimal amountToDistribute, decimal fromDepositExpResult, decimal fromInterestsExpResult)
        {
            CAmount cAmount = new CAmount()
            {
                FromDeposits = fromDepositsInput,
                FromInterests = fromInterestsResult
            };

            CAmount result = CAmount.From(amountToDistribute, cAmount);

            Assert.Multiple(() =>
            {
                Assert.That(result.FromDeposits, Is.EqualTo(fromDepositExpResult).Within(0.001));
                Assert.That(result.FromInterests, Is.EqualTo(fromInterestsExpResult).Within(0.001));
            });
        }
    }
}
