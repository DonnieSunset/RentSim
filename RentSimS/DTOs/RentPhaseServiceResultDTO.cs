namespace RentSimS.DTOs
{
    public class RentPhaseServiceResultDTO
    {
        public decimal MonthlyDepositRate { get; set; }

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

        public RentPhaseServiceResultDTO()
        {
            Entities = new List<Entity>();
            FirstYearBeginValues = new AssetsDTO();
        }
    }
}
