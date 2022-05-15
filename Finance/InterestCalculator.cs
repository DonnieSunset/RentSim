using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance
{
    public class InterestCalculator
    {
        public static decimal GetInterestsFor(decimal amount, int interestPercent)
        {
            double interestFactor = interestPercent / 100d;
            var result = (decimal)interestFactor * amount;

            return result;
        }
    }
}
