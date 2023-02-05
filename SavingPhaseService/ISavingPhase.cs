using SavingPhaseService.Clients;
using SavingPhaseService.Contracts;

namespace SavingPhaseService
{
    public interface ISavingPhase
    {
        public Task<decimal> Calculate(
           int ageFrom,
           int ageTo,
           decimal startCapital,
           int growthRate,
           decimal saveAmountPerMonth,
           bool capitalYieldsTax,
           IFinanceMathClient financeMathClient
           );

        public SavingPhaseServiceResult Simulate(
            int ageFrom,
            int ageTo,
            decimal startCapital,
            decimal growthRate,
            decimal saveAmountPerMonth,
            bool capitalYieldsTax
            );
    }
}
