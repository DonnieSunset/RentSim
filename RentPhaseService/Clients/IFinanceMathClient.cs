using RentPhaseService.DTOs;

namespace RentPhaseService.Clients
{
    public interface IFinanceMathClient
    {
        public Task<decimal> GetAmountWithInflationAsync(int ageStart, int ageEnd, decimal amount, double inflationRate);

        public Task<RentPhaseServiceResultDTO> StartCapitalByNumericalSparkassenformel(RentPhaseServiceInputDTO input);
    }
}
