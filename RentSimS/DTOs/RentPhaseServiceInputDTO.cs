namespace RentSimS.DTOs
{
    public class RentPhaseServiceInputDTO
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
    }
}
