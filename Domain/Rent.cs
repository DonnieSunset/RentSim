namespace Domain
{
    public class Rent
    {
        public class RentInternal 
        { 
            private decimal myRent = -1;
            private int myRentDuration = -1;

            internal RentInternal(decimal rent, int rentDurationInYears)
            {
                myRent = rent;
                myRentDuration = rentDurationInYears;
            }

            public decimal PerMonth => myRent;
            public decimal PerYear => myRent * 12;
            public decimal Total => myRent * 12 * myRentDuration;
        }

        public RentInternal FromCurrentAge { get; private set; }
        public RentInternal FromRentStartAge { get; private set; }

        public Rent(decimal netRentPerMonthFromCurrentAge, decimal netRentPerMonthFromRentStartAge, int rentDurationInYears)
        { 
            FromCurrentAge = new RentInternal(netRentPerMonthFromCurrentAge, rentDurationInYears);
            FromRentStartAge = new RentInternal(netRentPerMonthFromRentStartAge, rentDurationInYears);
        }
    }
}
