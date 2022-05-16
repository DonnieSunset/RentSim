using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance
{
    public class FinanceCalculator
    {
        public static (decimal partOfAmount1, decimal partOfAmount2) WithdrawUniformFromTwoAmounts(decimal amount1, decimal amount2, decimal withdrawalAmount)
        {
            var fractionAmount1 = amount1 / (amount1 + amount2);
            var fractionAmount2 = amount2 / (amount1 + amount2);

            var deviation = Math.Abs(fractionAmount1 + fractionAmount2) - 1;

            if (deviation > (decimal)0.01)
            {
                throw new Exception($"Something went wrong here: Deviation <{deviation}> should be 0.");
            }

            var withdrawalAmount1 = fractionAmount1 * withdrawalAmount;
            var withdrawalAmount2 = fractionAmount2 * withdrawalAmount;

            return (withdrawalAmount1, withdrawalAmount2);
        }
    }
}
