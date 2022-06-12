using Finance;

namespace Domain
{
    //public class RentPhaseInputData
    //{
    //    public RentPhaseInputData(
    //        int ageCurrent,
    //        int ageRentStart,
    //        int ageEnd,
    //        double inflationRate,
    //        decimal needsCurrentAgeMinimal_perMonth,
    //        decimal needsCurrentAgeComfort_perMonth,
    //        decimal interestRate_Cash,
    //        decimal interestRate_Stocks_GoodCase,
    //        decimal interestRate_Stocks_BadCase,
    //        decimal crashFactor_Stocks_BadCase,
    //        decimal assumedRent_perMonth,
    //        decimal taxFactor_Stocks
    //        )
    //    {
    //        DurationInYears = ageEnd - ageRentStart;
    //        AgeRentStart = ageRentStart;
    //        AgeEnd = ageEnd;
    //        int inflationYears = ageRentStart - ageCurrent;

    //        NeedsMinimum_PerMonth = Inflation.Calc(inflationYears, needsCurrentAgeMinimal_perMonth, inflationRate);
    //        NeedsMinimum_PerMonth -= assumedRent_perMonth;
    //        NeedsComfort_PerMonth = Inflation.Calc(inflationYears, needsCurrentAgeComfort_perMonth, inflationRate);
    //        NeedsComfort_PerMonth -= assumedRent_perMonth;

    //        InterestRate_Cash = interestRate_Cash;
    //        InterestRate_Stocks_GoodCase = interestRate_Stocks_GoodCase;
    //        InterestRate_Stocks_BadCase = interestRate_Stocks_BadCase;
    //        CrashFactor_Stocks_BadCase = crashFactor_Stocks_BadCase;

    //        TaxFactor_Stocks = taxFactor_Stocks;
    //    }

    //    public decimal NeedsMinimum_PerMonth { get; private set; }
    //    public decimal NeedsMinimum_PerYear => NeedsMinimum_PerMonth * 12;
    //    public decimal NeedsMinimum_PerPhase => NeedsMinimum_PerYear * DurationInYears;
    //    public decimal NeedsComfort_PerMonth { get; private set; }
    //    public decimal NeedsComfort_PerYear => NeedsComfort_PerMonth * 12;
    //    public decimal NeedsComfort_PerPhase => NeedsComfort_PerYear * DurationInYears;

    //    public int AgeRentStart { get; private set; }
    //    public int AgeEnd { get; private set; }
    //    public int DurationInYears { get; private set; }

    //    public decimal InterestRate_Cash { get; private set; }
    //    public decimal InterestRate_Stocks_GoodCase { get; private set; }
    //    public decimal InterestRate_Stocks_BadCase { get; private set; }
    //    public decimal CrashFactor_Stocks_BadCase { get; private set; }
    //    public decimal TaxFactor_Stocks { get; private set; }
    //}
}