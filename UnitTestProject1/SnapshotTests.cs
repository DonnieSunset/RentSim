using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processing;

namespace Processing_uTest
{
    [TestClass]
    public class SnapshotTests
    {
        [DataTestMethod]
        [DataRow(7f, 0.56541454f)]
        public void InterestPerYearToInterestPerMonth_validInterest_validResult(double interestPerYear, double interestPerMonth)
        {
            var testee = new RentSimResultRow(null);

            double result = testee.InterestPerYearToInterestPerMonth(interestPerYear);

            Assert.AreEqual(interestPerMonth, result, 0.0001f);
        }
    }
}
