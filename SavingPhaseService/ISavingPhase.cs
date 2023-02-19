using SavingPhaseService.DTOs;

namespace SavingPhaseService
{
    public interface ISavingPhase
    {
        public SavingPhaseServiceResultDTO Simulate(SavingPhaseServiceInputDTO input);
    }
}
