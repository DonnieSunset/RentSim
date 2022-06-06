namespace Finance
{
    public struct BlaResult
    {
        public decimal rate_Cash;
        public decimal rate_Stocks_Min;
        public decimal rate_Stocks_Max;
        public decimal total_Cash;
        public decimal total_Stocks;
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

        public static BlaResult BlaCalculate(
            decimal InterestRate_Stocks_GoodCase,
            decimal InterestRate_Stocks_BadCase,
            decimal InterestRate_Cash,
            int DurationInYears,
            decimal comfort_total_needed_Year,
            decimal minimum_total_needed_Year,
            decimal crashFactor_Stocks_BadCase)
        {
            var result = new BlaResult();

            decimal InterestFactor_Stocks_GoodCase = InterestRate_Stocks_GoodCase + 1;
            decimal InterestFactor_Stocks_BadCase = InterestRate_Stocks_BadCase + 1;
            decimal InterestFactor_Cash = InterestRate_Cash + 1;

            var z_stocks_max = (-(FinanceCalculator.Pow(InterestFactor_Stocks_GoodCase, DurationInYears) - 1) / ((InterestFactor_Stocks_GoodCase - 1) * (FinanceCalculator.Pow(InterestFactor_Stocks_GoodCase, DurationInYears))));
            var z_stocks_min = (-(FinanceCalculator.Pow(InterestFactor_Stocks_BadCase, DurationInYears) - 1) / ((InterestFactor_Stocks_BadCase - 1) * (FinanceCalculator.Pow(InterestFactor_Stocks_BadCase, DurationInYears))));
            var z_cash = (-(FinanceCalculator.Pow(InterestFactor_Cash, DurationInYears) - 1) / ((InterestFactor_Cash - 1) * (FinanceCalculator.Pow(InterestFactor_Cash, DurationInYears))));

            //Console.WriteLine($"SSHH: z_stocks_min {z_stocks_min:F2} z_stocks_max {z_stocks_max:F2}");
            //Console.WriteLine($"SSHH: z_cash {z_cash:F2}");

            

            result.rate_Cash = (comfort_total_needed_Year * crashFactor_Stocks_BadCase * z_stocks_max - minimum_total_needed_Year * z_stocks_min) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min);
            result.rate_Stocks_Max = z_stocks_min * (-comfort_total_needed_Year + minimum_total_needed_Year) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min);
            result.rate_Stocks_Min = crashFactor_Stocks_BadCase * z_stocks_max * (-comfort_total_needed_Year + minimum_total_needed_Year) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min);
            result.total_Cash = z_cash * (comfort_total_needed_Year * crashFactor_Stocks_BadCase * z_stocks_max - minimum_total_needed_Year * z_stocks_min) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min);
            result.total_Stocks = z_stocks_max * z_stocks_min * (-comfort_total_needed_Year + minimum_total_needed_Year) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min);

            Console.WriteLine($"==========================================================");
            Console.WriteLine($"Rate_Cash: {result.rate_Cash:F2}");
            Console.WriteLine($"Rate_Stocks_Max: {result.rate_Stocks_Max:F2}");
            Console.WriteLine($"Rate_Stocks_Min: {result.rate_Stocks_Min:F2}");
            Console.WriteLine($"Total_Cash: {result.total_Cash:F2}");
            Console.WriteLine($"Total_Stocks: {result.total_Stocks:F2}");
            Console.WriteLine($"==========================================================");

            return result;
        }
    }
}
