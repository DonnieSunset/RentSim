namespace RentPhaseService.DTOs
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
            FromDeposits += (FromDeposits / Total) * amount;
            FromInterests += (FromInterests / Total) * amount;
        }
    }
}
