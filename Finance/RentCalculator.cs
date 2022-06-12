using Finance.Results;

namespace Finance
{
    public class RentCalculator
    {
        public static StateRentResult ApproxStateRent(int ageCurrent, decimal netRentAgeCurrent, int ageRentStart, decimal netRentAgeRentStart, int ageInQuestion)
        {
            var result = new StateRentResult();

            if (ageInQuestion <= ageCurrent || ageInQuestion >= ageRentStart)
            {
                throw new InvalidDataException($"Param: {nameof(ageInQuestion)} is {ageInQuestion} but should be between {ageCurrent} and {ageRentStart}.");
            }

            result.assumedStateRent_FromStopWorkAge_PerMonth = (netRentAgeRentStart - netRentAgeCurrent) / (ageRentStart - ageCurrent) * (ageInQuestion - ageCurrent) + netRentAgeCurrent;
            return result;
        }

        public static LaterNeedsResult CalculateLaterNeeds(int ageCurrent, int ageRentStart, double inflationRate, decimal needsCurrentAgeMinimal, decimal needsCurrentAgeComfort, decimal assumedStateRent_FromStopWorkAge_PerMonth)
        {
            var result = new LaterNeedsResult();

            var myRentStartInflation = new Inflation(ageCurrent, ageRentStart, inflationRate);

            result.needsMinimum_AgeRentStart_WithInflation_PerMonth = myRentStartInflation.Calc(needsCurrentAgeMinimal) - assumedStateRent_FromStopWorkAge_PerMonth;
            result.needsComfort_AgeRentStart_WithInflation_PerMonth = myRentStartInflation.Calc(needsCurrentAgeComfort) - assumedStateRent_FromStopWorkAge_PerMonth;

            result.needsMinimum_AgeRentStart_WithInflation_PerYear = result.needsMinimum_AgeRentStart_WithInflation_PerMonth * 12;
            result.needsComfort_AgeRentStart_WithInflation_PerYear = result.needsComfort_AgeRentStart_WithInflation_PerMonth * 12;

            return result;
        }
    }
}
