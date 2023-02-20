using Protocol;
using RentSimS.DTOs;

namespace RentSimS.Clients
{
    public interface ISavingPhaseClient
    {
        public Task<SavingPhaseServiceResultDTO> GetSavingPhaseSimulationAsync(SavingPhaseServiceInputDTO input);
        public void LogSavingPhaseResult(SavingPhaseServiceResultDTO result, IProtocolWriter protocolWriter);
    }
}
