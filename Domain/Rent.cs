namespace Domain
{
    public class Rent
    {
        public AmountInternal FromCurrentAge { get; private set; }
        public AmountInternal FromRentStartAge { get; private set; }

        public Rent(decimal netRentPerMonthFromCurrentAge, decimal netRentPerMonthFromRentStartAge, int rentDurationInYears)
        { 
            FromCurrentAge = new AmountInternal(netRentPerMonthFromCurrentAge, rentDurationInYears);
            FromRentStartAge = new AmountInternal(netRentPerMonthFromRentStartAge, rentDurationInYears);
        }
    }
}
