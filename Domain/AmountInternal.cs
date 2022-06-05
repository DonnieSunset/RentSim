namespace Domain
{
    public class AmountInternal
    {
        private decimal myAmount = -1;
        private int myDurationDuration = -1;

        internal AmountInternal(decimal amountPerMonth, int durationInYears)
        {
            myAmount = amountPerMonth;
            myDurationDuration = durationInYears;
        }

        public decimal PerMonth => myAmount;
        public decimal PerYear => myAmount * 12;
        public decimal Total => myAmount * 12 * myDurationDuration;
    }
}
