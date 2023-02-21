using StopWorkPhaseService.DTOs;

namespace StopWorkPhaseService.Clients
{
    public interface IFinanceMathClient
    {
        public Task<StopWorkPhaseServiceResultDTO> RateByNumericalSparkassenformel(decimal betrag_cash, decimal betrag_stocks, decimal betrag_metals, decimal zins_cash, decimal zins_stocks, decimal zins_metals, decimal endbetrag, int yearStart, int yearEnd);
    }
}
