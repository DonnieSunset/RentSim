using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processing;

namespace Processing_uTest
{
    [TestClass]
    internal class RentPhaseTests
    {
        [TestMethod]
        public void CalculateRentPhaseWithdrawals_bla_blubb()
        {
            (double yearlyWithdrawalLowRiskAsset, double yearlyWithdrawalHighRiskAsset) = RentPhase.CalculateRentPhaseWithdrawals(
                ageRentStart: 67,
                ageEnd: 80,
                totalNeededAmountRentPhaseMinimum: 200000, 
                totalNeededAmountRentPhaseComfort: 300000, 
                monthlyNetRent: 1600
                );
        }
    }
}
