using Protocol;
using RentSimS.DTOs;

namespace RentSimS.Clients
{
    public interface ISavingPhaseClient
    {
        public Task<SavingPhaseServiceResult> GetSavingPhaseSimulationAsync(int ageFrom, int ageTo, decimal startCapital, int growthRate, decimal saveAmountPerMonth, bool capitalYieldsTax);

        public Task<SavingPhaseResult> GetAndLogSavingPhase(
            int ageFrom, int ageTo,
            decimal cash_startCapital, int cash_growthRate, decimal cash_saveAmountPerMonth,
            decimal stocks_startCapital, int stocks_growthRate, decimal stocks_saveAmountPerMonth,
            decimal metals_startCapital, int metals_growthRate, decimal metals_saveAmountPerMonth,
            IProtocolWriter protocolWriter);
    }
}
