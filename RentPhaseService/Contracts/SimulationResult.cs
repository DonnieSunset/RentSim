namespace RentPhaseService.Contracts
{
    public class SimulationResult
    {
        public record Entity
        { 
            public int Age { get; init; }
            public decimal Interests { get; init; }
            public decimal Deposit { get; init; }
        }

        public List<Entity> Entities { get; init; }

        public SimulationResult()
        {
            Entities = new List<Entity>();
        }
    }
}
