using System;

namespace Processing
{
    public static class RentSimMath
    {
        public static double InterestPerYearToInterestPerMonth(double interestPerYear)
        {
            //https://www.haushaltsfinanzen.de/finanzmathematik/konformer_zinssatz.php?Konformer-Zinssatz-mit-online-Rechner
            double interestPerMonth = Math.Pow(1d + (interestPerYear / 100d), 1d / 12d) - 1;

            return interestPerMonth * 100;
        }

        public static double InterestPerYearToInterestPerMonthRelative(double interestPerYear)
        {
            //https://www.haushaltsfinanzen.de/finanzmathematik/konformer_zinssatz.php?Konformer-Zinssatz-mit-online-Rechner
            double interestPerMonth = interestPerYear / 12d;

            return interestPerMonth;
        }

        public static double RentStopWorkAgeApproximation(int currentAge, int stopWorkAge, int rentStartAge, double currentRent, double rentStartRent)
        {
            if (rentStartAge == currentAge)
            {
                return rentStartRent;
            }

            double rentIncreasePerYear = (rentStartRent - currentRent) / (rentStartAge - currentAge);

            double rentIncreaseUntilStopWorkAge = (stopWorkAge - currentAge) * rentIncreasePerYear;

            double rentStopWorkAge = rentIncreaseUntilStopWorkAge + currentRent;

            return rentStopWorkAge;
        }

        public static double Middle(double left, double right)
        {
            return left + (right - left) / 2;
        }
    }
}
