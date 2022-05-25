namespace Domain
{
    public class LifeAssumptions
    {
        public int ageCurrent = 42;
        public int ageStopWork = 60;
        public int ageRentStart = 67;
        public int ageEnd = 80;

        public int stocks = 88800;
        public int stocksGrowthRate = 7;
        public int stocksSaveAmountPerMonth = 700;

        public int cash = 58000;
        public int cashGrowthRate = 0;
        public int cashSaveAmountPerMonth = 350;

        public int metals = 21400;
        public int metalsGrowthRate = 1;
        public int metalsSaveAmountPerMonth = 0;

        public decimal needsNowMinimum = 1900;
        public decimal needsNowComfort = 2600;

        //https://www.finanzrechner.org/sonstige-rechner/rentenbesteuerungsrechner/
        private decimal netStateRentFromCurrentAge = 827;
        private decimal netStateRentFromRentStartAge = 2025;

        public Rent Rent { get; private set; }
        public RentPhase RentPhase { get; private set; }

        public LifeAssumptions()
        {
            RentPhase = new RentPhase
            {
                Cash_InerestRate = 1.01m,
                Stocks_InterestRate_BadCase = 1.01m,
                Stocks_InterestRate_GoodCase = 1.06m,
                Stocks_CrashFactor_BadCase = 0.5m,
                DurationInYears = ageEnd - ageRentStart,
            };

            Rent = new Rent(netStateRentFromCurrentAge, netStateRentFromRentStartAge, RentPhase.DurationInYears);
        }
    }
}
