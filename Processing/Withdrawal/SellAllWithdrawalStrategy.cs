using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processing.Assets;

namespace Processing.Withdrawal
{
    internal class SellAllWithdrawalStrategy : IWithdrawalStrategy
    {
        private Portfolio portfolio;
        private WithdrawalRateInfo result;

        public SellAllWithdrawalStrategy(Portfolio basePortfolio)
        {
            portfolio = basePortfolio;
        }

        public void Adder(double amount)
        {
            throw new NotImplementedException();
        }

        public void Calculate()
        {
            //sell everything
            result = new WithdrawalRateInfo();
            

        }

        public WithdrawalRateInfo GetResults()
        {
            return result;
        }
    }
}
