namespace RentPhaseService.DTOs
{
    public class SimulationResultDTO
    {
        public record AssetsDTO
        {
            public decimal Cash { get; init; }
            public decimal Stocks { get; init; }
            public decimal Metals { get; init; }
        }

        public record Entity
        { 
            public int Age { get; init; }
            public AssetsDTO YearBegin { get; init; }
            public AssetsDTO Rates { get; init; }
            public AssetsDTO Zins { get; init; }
        }

        public List<Entity> Entities { get; init; }

        public SimulationResultDTO()
        {
            Entities = new List<Entity>();
        }
    }
}
