using SavingPhaseService.Clients;
using SavingPhaseService.DTOs;

namespace SavingPhaseService
{
    public interface ISavingPhase
    {

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
