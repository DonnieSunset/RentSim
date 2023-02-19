using Protocol;
using RentSimS.DTOs;

namespace RentSimS.Clients
{
    public interface ISavingPhaseClient
    {
        public Task<SavingPhaseServiceResult> GetSavingPhaseSimulationAsync(SavingPhaseServiceInputDTO input);

        public Task<SavingPhaseResult> GetAndLogSavingPhase(SavingPhaseServiceInputDTO input, IProtocolWriter protocolWriter);
    }
}
