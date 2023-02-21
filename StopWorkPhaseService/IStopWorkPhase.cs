using StopWorkPhaseService.Clients;
using StopWorkPhaseService.DTOs;

namespace StopWorkPhaseService
{
    public interface IStopWorkPhase
    {
        public Task<StopWorkPhaseServiceResultDTO> Simulate(StopWorkPhaseServiceInputDTO input, IFinanceMathClient financeMathClient);
    }
}
