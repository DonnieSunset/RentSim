namespace StopWorkPhaseService.DTOs
{
    public class StopWorkPhaseServiceInputDTO
    {
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public CAmount StartCapitalCash { get; set; }
        public CAmount StartCapitalStocks { get; set; }
        public CAmount StartCapitalMetals { get; set; }
        public decimal GrowthRateCash { get; set; }
        public decimal GrowthRateStocks { get; set; }
        public decimal GrowthRateMetals { get; set; }

        public decimal EndCapitalTotal { get; set; }
    }
}
