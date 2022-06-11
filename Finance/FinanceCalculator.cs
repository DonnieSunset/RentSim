namespace Finance
{
    public class FinanceCalculator
    {
        public static (decimal partOfAmount1, decimal partOfAmount2) WithdrawUniformFromTwoAmounts(decimal amount1, decimal amount2, decimal withdrawalAmount)
        {
            var fractionAmount1 = amount1 / (amount1 + amount2);
            var fractionAmount2 = amount2 / (amount1 + amount2);

            var deviation = Math.Abs(fractionAmount1 + fractionAmount2) - 1;

            if (deviation > (decimal)0.01)
            {
                throw new Exception($"Something went wrong here: Deviation <{deviation}> should be 0.");
            }

            var withdrawalAmount1 = fractionAmount1 * withdrawalAmount;
            var withdrawalAmount2 = fractionAmount2 * withdrawalAmount;

            return (withdrawalAmount1, withdrawalAmount2);
        }

        public static decimal Pow(decimal a, int b)
        {
            return (decimal)Math.Pow((double)a, b);
        }

        private static decimal GetZFactorForSparkassenformel(int durationInYears, decimal interestFactor)
        {
            if (interestFactor < 1 || interestFactor > 2)
            {
                throw new Exception($"Parameter {nameof(interestFactor)}: {interestFactor} must be between 1 und 2.");
            }

            if (interestFactor == 1)
            {
                return durationInYears;
            }

            return Pow(interestFactor, -durationInYears) * ((Pow(interestFactor, durationInYears) - 1) / (interestFactor - 1));
        }

        public static RentPhaseResult CalculateRentPhaseResult(
            decimal interestRate_Stocks_GoodCase,
            decimal interestRate_Stocks_BadCase,
            decimal InterestRate_Cash,
            int durationInYears,
            decimal comfort_total_needed_Year,
            decimal minimum_total_needed_Year,
            decimal crashFactor_Stocks_BadCase,
            decimal stocks_taxFactor)
        {
            var result = new RentPhaseResult();
            
            decimal interestFactor_Stocks_GoodCase = interestRate_Stocks_GoodCase + 1;
            decimal interestFactor_Stocks_BadCase = interestRate_Stocks_BadCase + 1;
            decimal interestFactor_Cash = InterestRate_Cash + 1;

            var z_cash = GetZFactorForSparkassenformel(durationInYears, interestFactor_Cash);
            var z_stocks_max = GetZFactorForSparkassenformel(durationInYears, interestFactor_Stocks_GoodCase) * stocks_taxFactor;
            var z_stocks_min = GetZFactorForSparkassenformel(durationInYears, interestFactor_Stocks_BadCase) * stocks_taxFactor;

            // Calculate the results
            result.rate_Cash = (comfort_total_needed_Year * crashFactor_Stocks_BadCase * z_stocks_max - minimum_total_needed_Year * z_stocks_min) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min);
            result.rateStocks_IncludedTaxes_GoodCase = z_stocks_min * (-comfort_total_needed_Year + minimum_total_needed_Year) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min)                             * stocks_taxFactor;
            result.rateStocks_IncludedTaxes_BadCase = crashFactor_Stocks_BadCase * z_stocks_max * (-comfort_total_needed_Year + minimum_total_needed_Year) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min) * stocks_taxFactor;
            result.total_Cash = z_cash * (comfort_total_needed_Year * crashFactor_Stocks_BadCase * z_stocks_max - minimum_total_needed_Year * z_stocks_min) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min);
            result.total_Stocks = z_stocks_max * z_stocks_min * (-comfort_total_needed_Year + minimum_total_needed_Year) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min);

            result.taxesPerYear_GoodCase = result.rateStocks_IncludedTaxes_GoodCase * (1- 1/stocks_taxFactor );
            result.taxesPerYear_BadCase = result.rateStocks_IncludedTaxes_BadCase * (1- 1/stocks_taxFactor);

            result.rateStocks_ExcludedTaxes_GoodCase = result.rateStocks_IncludedTaxes_GoodCase - result.taxesPerYear_GoodCase;
            result.rateStocks_ExcludedTaxes_BadCase = result.rateStocks_IncludedTaxes_BadCase - result.taxesPerYear_BadCase;

            return result;
        }
    }
}
