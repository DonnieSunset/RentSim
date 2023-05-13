using StopWorkPhaseService.Clients;
using StopWorkPhaseService.DTOs;

namespace StopWorkPhaseService
{
    public class StopWorkPhase : IStopWorkPhase
    {
        public async Task<StopWorkPhaseServiceResultDTO> Simulate(StopWorkPhaseServiceInputDTO input, IFinanceMathClient financeMathClient)
        {
            var stopWorkPhaseResult = await financeMathClient.RateByNumericalSparkassenformel(input);

            return stopWorkPhaseResult;
        }
    }
}
