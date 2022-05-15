using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance
{
    public class RentCalculator
    {
        public static decimal ApproxNetRent(int ageCurrent, decimal netRentAgeCurrent, int ageRentStart, decimal netRentAgeRentStart, int ageInQuestion)
        {
            if (ageInQuestion <= ageCurrent || ageInQuestion >= netRentAgeRentStart)
            {
                throw new InvalidDataException($"Param: {nameof(ageInQuestion)}");
            }

            var result = (netRentAgeRentStart - netRentAgeCurrent) / (ageRentStart - ageCurrent) * (ageInQuestion - ageCurrent) + netRentAgeCurrent;
            return result;
        }
    }
}
