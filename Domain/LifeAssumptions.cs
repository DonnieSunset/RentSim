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

        public CAmount Cash { get; set; } = new CAmount { FromDeposits = 40000 };
        public int CashGrowthRate { get; set; } = 0;
        public int CashSaveAmountPerMonth { get; set; } = 0;

        public CAmount Stocks { get; set; } = new CAmount { FromDeposits = 140000 };
        public int StocksGrowthRate { get; set; } = 4;
        public int StocksSaveAmountPerMonth { get; set; } = 1071;

        public CAmount Metals { get; set; } = new CAmount { FromDeposits = 21000 };
        public int MetalsGrowthRate { get; set; } = 1;
        public int MetalsSaveAmountPerMonth { get; set; } = 0;

        public decimal NeedsCurrentAgeMinimal_perMonth { get; set; } = 2000;
        public decimal NeedsCurrentAgeComfort_perMonth { get; set; } = 3000;

        public double InflationRate { get; set; } = 0.03d;

        //https://www.finanzrechner.org/sonstige-rechner/rentenbesteuerungsrechner/
        public decimal NetStateRentFromCurrentAge_perMonth { get; set; } = 933;
        public decimal NetStateRentFromRentStartAge_perMonth { get; set; } = 2156;
        public decimal GrossStateRentFromCurrentAge_perMonth { get; set; } = 1053;
        public decimal GrossStateRentFromRentStartAge_perMonth { get; set; } = 2885;


        //TODO: This is also valid for stop work age: rename it accordingly
        public decimal RentPhase_InterestRate_Cash { get; set; } = 0m;
        public decimal RentPhase_InterestRate_Stocks_GoodCase { get; set; } = 0.06m;
        public decimal RentPhase_InterestRate_Stocks_BadCase { get; set; } = 0m;
        public decimal RentPhase_CrashFactor_Stocks_BadCase { get; set; } = 0.5m;
        public decimal RentPhase_CrashFactor_Stocks_GoodCase { get; } = 1;  //never change that. Rent phase cannot and is not intended handle a crash in good case! This is just for convenience.

        public decimal TaxFactor_Stocks { get; set; } = 1.26m;
    }
}
