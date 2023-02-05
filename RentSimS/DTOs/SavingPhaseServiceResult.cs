namespace RentSimS.DTOs
{
    public class SavingPhaseServiceResult
    {
        public decimal FinalSavings { get; set; }

        public record Entity
        { 
            public int Age { get; init; }
            public decimal Interests { get; init; }
            public decimal Deposit { get; init; }
            public decimal Taxes { get; init; }
        }

        public List<Entity> Entities { get; init; }

        public SavingPhaseServiceResult()
        {
            Entities = new List<Entity>();
        }
    }
}
