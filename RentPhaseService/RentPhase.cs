using RentPhaseService.Clients;
using RentPhaseService.DTOs;

namespace RentPhaseService
{
    public class RentPhase : IRentPhase
    {
        public decimal ApproxStateRent(int ageCurrent, decimal netRentAgeCurrent, int ageRentStart, decimal netRentAgeRentStart, int ageInQuestion)
        {
            if (ageInQuestion < ageCurrent || ageInQuestion > ageRentStart)
            {
                throw new ArgumentException($"Param: {nameof(ageInQuestion)} is {ageInQuestion} but should be between {ageCurrent} and {ageRentStart}.");
            }

            decimal deltaRent = netRentAgeRentStart - netRentAgeCurrent;
            int deltaAges = ageRentStart - ageCurrent;

            if (deltaAges == 0)
            {
                if (netRentAgeCurrent != netRentAgeRentStart)
                {
                    throw new ArgumentException($"Invalid input: {nameof(ageCurrent)}: {ageCurrent}, " +
                        $"{nameof(ageRentStart)}: {ageRentStart}, " +
                        $"{nameof(netRentAgeCurrent)}: {netRentAgeCurrent}, " +
                        $"{nameof(netRentAgeRentStart)}: {netRentAgeRentStart}.");
                }
                else
                {
                    return netRentAgeCurrent;
                }
            }

            decimal linearRentPerYear = deltaRent / deltaAges;
            decimal numYearsToAgeInQuestion = ageInQuestion - ageCurrent;

            decimal result = linearRentPerYear * numYearsToAgeInQuestion + netRentAgeCurrent;
            return result;
        }

        public async Task<RentPhaseServiceResultDTO> Simulate(RentPhaseServiceInputDTO input, IFinanceMathClient financeMathClient)
        {
            var rentPhaseResultString = await financeMathClient.StartCapitalByNumericalSparkassenformel(
                   input.TotalRateNeeded_PerYear,
                   input.StartCapitalCash,
                   input.GrowthRateCash,
                   input.StartCapitalStocks,
                   input.GrowthRateStocks,
                   input.StartCapitalMetals,
                   input.GrowthRateMetals,
                   0,
                   input.AgeFrom,
                   input.AgeTo);

            return rentPhaseResultString;
        }
    }
}