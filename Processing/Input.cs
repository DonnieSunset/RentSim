namespace Processing
{
    public enum InterestRateType
    {
        Konform,
        Relativ
    }

    public class Input
    {
        public int ageCurrent;
        public int ageStopWork;
        public int ageRentStart;
        public int ageEnd;

        public InterestRateType interestRateType = InterestRateType.Konform;

        public int stocks;
        public int stocksGrowthRate;
        public int stocksMonthlyInvestAmount;

        public int cash;
        public int cashGrowthRate;
        public int cashMonthlyInvestAmount;

        public int metals;
        public int metalsGrowthRate;
        public int metalsMonthlyInvestAmount;

        public int netStateRentFromCurrentAge;
        public int netStateRentFromRentStartAge;

        public int NeedsNowMinimum;
        public int NeedsNowComfort;

    }
}
