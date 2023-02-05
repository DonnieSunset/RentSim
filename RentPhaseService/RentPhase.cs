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

        public async Task<SimulationResultDTO> Simulate(
                int ageFrom,
                int ageTo,
                decimal totalRateNeeded_perYear,
                decimal capitalCash, decimal growthRateCash,
                decimal capitalStocks, decimal growthRateStocks,
                decimal capitalMetals, decimal growthRateMetals,
                IFinanceMathClient financeMathClient)
        {
            //decimal capitalTotal = capitalCash + capitalStocks + capitalMetals;
            //double factorCash = (double)(capitalCash / capitalTotal);
            //double factorStocks = (double)(capitalStocks / capitalTotal);
            //double factorMetals = (double)(capitalMetals / capitalTotal);

            var rentPhaseResultString = await financeMathClient.StartCapitalByNumericalSparkassenformel(
                   totalRateNeeded_perYear,
                   capitalCash,
                   growthRateCash,
                   capitalStocks,
                   growthRateStocks,
                   capitalMetals,
                   growthRateMetals,
                   0,
                   ageFrom,
                   ageTo);

            return rentPhaseResultString;
        }
    }
}