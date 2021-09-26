using Processing.Assets;
using System.Collections.Generic;

namespace Processing.Withdrawal
{
    public interface IWithdrawalStrategy
    {
        public double SimulateTaxesAtWithdrawal(double amount);
    }
}
