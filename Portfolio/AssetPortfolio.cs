
namespace Portfolio
{
    public class AssetPortfolio
    {
        private readonly Cash myCash;

        public AssetPortfolio()
        {
            myCash = new Cash();
        }

        public TransactionDetails SaveCash(decimal amount)
        {
            myCash.Save(amount);

            return new TransactionDetails()
            {
                cashDeposits = amount
            };
        }

        public TransactionDetails WithdrawCash(decimal amount)
        {
            myCash.Withdraw(amount);

            return new TransactionDetails()
            {
                cashDeposits = -amount,
            };
        }

    }
}
