using Protocol;
using PhaseIntegratorService.DTOs;

namespace PhaseIntegratorService.Clients
{
    public interface IStopWorkPhaseClient
    {
        public Task<StopWorkPhaseServiceResultDTO> GetStopWorkPhaseSimulationAsync(StopWorkPhaseServiceInputDTO input);

        public void LogStopWorkPhaseResult(StopWorkPhaseServiceResultDTO result, IProtocolWriter protocol);
    }
}
