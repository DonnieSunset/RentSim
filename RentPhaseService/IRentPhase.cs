using RentPhaseService.Clients;

namespace RentPhaseService
{

    public interface IRentPhase
    {
        public decimal ApproxStateRent(int ageCurrent, decimal netRentAgeCurrent, int ageRentStart, decimal netRentAgeRentStart, int ageInQuestion);

        public Task<string> Simulate(
            int ageStart,
            int ageEnd,
            decimal totalRateNeeded_perYear,
            decimal capitalCash, double growthRateCash,
            decimal capitalStocks, double growthRateStocks,
            decimal capitalMetals, double growthRateMetals,
            IFinanceMathClient financeMathClient);
    }
}
