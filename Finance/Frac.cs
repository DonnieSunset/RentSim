namespace Finance
{
    /// <summary>
    /// Class representing fraction of values, 
    /// i.e. taxes and interests.
    /// </summary>
    public class Frac
    {
        private decimal _rate;

        private Frac(decimal rate)
        {
            _rate = rate;
        }

        /// <summary>
        /// For testability
        /// </summary>
        internal decimal Rate => _rate;

        /// <summary>
        /// Factor means 1.x 
        /// </summary>
        public static Frac FromFactor(decimal factor)
        {
            if (factor < 1 || factor > 2)
                throw new ArgumentException($"Factor must be betwen 1 (=0%) and 2 (=100%), but was {factor}.");

            return new Frac(factor-1);
        }

        /// <summary>
        /// Rate means 0.x
        /// </summary>
        public static Frac FromRate(decimal rate)
        {
            if (rate < 0 || rate > 1)
                throw new ArgumentException($"Rate must be betwen 0 (=0%) and 1 (=100%), but was {rate}.");

            return new Frac(rate);
        }

        /// <summary>
        /// Returns taxes for a given amount, e.g.
        /// 25% of 120 => returns 30
        /// </summary>
        public decimal ForAmount(decimal amount)
        {
            return amount * _rate;
        }

        /// <summary>
        /// Spits amount and taxes from a given total, e.g.
        /// you have a balance of 100, how much can you withdraw and how much are the taxes
        /// so that it sums up to 100.
        /// </summary>
        public (decimal amount, decimal taxes) FromTotal(decimal total)
        {
            var reverseFactor = 1 / (_rate + 1);
            
            var amount = total * reverseFactor;
            var taxes = total - amount;

            return (amount, taxes);
        }
    }
}
