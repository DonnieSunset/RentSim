namespace Domain
{
    public class ResultRow : IComparable<ResultRow>
    {
        public int age;
        
        public decimal cashYearBegin;
        public List<decimal> cashDeposits = new();
        public decimal cashInterests;
        public decimal cashTaxes;
        public decimal cashYearEnd;

        public decimal stocksYearBegin;
        public List<decimal> stocksDeposits = new();
        public decimal stocksInterests;
        public decimal stocksTaxes;
        public decimal stocksYearEnd;

        public decimal metalsYearBegin;
        public List<decimal> metalsDeposits = new();
        public decimal metalsInterests;
        public decimal metalsTaxes;
        public decimal metalsYearEnd;

        
        public decimal TotalYearBegin => stocksYearBegin + cashYearBegin + metalsYearBegin;
        public decimal TotalDeposits => stocksDeposits.Sum() + cashDeposits.Sum() + metalsDeposits.Sum();
        public decimal TotalInterests => stocksInterests + cashInterests + metalsInterests;
        public decimal TotalTaxes => stocksTaxes + cashTaxes + metalsTaxes;
        public decimal TotalYearEnd => stocksYearEnd + cashYearEnd + metalsYearEnd;

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
