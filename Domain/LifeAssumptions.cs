namespace Domain
{
    /// <summary>
    /// This class holds all input data in a flat hierarchy
    /// </summary>
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

        public decimal needsCurrentAgeMinimal = 1900;
        public decimal needsCurrentAgeComfort = 2600;

        public double inflationRate = 1.03d;

        //https://www.finanzrechner.org/sonstige-rechner/rentenbesteuerungsrechner/
        private decimal netStateRentFromCurrentAge = 827;
        private decimal netStateRentFromRentStartAge = 2025;

        public decimal rentPhase_InterestRate_Cash = 1.01m;
        public decimal rentPhase_InterestRate_Stocks_GoodCase = 1.06m;
        public decimal rentPhase_InterestRate_Stocks_BadCase = 1.01m;
        public decimal rentPhase_CrashFactor_Stocks_BadCase = 0.5m;


        public Rent Rent { get; private set; }
        public RentPhaseInputData RentPhase { get; private set; }

        public LifeAssumptions()
        {
            ///Rent phases will get their flat data here. In the constructor, they create the more complex structures that reqire calculations..
            RentPhase = new RentPhaseInputData(
                ageCurrent,
                ageRentStart,
                ageEnd,
                inflationRate,
                needsCurrentAgeMinimal,
                needsCurrentAgeComfort,
                rentPhase_InterestRate_Cash,
                rentPhase_InterestRate_Stocks_GoodCase,
                rentPhase_InterestRate_Stocks_BadCase,
                rentPhase_CrashFactor_Stocks_BadCase
                );
            //{
                //InterestRate_Cash = rentPhase_InterestRate_Cash,
                //InterestRate_Stocks_BadCase = rentPhase_InterestRate_Stocks_BadCase,
                //InterestRate_Stocks_GoodCase = rentPhase_InterestRate_Stocks_GoodCase,
                //CrashFactor_Stocks_BadCase = rentPhase_CrashFactor_Stocks_BadCase,

                //InflationRate = inflationRate,

                //AgeCurrent = ageCurrent,
                //AgeRentStart = ageRentStart,
                //AgeEnd = ageEnd,

                //NeedsCurrentAgeMinimal = needsNowMinimum,
                //NeedsCurrentAgeComfort = needsNowComfort
            //};

            Rent = new Rent(netStateRentFromCurrentAge, netStateRentFromRentStartAge, RentPhase.DurationInYears);
        }
    }
}
