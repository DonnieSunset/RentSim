namespace Finance
{
    public class InterestCalculator
    {
        public static decimal GetInterestsFor(decimal amount, int interestPercent)
        {
            double interestFactor = interestPercent / 100d;
            var result = (decimal)interestFactor * amount;

            return result;
        }
    }
}
