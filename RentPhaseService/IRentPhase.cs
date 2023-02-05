using RentPhaseService.Clients;
using RentPhaseService.DTOs;

namespace RentPhaseService
{

    public interface IRentPhase
    {
        public decimal ApproxStateRent(int ageCurrent, decimal netRentAgeCurrent, int ageRentStart, decimal netRentAgeRentStart, int ageInQuestion);

        public Task<SimulationResultDTO> Simulate(
            int ageFrom,
            int ageTo,
            decimal totalRateNeeded_perYear,
            decimal capitalCash, decimal growthRateCash,
            decimal capitalStocks, decimal growthRateStocks,
            decimal capitalMetals, decimal growthRateMetals,
            IFinanceMathClient financeMathClient);
    }
}
