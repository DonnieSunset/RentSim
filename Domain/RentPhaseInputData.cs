using Finance;

namespace Domain
{
    public class RentPhaseInputData //: BaseData
    {
        //internal decimal InterestRate_Stocks_GoodCase { get; set; }
        //internal decimal InterestRate_Stocks_BadCase { get; set; }
        //internal decimal CrashFactor_Stocks_BadCase { get; set; }
        //internal decimal InterestRate_Cash { get; set; }

        //internal int AgeCurrent { get; set; }
        //internal int AgeRentStart { get; set; }
        //internal int AgeEnd { get; set; }

        //internal double InflationRate { get; set; }
        //internal decimal NeedsCurrentAgeMinimal { get; set; }
        //internal decimal NeedsCurrentAgeComfort { get; set; }


        public RentPhaseInputData(
            int ageCurrent,
            int ageRentStart,
            int ageEnd,
            double inflationRate,
            decimal needsCurrentAgeMinimal,
            decimal needsCurrentAgeComfort,
            decimal interestRate_Cash,
            decimal interestRate_Stocks_GoodCase,
            decimal interestRate_Stocks_BadCase,
            decimal crashFactor_Stocks_BadCase
            )
        {
            DurationInYears = ageEnd - ageRentStart;

            int inflationYears = ageRentStart - ageCurrent;

            //hier muss man noch die staatliche rente abziehen
            //zwei schritte: 1) inflation anwenden 2) staatliche rente abziehen
            //vllt sollte man den teil auscarven
            var NeedsCurrentAgeMinimalInf = Inflation.Calc(inflationYears, needsCurrentAgeMinimal, inflationRate);
            var NeedsCurrentAgeComfortInf = Inflation.Calc(inflationYears, needsCurrentAgeComfort, inflationRate);

            NeedsMinimum = new AmountInternal(amountPerMonth: NeedsCurrentAgeMinimalInf, durationInYears: DurationInYears);
            NeedsComfort = new AmountInternal(amountPerMonth: NeedsCurrentAgeComfortInf, durationInYears: DurationInYears);

            InterestRate_Cash = interestRate_Cash;
            InterestRate_Stocks_GoodCase = interestRate_Stocks_GoodCase;
            InterestRate_Stocks_BadCase = interestRate_Stocks_BadCase;
            CrashFactor_Stocks_BadCase = crashFactor_Stocks_BadCase;
        }

        // muss ma auch noch inflation berücksichtigen
        public AmountInternal NeedsComfort { get; private set; }
        public AmountInternal NeedsMinimum { get; private set; }

        public int DurationInYears { get; private set; }

        public decimal InterestRate_Cash { get; private set; }
        public decimal InterestRate_Stocks_GoodCase { get; private set; }
        public decimal InterestRate_Stocks_BadCase { get; private set; }
        public decimal CrashFactor_Stocks_BadCase { get; private set; }
    }
}