namespace FinanceMathService
{
    public interface IFinanceMath
    {
        public double NonRiskAssetsNeededInCaseOfRiskAssetCrash(double totalAmount, double stocksCrashFactor, double totalAmount_minNeededAfterCrash);
        public double RateByNumericalSparkassenformel(List<double> betrag, List<double> zins, double endbetrag, int yearStart, int yearEnd);
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
            out List<object> protocol);
        public decimal SparkassenFormel(decimal anfangskapital, decimal rate_proJahr, double zinsFaktor, int anzahlJahre);
        public decimal AmountWithInflation(int ageStart, int ageEnd, decimal amount, double inflationRate);

    }
}
