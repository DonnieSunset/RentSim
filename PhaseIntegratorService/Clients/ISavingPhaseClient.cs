using Protocol;
using PhaseIntegratorService.DTOs;

namespace PhaseIntegratorService.Clients
{
    public interface ISavingPhaseClient
    {
        public Task<SavingPhaseServiceResultDTO> GetSavingPhaseSimulationAsync(SavingPhaseServiceInputDTO input);
        public void LogSavingPhaseResult(SavingPhaseServiceResultDTO result, IProtocolWriter protocolWriter);
    }
}
