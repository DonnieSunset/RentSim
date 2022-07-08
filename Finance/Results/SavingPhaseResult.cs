namespace Finance.Results
{
    public class SavingPhaseResult
    {
        public decimal savingsCash;
        public decimal savingsStocks;
        public decimal savingsMetals;

        public decimal SavingsTotal => savingsCash + savingsStocks + savingsMetals;

        public void Print()
        {
            string result = $"===============--- {nameof(SavingPhaseResult)} ---====================" + Environment.NewLine +
                            $"{nameof(savingsCash)}:    {savingsCash:F2}" + Environment.NewLine +
                            $"{nameof(savingsStocks)}:  {savingsStocks:F2}" + Environment.NewLine +
                            $"{nameof(savingsMetals)}:  {savingsMetals:F2}" + Environment.NewLine +
                            $"{nameof(SavingsTotal)}:   {SavingsTotal:F2}" + Environment.NewLine +
                            $"==========================================================" + Environment.NewLine;

            Console.WriteLine(result);
        }
    }
}
