namespace Domain
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

        /// <summary>
        /// Creates a new <see cref="CAmount"/> object with the total amount of <paramref name="amount"/>
        /// and the distribution of <paramref name="other"/>
        /// </summary>
        public static CAmount From(decimal amount, CAmount other)
        {
            return new CAmount()
            {
                FromDeposits = amount * (other.FromDeposits / other.Total),
                FromInterests = amount * (other.FromInterests / other.Total)
            };
        }
    }
}
