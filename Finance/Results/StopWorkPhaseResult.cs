namespace Finance.Results
{
    public class StopWorkPhaseResult
    {
        public int ageStopWork;

        public decimal rate_Cash;
        //public decimal rateStocks_IncludedTaxes_BadCase;
        public decimal rateStocks_IncludedTaxes;
        //public decimal rateStocks_ExcludedTaxes_BadCase;
        public decimal rateStocks_ExcludedTaxes;
        public decimal neededPhaseBegin_Cash;
        public decimal neededPhaseBegin_Stocks;
        public decimal taxesPerYear; //todo: why do we need this? this should be calculated in the simulation phase.
        //public decimal taxesPerYear_BadCase;

        public decimal NeededPhaseBegin_Total => neededPhaseBegin_Cash + neededPhaseBegin_Stocks;

        public void Print()
        {
            string result = $"===============--- {nameof(StopWorkPhaseResult)} ---===================="             + Environment.NewLine +
                            $"{nameof(ageStopWork)}:                        {ageStopWork}"                          + Environment.NewLine +
                            $"{nameof(rate_Cash)}:                          {rate_Cash:F2}"                         + Environment.NewLine +
                            $"{nameof(rateStocks_ExcludedTaxes)}:           {rateStocks_ExcludedTaxes:F2}" + Environment.NewLine +
                            //$"{nameof(rateStocks_ExcludedTaxes_BadCase)}:   {rateStocks_ExcludedTaxes_BadCase:F2}"  + Environment.NewLine +
                            $"{nameof(rateStocks_IncludedTaxes)}:           {rateStocks_IncludedTaxes:F2}" + Environment.NewLine +
                            //$"{nameof(rateStocks_IncludedTaxes_BadCase)}:   {rateStocks_IncludedTaxes_BadCase:F2}"  + Environment.NewLine +
                            $"{nameof(neededPhaseBegin_Cash)}:              {neededPhaseBegin_Cash:F2}"             + Environment.NewLine +
                            $"{nameof(neededPhaseBegin_Stocks)}:            {neededPhaseBegin_Stocks:F2}"           + Environment.NewLine +
                            $"{nameof(NeededPhaseBegin_Total)}:             {NeededPhaseBegin_Total:F2}"            + Environment.NewLine +
                            $"{nameof(taxesPerYear)}:                       {taxesPerYear:F2}"             + Environment.NewLine +
                            //$"{nameof(taxesPerYear_BadCase)}:               {taxesPerYear_BadCase:F2}"              + Environment.NewLine +
                            $"===============================================================";

            Console.WriteLine(result);
        }
    }
}
