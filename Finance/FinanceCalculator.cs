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

        public static decimal Log(decimal x)
        {
            return (decimal)Math.Log((double)x);
        }


        public static decimal SparkassenFormel(decimal anfangskapital, decimal rate_proJahr, double zinsFaktor, int anzahlJahre)
        {
            decimal zinsFaktor_d = (decimal)zinsFaktor;

            decimal endKapital;
            if (zinsFaktor_d == 1)
            {
                endKapital = anfangskapital + rate_proJahr * anzahlJahre;
            }
            else
            {
                //endKapital = anfangskapital * Pow(zinsFaktor_d, anzahlJahre) + (rate_proJahr * zinsFaktor_d * ((Pow(zinsFaktor_d, anzahlJahre)) - 1) / (zinsFaktor_d - 1));
                endKapital = anfangskapital * Pow(zinsFaktor_d, anzahlJahre) + (rate_proJahr * 1 * ((Pow(zinsFaktor_d, anzahlJahre)) - 1) / (zinsFaktor_d - 1));
            }
            
            return endKapital;
        }

        public static decimal GetZFactorForSparkassenformel(int durationInYears, decimal interestFactor)
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
    }
}
