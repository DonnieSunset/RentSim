using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processing;

namespace Processing_uTest
{
    [TestClass]
    public class RentSimMathTests
    {
        [DataTestMethod]
        [DataRow(7f, 0.56541454f)]
        public void InterestPerYearToInterestPerMonth_validInterest_validResult(double interestPerYear, double interestPerMonth)
        {
            double result = RentSimMath.InterestPerYearToInterestPerMonth(interestPerYear);

            Assert.AreEqual(interestPerMonth, result, 0.0001f);
        }
    }
}
