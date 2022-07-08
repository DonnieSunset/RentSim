namespace Finance.Results
{
    public class RentPhaseResult
    {
        public decimal rate_Cash;
        public decimal rateStocks_IncludedTaxes_BadCase;
        public decimal rateStocks_IncludedTaxes_GoodCase;
        public decimal rateStocks_ExcludedTaxes_BadCase;
        public decimal rateStocks_ExcludedTaxes_GoodCase;
        public decimal total_Cash;
        public decimal total_Stocks;
        public decimal taxesPerYear_GoodCase;
        public decimal taxesPerYear_BadCase;

        public void Print()
        {
            string result = $"===============--- {nameof(RentPhaseResult)} ---===================="                 + Environment.NewLine +
                            $"{nameof(rate_Cash)}:                         {rate_Cash:F2}"                          + Environment.NewLine +
                            $"{nameof(rateStocks_ExcludedTaxes_GoodCase)}: {rateStocks_ExcludedTaxes_GoodCase:F2}"  + Environment.NewLine +
                            $"{nameof(rateStocks_ExcludedTaxes_BadCase)}:  {rateStocks_ExcludedTaxes_BadCase:F2}"   + Environment.NewLine +
                            $"{nameof(rateStocks_IncludedTaxes_GoodCase)}: {rateStocks_IncludedTaxes_GoodCase:F2}"  + Environment.NewLine +
                            $"{nameof(rateStocks_IncludedTaxes_BadCase)}:  {rateStocks_IncludedTaxes_BadCase:F2}"   + Environment.NewLine +
                            $"{nameof(total_Cash)}:                        {total_Cash:F2}"                         + Environment.NewLine +
                            $"{nameof(total_Stocks)}:                      {total_Stocks:F2}"                       + Environment.NewLine +
                            $"{nameof(taxesPerYear_GoodCase)}:             {taxesPerYear_GoodCase:F2}"              + Environment.NewLine +
                            $"{nameof(taxesPerYear_BadCase)}:              {taxesPerYear_BadCase:F2}"               + Environment.NewLine +
                            $"==========================================================";

            Console.WriteLine(result);
        }
    }
}
