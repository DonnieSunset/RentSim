using Microsoft.AspNetCore.Mvc;
using RentSimS.DTOs;

namespace RentSimS.Clients
{
    public interface IFinanceMathClient
    {
        public Task<decimal> GetAmountWithInflationAsync(int ageStart, int ageEnd, decimal amount, double inflationRate);

        public Task<SimulationResultDTO> RateByNumericalSparkassenformel(decimal betrag_cash, decimal betrag_stocks, decimal betrag_metals, decimal zins_cash, decimal zins_stocks, decimal zins_metals, decimal endbetrag, int yearStart, int yearEnd);

        public Task<SimulationResultDTO> StartCapitalByNumericalSparkassenformel(decimal rateTotal_perYear, double factor1, double zins1, double factor2, double zins2, double factor3, double zins3, decimal endbetrag, int yearStart, int yearEnd);

    }
}
