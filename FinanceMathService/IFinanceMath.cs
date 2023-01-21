namespace FinanceMathService
{
    public interface IFinanceMath
    {
        public double NonRiskAssetsNeededInCaseOfRiskAssetCrash(double totalAmount, double stocksCrashFactor, double totalAmount_minNeededAfterCrash);
        public double RateByNumericalSparkassenformel(List<double> betrag, List<double> zins, double endbetrag, int jahre);
        public decimal SparkassenFormel(decimal anfangskapital, decimal rate_proJahr, double zinsFaktor, int anzahlJahre);

    }
}
