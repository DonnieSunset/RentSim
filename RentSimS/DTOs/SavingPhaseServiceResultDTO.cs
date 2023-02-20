namespace RentSimS.DTOs
{
    public class SavingPhaseServiceResultDTO
    {
        public decimal FinalSavingsCash { get; set; }
        public decimal FinalSavingsStocks { get; set; }
        public decimal FinalSavingsMetals { get; set; }
        public decimal FinalSavings { get { return FinalSavingsCash + FinalSavingsStocks + FinalSavingsMetals; } }

        public record AssetsDTO
        {
            public decimal Cash { get; init; }
            public decimal Stocks { get; init; }
            public decimal Metals { get; init; }
        }

        public record Entity
        {
            public int Age { get; init; }
            public AssetsDTO Deposits { get; init; }
            public AssetsDTO Interests { get; init; }
            public AssetsDTO Taxes { get; init; }
        }

        public List<Entity> Entities { get; init; }
        public AssetsDTO FirstYearBeginValues { get; set; }

        public SavingPhaseServiceResultDTO()
        {
            Entities = new List<Entity>();
            FirstYearBeginValues = new AssetsDTO();
        }
    }
}
