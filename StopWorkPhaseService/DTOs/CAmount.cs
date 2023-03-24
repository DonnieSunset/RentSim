namespace StopWorkPhaseService.DTOs
{
    public class CAmount
    {
        public decimal FromDeposits { get; set; }
        public decimal FromInterests { get; set; }

        public decimal Total => FromDeposits + FromInterests;

        public CAmount() { }

        public CAmount(CAmount other) 
        { 
            this.FromDeposits = other.FromDeposits;
            this.FromInterests = other.FromInterests;
        }

        public void DistributeEqually(decimal amount)
        {
            decimal depositsPart = (FromDeposits / Total) * amount;
            decimal interestsPart = (FromInterests / Total) * amount;

            FromDeposits += depositsPart;
            FromInterests += interestsPart;
        }
    }
}
