using Microsoft.AspNetCore.Mvc;

namespace RentSimS.Clients
{
    public interface IFinanceMathClient
    {
        public Task<decimal> GetAmountWithInflationAsync(int ageStart, int ageEnd, decimal amount, double inflationRate);

        public Task<decimal> StartCapitalByNumericalSparkassenformel(decimal rateTotal_perYear, double factor1, double zins1, double factor2, double zins2, double factor3, double zins3, decimal endbetrag, int yearStart, int yearEnd);

    }
}
