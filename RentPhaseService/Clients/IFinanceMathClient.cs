using RentPhaseService.DTOs;

namespace RentPhaseService.Clients
{
    public interface IFinanceMathClient
    {
        public Task<decimal> GetAmountWithInflationAsync(int ageStart, int ageEnd, decimal amount, double inflationRate);

        public Task<RentPhaseServiceResultDTO> RateByNumericalSparkassenformel(decimal betrag_cash, decimal betrag_stocks, decimal betrag_metals, decimal zins_cash, decimal zins_stocks, decimal zins_metals, decimal endbetrag, int yearStart, int yearEnd);

        public Task<RentPhaseServiceResultDTO> StartCapitalByNumericalSparkassenformel(decimal rateTotal_perYear, decimal betrag_cash, decimal zins_cash, decimal betrag_stocks, decimal zins_stocks, decimal betrag_metals, decimal zins_metals, decimal endbetrag, int yearStart, int yearEnd);

    }
}
