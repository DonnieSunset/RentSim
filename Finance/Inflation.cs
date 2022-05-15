namespace Finance
{
    public static class Inflation
    {
        public static decimal Calc(int startAge, int endAge, decimal amount, double inflationRateInPercent)
        {
            return Calc(endAge - startAge, amount, inflationRateInPercent);
        }


        public static decimal Calc(int numYears, decimal amount, double inflationRateInPercent)
        {
            double inflationFactor = (inflationRateInPercent / 100) + 1;
            double finalInflationFactor = Math.Pow(inflationFactor, numYears);
            return amount * (decimal)finalInflationFactor;
        }
    }
}