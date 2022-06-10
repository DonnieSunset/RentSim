namespace Finance
{
    public struct BlaResult
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
    }

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

        public static decimal Sparkassenformel(decimal K0, decimal q, int n)
        {
            //decimal K0 = R * -(Pow(q,n) - 1) / ((q - 1) * (Pow(q,n)));

            decimal R = K0 / (-(Pow(q, n) - 1) / ((q - 1) * (Pow(q, n))));

            return R;

        }

        public static decimal SparkassenformelMitSteuern(decimal K0, decimal q, int n, decimal s)
        {
            decimal R = K0 * Pow(q, n) * (q - 1) / (s * (Pow(q, n) - 1));

            return R;
    }

        public static BlaResult BlaCalculate(
            decimal InterestRate_Stocks_GoodCase,
            decimal InterestRate_Stocks_BadCase,
            decimal InterestRate_Cash,
            int DurationInYears,
            decimal comfort_total_needed_Year,
            decimal minimum_total_needed_Year,
            decimal crashFactor_Stocks_BadCase,
            decimal stocks_taxFactor)
        {
            var result = new BlaResult();
            //stocks_taxFactor = 1.26m;

            decimal InterestFactor_Stocks_GoodCase = InterestRate_Stocks_GoodCase + 1;
            decimal InterestFactor_Stocks_BadCase = InterestRate_Stocks_BadCase + 1;
            decimal InterestFactor_Cash = InterestRate_Cash + 1;

            var z_stocks_max =  (-(FinanceCalculator.Pow(InterestFactor_Stocks_GoodCase, DurationInYears) - 1) / ((InterestFactor_Stocks_GoodCase - 1) * (FinanceCalculator.Pow(InterestFactor_Stocks_GoodCase, DurationInYears)))) / InterestFactor_Stocks_GoodCase;
            var z_stocks_min =  (-(FinanceCalculator.Pow(InterestFactor_Stocks_BadCase, DurationInYears) - 1) / ((InterestFactor_Stocks_BadCase - 1) * (FinanceCalculator.Pow(InterestFactor_Stocks_BadCase, DurationInYears)))) / InterestFactor_Stocks_BadCase;
            var z_cash = (-(FinanceCalculator.Pow(InterestFactor_Cash, DurationInYears) - 1) / ((InterestFactor_Cash - 1) * (FinanceCalculator.Pow(InterestFactor_Cash, DurationInYears)))) / InterestFactor_Cash;
          
            // Calculate the results without taxes
            result.rate_Cash = (comfort_total_needed_Year * crashFactor_Stocks_BadCase * z_stocks_max - minimum_total_needed_Year * z_stocks_min) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min);
            result.rateStocks_IncludedTaxes_GoodCase = z_stocks_min * (-comfort_total_needed_Year + minimum_total_needed_Year) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min) / stocks_taxFactor;
            result.rateStocks_IncludedTaxes_BadCase = crashFactor_Stocks_BadCase * z_stocks_max * (-comfort_total_needed_Year + minimum_total_needed_Year) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min) / stocks_taxFactor;
            result.total_Cash = z_cash * (comfort_total_needed_Year * crashFactor_Stocks_BadCase * z_stocks_max - minimum_total_needed_Year * z_stocks_min) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min);
            result.total_Stocks = z_stocks_max * z_stocks_min * (-comfort_total_needed_Year + minimum_total_needed_Year) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min);

            // I have no idea why the total numbers are negative in the first place
            result.total_Cash = -result.total_Cash;
            result.total_Stocks = -result.total_Stocks;

            // Based on the results, we re-calculate the stocks part with the Sparkassenformel, but considering also taxes
            // The difference between the stocks rates of both calculations is the tax rate
            decimal neededStocks_GoodCase = comfort_total_needed_Year - result.total_Stocks;
            decimal neededStocks_BadCase = minimum_total_needed_Year - result.total_Stocks;

            //result.rateStocks_IncludedTaxes_GoodCase = - Sparkassenformel(result.total_Stocks, (InterestRate_Stocks_GoodCase * (decimal)stocks_taxFactor) + 1, DurationInYears);
            //result.rateStocks_IncludedTaxes_BadCase = - Sparkassenformel(result.total_Stocks * crashFactor_Stocks_BadCase, (InterestRate_Stocks_BadCase * (decimal)stocks_taxFactor) + 1, DurationInYears);

            //result.taxesPerYear_GoodCase = result.rateStocks_IncludedTaxes_GoodCase - result.rateStocks_ExcludedTaxes_GoodCase;
            //result.taxesPerYear_BadCase = result.rateStocks_IncludedTaxes_BadCase - result.rateStocks_ExcludedTaxes_BadCase;


            Console.WriteLine($"==========================================================");
            Console.WriteLine($"Rate_Cash:                         {result.rate_Cash:F2}");
            Console.WriteLine($"rateStocks_ExcludedTaxes_GoodCase: {result.rateStocks_ExcludedTaxes_GoodCase:F2}");
            Console.WriteLine($"rateStocks_ExcludedTaxes_BadCase:  {result.rateStocks_ExcludedTaxes_BadCase:F2}");
            Console.WriteLine($"rateStocks_IncludedTaxes_GoodCase: {result.rateStocks_IncludedTaxes_GoodCase:F2}");
            Console.WriteLine($"rateStocks_IncludedTaxes_BadCase:  {result.rateStocks_IncludedTaxes_BadCase:F2}");
            Console.WriteLine($"Total_Cash:                        {result.total_Cash:F2}");
            Console.WriteLine($"Total_Stocks:                      {result.total_Stocks:F2}");
            //Console.WriteLine($"rateStocks_IncludedTaxes_GoodCase: {result.rateStocks_IncludedTaxes_GoodCase:F2}");
            //Console.WriteLine($"rateStocks_IncludedTaxes_BadCase:  {result.rateStocks_IncludedTaxes_BadCase:F2}");
            Console.WriteLine($"Taxes_GoodCase:                    {result.taxesPerYear_GoodCase:F2}");
            Console.WriteLine($"Taxes_BadCase:                     {result.taxesPerYear_BadCase:F2}");
            Console.WriteLine($"==========================================================");

            return result;
        }






        public static BlaResult BlaCalculate2(
            decimal InterestRate_Stocks_GoodCase,
            decimal InterestRate_Stocks_BadCase,
            decimal InterestRate_Cash,
            int DurationInYears,
            decimal comfort_total_needed_Year,
            decimal minimum_total_needed_Year,
            decimal crashFactor_Stocks_BadCase,
            decimal stocks_taxFactor)
        {
            var result = new BlaResult();
            
            //stocks_taxFactor = 1 / stocks_taxFactor;

            decimal InterestFactor_Stocks_GoodCase = InterestRate_Stocks_GoodCase + 1;
            decimal InterestFactor_Stocks_BadCase = InterestRate_Stocks_BadCase + 1;
            decimal InterestFactor_Cash = InterestRate_Cash + 1;

            var z_cash =       Pow(InterestFactor_Cash, -DurationInYears) * ((Pow(InterestFactor_Cash, DurationInYears) - 1) / (InterestFactor_Cash - 1));  
            var z_stocks_max = Pow(InterestFactor_Stocks_GoodCase, -DurationInYears) * ((Pow(InterestFactor_Stocks_GoodCase, DurationInYears) - 1) / (InterestFactor_Stocks_GoodCase - 1))      * stocks_taxFactor;
            var z_stocks_min = Pow(InterestFactor_Stocks_BadCase, -DurationInYears) * ((Pow(InterestFactor_Stocks_BadCase, DurationInYears) - 1) / (InterestFactor_Stocks_BadCase - 1))         * stocks_taxFactor;

            // Calculate the results without taxes
            result.rate_Cash = (comfort_total_needed_Year * crashFactor_Stocks_BadCase * z_stocks_max - minimum_total_needed_Year * z_stocks_min) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min);
            result.rateStocks_IncludedTaxes_GoodCase = z_stocks_min * (-comfort_total_needed_Year + minimum_total_needed_Year) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min)                             * stocks_taxFactor;
            result.rateStocks_IncludedTaxes_BadCase = crashFactor_Stocks_BadCase * z_stocks_max * (-comfort_total_needed_Year + minimum_total_needed_Year) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min) * stocks_taxFactor;
            result.total_Cash = z_cash * (comfort_total_needed_Year * crashFactor_Stocks_BadCase * z_stocks_max - minimum_total_needed_Year * z_stocks_min) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min);
            result.total_Stocks = z_stocks_max * z_stocks_min * (-comfort_total_needed_Year + minimum_total_needed_Year) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min);

            // I have no idea why the total numbers are negative in the first place
            //result.total_Cash = -result.total_Cash;
            //result.total_Stocks = -result.total_Stocks;

            // Based on the results, we re-calculate the stocks part with the Sparkassenformel, but considering also taxes
            // The difference between the stocks rates of both calculations is the tax rate
            //decimal neededStocks_GoodCase = comfort_total_needed_Year - result.total_Stocks;
            //decimal neededStocks_BadCase = minimum_total_needed_Year - result.total_Stocks;

            //result.rateStocks_IncludedTaxes_GoodCase = -Sparkassenformel(result.total_Stocks, (InterestRate_Stocks_GoodCase * (decimal)stocks_taxFactor) + 1, DurationInYears);
            //result.rateStocks_IncludedTaxes_BadCase = -Sparkassenformel(result.total_Stocks * crashFactor_Stocks_BadCase, (InterestRate_Stocks_BadCase * (decimal)stocks_taxFactor) + 1, DurationInYears);

            //result.taxesPerYear_GoodCase = result.rateStocks_IncludedTaxes_GoodCase - result.rateStocks_ExcludedTaxes_GoodCase;
            //result.taxesPerYear_BadCase = result.rateStocks_IncludedTaxes_BadCase - result.rateStocks_ExcludedTaxes_BadCase;

            //result.taxesPerYear_GoodCase = - result.rateStocks_IncludedTaxes_GoodCase * (1/stocks_taxFactor - 1);
            //result.taxesPerYear_BadCase = - result.rateStocks_IncludedTaxes_BadCase * (1/stocks_taxFactor - 1);

            result.taxesPerYear_GoodCase = result.rateStocks_IncludedTaxes_GoodCase * (1- 1/stocks_taxFactor );
            result.taxesPerYear_BadCase = result.rateStocks_IncludedTaxes_BadCase * (1- 1/stocks_taxFactor);


            result.rateStocks_ExcludedTaxes_GoodCase = result.rateStocks_IncludedTaxes_GoodCase - result.taxesPerYear_GoodCase;
            result.rateStocks_ExcludedTaxes_BadCase = result.rateStocks_IncludedTaxes_BadCase - result.taxesPerYear_BadCase;

            Console.WriteLine($"==========================================================");
            Console.WriteLine($"Rate_Cash:                         {result.rate_Cash:F2}");
            Console.WriteLine($"rateStocks_ExcludedTaxes_GoodCase: {result.rateStocks_ExcludedTaxes_GoodCase:F2}");
            Console.WriteLine($"rateStocks_ExcludedTaxes_BadCase:  {result.rateStocks_ExcludedTaxes_BadCase:F2}");
            Console.WriteLine($"rateStocks_IncludedTaxes_GoodCase: {result.rateStocks_IncludedTaxes_GoodCase:F2}");
            Console.WriteLine($"rateStocks_IncludedTaxes_BadCase:  {result.rateStocks_IncludedTaxes_BadCase:F2}");
            Console.WriteLine($"Total_Cash:                        {result.total_Cash:F2}");
            Console.WriteLine($"Total_Stocks:                      {result.total_Stocks:F2}");
            Console.WriteLine($"Taxes_GoodCase:                    {result.taxesPerYear_GoodCase:F2}");
            Console.WriteLine($"Taxes_BadCase:                     {result.taxesPerYear_BadCase:F2}");
            Console.WriteLine($"==========================================================");

            return result;
        }
    }
}
