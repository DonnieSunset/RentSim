using Protocol;
using RentSimS.DTOs;

namespace RentSimS.Clients
{
    public interface IStopWorkPhaseClient
    {
        public Task<StopWorkPhaseServiceResultDTO> GetStopWorkPhaseSimulationAsync(StopWorkPhaseServiceInputDTO input);

        public void LogStopWorkPhaseResult(StopWorkPhaseServiceResultDTO result, IProtocolWriter protocol);
    }
}
