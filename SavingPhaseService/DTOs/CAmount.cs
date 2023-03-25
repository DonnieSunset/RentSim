namespace SavingPhaseService.DTOs
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
            // Total changes during the first op,
            // thats why it cannot be re-used in the second op and we have to bufferit
            var totalBuffered = Total;

            FromDeposits += (FromDeposits / totalBuffered) * amount;
            FromInterests += (FromInterests / totalBuffered) * amount;
        }
    }
}
