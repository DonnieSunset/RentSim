using Domain;

namespace RentSimS.DTOs
{
    public class SavingPhaseServiceResultDTO
    {
        public CAmount FinalSavingsCash { get; set; }
        public CAmount FinalSavingsStocks { get; set; }
        public CAmount FinalSavingsMetals { get; set; }
        public decimal FinalSavings { get { return FinalSavingsCash.Total + FinalSavingsStocks.Total + FinalSavingsMetals.Total; } }

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
        public ResultDTO Result { get; set; } = new ResultDTO();

        public SavingPhaseServiceResultDTO()
        {
            Entities = new List<Entity>();
            FirstYearBeginValues = new AssetsDTO();

            FinalSavingsCash = new CAmount();
            FinalSavingsStocks = new CAmount();
            FinalSavingsMetals = new CAmount();
        }
    }
}
