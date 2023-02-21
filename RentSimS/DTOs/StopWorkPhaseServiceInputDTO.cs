namespace RentSimS.DTOs
{
    public class StopWorkPhaseServiceInputDTO
    {
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public decimal StartCapitalCash { get; set; }
        public decimal StartCapitalStocks { get; set; }
        public decimal StartCapitalMetals { get; set; }
        public decimal GrowthRateCash { get; set; }
        public decimal GrowthRateStocks { get; set; }
        public decimal GrowthRateMetals { get; set; }
        public decimal EndCapitalTotal { get; set; }
    }
}
