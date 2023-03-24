namespace FinanceMathService.DTOs
{
    public class StartCapitalByNumericalSparkassenformelInputDTO
    {
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public decimal FractionCash { get; set; }
        public decimal FractionStocks { get; set; }
        public decimal FractionMetals { get; set; }
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
    }
}
