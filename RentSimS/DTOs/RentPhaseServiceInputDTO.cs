using Domain;

namespace RentSimS.DTOs
{
    public class RentPhaseServiceInputDTO
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

        public decimal FractionCash => StartCapitalCash.Total / (StartCapitalCash.Total + StartCapitalStocks.Total + StartCapitalMetals.Total);
        public decimal FractionStocks => StartCapitalStocks.Total / (StartCapitalCash.Total + StartCapitalStocks.Total + StartCapitalMetals.Total);
        public decimal FractionMetals => StartCapitalMetals.Total / (StartCapitalCash.Total + StartCapitalStocks.Total + StartCapitalMetals.Total);
    }
}
