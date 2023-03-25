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
    }
}
