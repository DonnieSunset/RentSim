namespace Protocol
{
    public class ResultRow : IComparable<ResultRow>
    {
        public int age;
        public decimal stocksYearBegin;
        public decimal stocksDeposits;
        public decimal stocksInterests;
        public decimal stocksTaxes;
        public decimal stocksYearEnd;

        public decimal cashYearBegin;
        public decimal cashDeposits;
        public decimal cashInterests;
        public decimal cashtaxes;
        public decimal cashYearEnd;

        public decimal TotalYearBegin => stocksYearBegin + cashYearBegin;
        public decimal TotalYearEnd => stocksYearEnd + cashYearEnd;

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
