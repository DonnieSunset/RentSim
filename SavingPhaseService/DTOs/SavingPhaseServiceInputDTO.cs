namespace SavingPhaseService.DTOs
{
    public class SavingPhaseServiceInputDTO
    {
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public CAmount StartCapitalCash { get; set; }
        public CAmount StartCapitalStocks { get; set; }
        public CAmount StartCapitalMetals { get; set; }
        public decimal GrowthRateCash { get; set; }
        public decimal GrowthRateStocks { get; set; }
        public decimal GrowthRateMetals { get; set; }
        public decimal SaveAmountPerMonthCash { get; set; }
        public decimal SaveAmountPerMonthStocks { get; set; }
        public decimal SaveAmountPerMonthMetals { get; set; }

        public SavingPhaseServiceInputDTO()
        {
            StartCapitalCash = new CAmount();
            StartCapitalStocks = new CAmount();
            StartCapitalMetals = new CAmount();
        }
    }
}
