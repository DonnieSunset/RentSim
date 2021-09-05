namespace Processing.Withdrawal
{
    public interface IWithdrawalStrategy
    {
        public double SimulateTaxesAtWithdrawal(double amount);
    }
}
