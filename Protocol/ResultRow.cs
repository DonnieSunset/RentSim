namespace Protocol
{
    public class ResultRow : IComparable<ResultRow>
    {
        public int age;
        
        public decimal cashYearBegin;
        public decimal cashDeposits;
        public decimal cashWithdrawals;
        public decimal cashInterests;
        public decimal cashTaxes;
        public decimal cashYearEnd;

        public decimal stocksYearBegin;
        public decimal stocksDeposits;
        public decimal stocksWithdrawals;
        public decimal stocksInterests;
        public decimal stocksTaxes;
        public decimal stocksYearEnd;

        public decimal metalsYearBegin;
        public decimal metalsDeposits;
        public decimal metalsWithdrawals;
        public decimal metalsInterests;
        public decimal metalsTaxes;
        public decimal metalsYearEnd;

        public decimal TotalYearBegin => stocksYearBegin + cashYearBegin + metalsYearBegin;
        public decimal TotalYearEnd => stocksYearEnd + cashYearEnd + metalsYearBegin;

        public int CompareTo(ResultRow? other)
        {
            if (other == null) return 1;

            if (this.age < other.age)
                return -1;
            else if (this.age > other.age)
                return 1;
            else 
                return 0;
        }
    }
}
