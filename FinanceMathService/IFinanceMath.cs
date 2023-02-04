using FinanceMathService.DTOs;

namespace FinanceMathService
{
    public interface IFinanceMath
    {
        public double NonRiskAssetsNeededInCaseOfRiskAssetCrash(double totalAmount, double stocksCrashFactor, double totalAmount_minNeededAfterCrash);

        public decimal RateByNumericalSparkassenformel(
            decimal betrag_cash,
            decimal betrag_stocks,
            decimal betrag_metals,
            decimal zins_cash,
            decimal zins_stocks,
            decimal zins_metals,
            decimal endbetrag,
            int yearStart, int yearEnd,
            out SimulationResultDTO protocol);

        public decimal StartCapitalByNumericalSparkassenformel(
            decimal rateTotal_perYear,
            double factor_cash,
            double factor_stocks,
            double factor_metals,
            double zinsRate_cash,
            double zinsRate_stocks,
            double zinsRate_metals,
            decimal endbetrag,
            int yearStart, int yearEnd,
            out SimulationResultDTO protocol);

        public decimal SparkassenFormel(decimal anfangskapital, decimal rate_proJahr, double zinsFaktor, int anzahlJahre);
        public decimal AmountWithInflation(int ageStart, int ageEnd, decimal amount, double inflationRate);

    }
}
