namespace RentSimS.Clients
{
    public interface IFinanceMathClient
    {
        public Task<decimal> GetAmountWithInflationAsync(int ageStart, int ageEnd, decimal amount, double inflationRate);
    }
}
