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
           IFinanceMathClient financeMathClient);

        public SimulationResult Simulate(
            int ageFrom,
            int ageTo,
            decimal startCapital,
            int growthRate,
            decimal saveAmountPerMonth);
    }
}
