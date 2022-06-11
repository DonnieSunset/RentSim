using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing
{
    /// <summary>
    /// This class is used to calculate the possible yearly withdrawals from the time of rent start
    /// until the time of death.
    /// 
    /// Assumptions:
    /// 1) The needed money for the rent is divided in a minimum amount (to cover the absolute minimum
    /// in order to survive) and a "comfort" amount, needed to live comfortly. 
    /// 2) At the beginning of the rent phase, we have two asset classes. One with low or zero risk 
    /// which gives low or zero interests and one asset class with high risk and high interests, typically 
    /// stocks.
    /// 3) The minimum needed amount shall be covered by the low risk savings. This ensures that even in 
    /// a stock market crash, the minumum money is available to cover the basic needs. The rest shall be 
    /// covered by high risk such that in average expected interests, the comfort amount of yearly
    /// withdrawings is possible.
    /// 
    /// Input:
    /// * Age start rent
    /// * Age end
    /// * Needed minimum total amount from age rent start to age end
    /// * Needed comfort amount from age rent start to age end
    /// * monthly net rent (depends on age stop work!!!)
    /// 
    /// Output: 
    /// * Yearly withdrawals of low risk assets
    /// * Yearly withdrawals of high risk assets
    /// -- both considering taxes and interests and inflation
    /// 
    /// </summary>
    internal class RentPhase
    {
        public static (double yearlyWithdrawalLowRiskAsset, double yearlyWithdrawalHighRiskAsset) CalculateRentPhaseWithdrawals(
            int ageRentStart, 
            int ageEnd,
            double totalNeededAmountRentPhaseMinimum,
            double totalNeededAmountRentPhaseComfort,
            double monthlyNetRent)
        {


            return (0, 0);
        }
    }
}
