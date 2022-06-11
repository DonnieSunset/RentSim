namespace Finance
{
    public class Inflation
    {
        private int myStartAge;
        private int myEndAge;
        private double myInflationRate;

        public Inflation(int startAge, int endAge, double inflationRate)
        {
            this.myStartAge = startAge;
            this.myEndAge = endAge;
            this.myInflationRate = inflationRate;
        }

        public decimal Calc(decimal amount)
        {
            return Inflation.Calc(this.myStartAge, this.myEndAge, amount, this.myInflationRate);
        }

        public static decimal Calc(int startAge, int endAge, decimal amount, double inflationRate)
        {
            return Calc(endAge - startAge, amount, inflationRate);
        }


        public static decimal Calc(int numYears, decimal amount, double inflationRate)
        {
            double inflationFactor = inflationRate + 1;

            if (inflationFactor < 1 || inflationFactor > 2)
            {
                throw new ArgumentException($"{nameof(inflationFactor)}: {inflationFactor}");
            }

            double finalInflationFactor = Math.Pow(inflationFactor, numYears);
            return amount * (decimal)finalInflationFactor;
        }
    }
}