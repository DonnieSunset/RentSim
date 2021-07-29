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

        [DataTestMethod]
        [DataRow(50, 60, 70, 1000, 3000, 2000)]
        [DataRow(41, 60, 67, 762, 2015, 1677.65d)]
        [DataRow(67, 67, 67, 762, 2015, 2015)]
        public void RentStopWorkAgeApproximation_validInput_validResults(int currentAge, int stopWorkAge, int rentStartAge, int currentRent, int rentStartRent, double expectedResult)
        {
            double result = RentSimMath.RentStopWorkAgeApproximation(currentAge, stopWorkAge, rentStartAge, currentRent, rentStartRent);

            Assert.AreEqual(expectedResult, result, 0.01f);
        }
    }
}
