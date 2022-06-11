namespace Finance
{
    public static class Inflation
    {
        public static decimal Calc(int startAge, int endAge, decimal amount, double inflationRate)
        {
            return Calc(endAge - startAge, amount, inflationRate);
        }


        public static decimal Calc(int numYears, decimal amount, double inflationRate)
        {
            double inflationFactor = inflationRate + 1;

            if (inflationFactor < 1 || inflationFactor >= 2)
            {
                throw new ArgumentException($"{nameof(inflationFactor)}: {inflationFactor}");
            }

            double finalInflationFactor = Math.Pow(inflationFactor, numYears);
            return amount * (decimal)finalInflationFactor;
        }
    }
}