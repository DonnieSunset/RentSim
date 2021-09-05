namespace Processing.Assets
{
    public interface IMustPayTaxesAfterWithdrawal
    {
        public double WithdrawalTaxRate { get; }

        public double GetTaxesAfterWithdrawal(double amount);
    }
}
