namespace Domain
{
    /// <summary>
    /// This class holds all input data in a flat hierarchy
    /// </summary>
    public class LifeAssumptions
    {
        public int ageCurrent = 43;
        //public int ageStopWork = 60;
        public int ageRentStart = 67;
        public int ageEnd = 80;

        public CAmount stocks = new CAmount { FromDeposits = 118033 };
        public int stocksGrowthRate = 5;
        public int stocksSaveAmountPerMonth = 1171;

        public CAmount cash = new CAmount { FromDeposits = 44000 };
        public int cashGrowthRate = 0;
        public int cashSaveAmountPerMonth = 0;

        public CAmount metals = new CAmount { FromDeposits = 21200 };
        public int metalsGrowthRate = 1;
        public int metalsSaveAmountPerMonth = 0;

        public decimal needsCurrentAgeMinimal_perMonth = 2000;
        public decimal needsCurrentAgeComfort_perMonth = 3000;

        public double inflationRate = 0.03d;

        //https://www.finanzrechner.org/sonstige-rechner/rentenbesteuerungsrechner/
        public decimal netStateRentFromCurrentAge_perMonth = 827;
        public decimal netStateRentFromRentStartAge_perMonth = 2025;
        public decimal grossStateRentFromCurrentAge_perMonth = 924;
        public decimal grossStateRentFromRentStartAge_perMonth = 2703;


        //TODO: This is also valid for stop work age: rename it accordingly
        public decimal rentPhase_InterestRate_Cash = 0m;
        public decimal rentPhase_InterestRate_Stocks_GoodCase = 0.06m;
        public decimal rentPhase_InterestRate_Stocks_BadCase = 0m;
        public decimal rentPhase_CrashFactor_Stocks_BadCase = 0.5m;
        public readonly decimal rentPhase_CrashFactor_Stocks_GoodCase = 1;  //never change that. Rent phase cannot and is not intended handle a crash in good case! This is just for convenience.

        public decimal taxFactor_Stocks = 1.26m;

        public LifeAssumptions()
        {
        }
    }
}
