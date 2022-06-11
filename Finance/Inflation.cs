namespace Finance
{
    public static class Inflation
    {
        public static decimal Calc(int startAge, int endAge, decimal amount, double inflationRateFactor)
        {
            return Calc(endAge - startAge, amount, inflationRateFactor);
        }


        public static decimal Calc(int numYears, decimal amount, double inflationRateFactor)
        {
            if (inflationRateFactor < 1 || inflationRateFactor >= 2)
            {
                throw new ArgumentException($"{nameof(inflationRateFactor)}: {inflationRateFactor}");
            }

            double finalInflationFactor = Math.Pow(inflationRateFactor, numYears);
            return amount * (decimal)finalInflationFactor;
        }
    }
}