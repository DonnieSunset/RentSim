namespace SavingPhaseService.Clients
{
    public interface IFinanceMathClient
    {
        public Task<decimal> GetSparkassenFormelAsync(decimal anfangskapital, decimal rateProJahr, double zinsFaktor, int anzahlJahre);
    }
}
