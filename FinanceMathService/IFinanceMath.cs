using FinanceMathService.DTOs;

namespace FinanceMathService
{
    public interface IFinanceMath
    {
        public double NonRiskAssetsNeededInCaseOfRiskAssetCrash(double totalAmount, double stocksCrashFactor, double totalAmount_minNeededAfterCrash);

        public SimulationResultDTO RateByNumericalSparkassenformel(RateByNumericalSparkassenformelInputDTO input);

        public SimulationResultDTO StartCapitalByNumericalSparkassenformel(StartCapitalByNumericalSparkassenformelInputDTO input);

        public decimal SparkassenFormel(decimal anfangskapital, decimal rate_proJahr, double zinsFaktor, int anzahlJahre);
        public decimal AmountWithInflation(int ageStart, int ageEnd, decimal amount, double inflationRate);

    }
}
