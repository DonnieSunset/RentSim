namespace Domain
{
    public class RentPhase
    {
        public decimal Stocks_InterestRate_GoodCase { get; internal set; }
        public decimal Stocks_InterestRate_BadCase { get; internal set; }
        public decimal Stocks_CrashFactor_BadCase { get; internal set; }
        public decimal Cash_InerestRate { get; internal set; }
        public int DurationInYears { get; internal set; }
    }
}