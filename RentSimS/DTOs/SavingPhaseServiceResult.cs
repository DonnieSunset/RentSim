namespace RentSimS.DTOs
{
    public class SavingPhaseServiceResult
    {
        public decimal FinalSavingsCash { get; set; }
        public decimal FinalSavingsStocks { get; set; }
        public decimal FinalSavingsMetals { get; set; }
        public decimal FinalSavings { get { return FinalSavingsCash + FinalSavingsStocks + FinalSavingsMetals; } }


        public record AssetInfo
        {
            public int Age { get; init; }
            public decimal InterestsCash { get; init; }
            public decimal InterestsStocks { get; init; }
            public decimal InterestsMetals { get; init; }
            public decimal DepositCash { get; init; }
            public decimal DepositStocks { get; init; }
            public decimal DepositMetals { get; init; }
            public decimal TaxesCash { get; init; }
            public decimal TaxesStocks { get; init; }
            public decimal TaxesMetals { get; init; }
        }

        public List<AssetInfo> Entities { get; init; }

        public SavingPhaseServiceResult()
        {
            Entities = new List<AssetInfo>();
        }
    }
}
