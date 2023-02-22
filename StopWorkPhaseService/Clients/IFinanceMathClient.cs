using StopWorkPhaseService.DTOs;

namespace StopWorkPhaseService.Clients
{
    public interface IFinanceMathClient
    {
        public Task<StopWorkPhaseServiceResultDTO> RateByNumericalSparkassenformel(StopWorkPhaseServiceInputDTO input);
    }
}
