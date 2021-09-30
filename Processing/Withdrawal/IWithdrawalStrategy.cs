using System;

namespace Processing.Withdrawal
{
    public interface IWithdrawalStrategy
    {
        public double SimulateTaxesAtWithdrawal(int age, double amount);

        public double GetWithdrawalAmount(int age);
        public double GetWithdrawalAmount(int age, Type assetType);
    }
}
