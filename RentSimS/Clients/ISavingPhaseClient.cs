using SavingPhaseService.Contracts;

namespace RentSimS.Clients
{
    public interface ISavingPhaseClient
    {
        public Task<decimal> GetSavingPhaseResultAsync(int ageFrom, int ageTo, decimal startCapital, int growthRate, decimal saveAmountPerMonth);
        public Task<SimulationResult> GetSavingPhaseSimulationAsync(int ageFrom, int ageTo, decimal startCapital, int growthRate, decimal saveAmountPerMonth);
    }
}
