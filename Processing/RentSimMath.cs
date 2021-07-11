using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing
{
    public static class RentSimMath
    {
        public static double InterestPerYearToInterestPerMonth(double interestPerYear)
        {
            //https://www.haushaltsfinanzen.de/finanzmathematik/konformer_zinssatz.php?Konformer-Zinssatz-mit-online-Rechner
            double interestPerMonth = Math.Pow(1f + (interestPerYear / 100f), 1f / 12f) - 1;

            return interestPerMonth * 100;
        }
    }
}
