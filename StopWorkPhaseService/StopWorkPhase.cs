using StopWorkPhaseService.Clients;
using StopWorkPhaseService.DTOs;

namespace StopWorkPhaseService
{
    public class StopWorkPhase : IStopWorkPhase
    {
        public async Task<StopWorkPhaseServiceResultDTO> Simulate(StopWorkPhaseServiceInputDTO input, IFinanceMathClient financeMathClient)
        {
            var stopWorkPhaseResultString = await financeMathClient.RateByNumericalSparkassenformel(input);

            return stopWorkPhaseResultString;
        }
    }
}
