
using Domain;
using Protocol;

namespace Portfolio
{
    public class AssetPortfolio
    {
        private readonly Cash myCash;
        //private readonly MarketAssumptions myMarketAssumptions;
        private readonly LifeAssumptions myLifeAssumptions;

        public AssetPortfolio(LifeAssumptions lifeAssumptions)
        {
            myLifeAssumptions = lifeAssumptions;
            myCash = new Cash(lifeAssumptions.cash, lifeAssumptions.cashGrowthRate);
        }

        public TransactionDetails SaveCash(decimal amount)
        {
            myCash.Save(amount);

            return new TransactionDetails()
            {
                cashDeposits = amount
            };
        }

        //public TransactionDetails WithdrawCash(decimal amount)
        //{
        //    myCash.Withdraw(amount);

        //    return new TransactionDetails()
        //    {
        //        cashDeposits = -amount,
        //    };
        //}

        //public TransactionDetails GetInterestsForCash()
        //{
        //    //decimal amount = InterestCalculator.GetInterestsFor(myCash.Total, myCash.InterestsPercent);

        //    //myCash.ApplyInterests(amount);

        //    //return new TransactionDetails()
        //    //{
        //    //    cashInterests = amount,
        //    //};
        //}

    }
}
