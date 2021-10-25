using System;

namespace Processing.Withdrawal
{
    public interface IWithdrawalStrategy
    {
        public WithdrawalRateInfo GetResults();

        public void Calculate();
    }
}
