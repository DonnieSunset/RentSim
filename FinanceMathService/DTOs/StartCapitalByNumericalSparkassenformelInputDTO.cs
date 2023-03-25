namespace FinanceMathService.DTOs
{
    public class StartCapitalByNumericalSparkassenformelInputDTO
    {
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public CAmount StartCapitalCash { get; set; }
        public CAmount StartCapitalStocks { get; set; }
        public CAmount StartCapitalMetals { get; set; }
        public decimal GrowthRateCash { get; set; }
        public decimal GrowthRateStocks { get; set; }
        public decimal GrowthRateMetals { get; set; }

        public decimal TotalRateNeeded_PerYear { get; set; }

        public void Validate()
        {
            if (Math.Abs(FractionCash + FractionStocks + FractionMetals - 1m) > 0.0001m)
            {
                throw new ArgumentException("Fractions dont sum up to 1");
            }

            if (TotalRateNeeded_PerYear > 0)
            {
                throw new ArgumentException($"Total Rate Needed must be negative, but is {TotalRateNeeded_PerYear}.");
            }
        }

        public decimal FractionCash => StartCapitalCash.Total / (StartCapitalCash.Total + StartCapitalStocks.Total + StartCapitalMetals.Total);
        public decimal FractionStocks => StartCapitalStocks.Total / (StartCapitalCash.Total + StartCapitalStocks.Total + StartCapitalMetals.Total);
        public decimal FractionMetals => StartCapitalMetals.Total / (StartCapitalCash.Total + StartCapitalStocks.Total + StartCapitalMetals.Total);
    }
}
