namespace Domain
{
    public class ResultRow : IComparable<ResultRow>
    {
        public class AssetInfo
        {
            public decimal YearBegin { get; set; } 
            public decimal Deposits { get; set; }
            public decimal Interests { get; set; }
            public decimal Taxes { get; set; }
            public decimal YearEnd 
            {
                get
                {
                    return YearBegin + Deposits + Interests + Taxes;
                }
            }
        }

        public int Age { get; set; }

        public AssetInfo cash = new AssetInfo();
        public AssetInfo stocks = new AssetInfo();
        public AssetInfo metals = new AssetInfo();
        
        public decimal TotalYearBegin => stocks.YearBegin + cash.YearBegin + metals.YearBegin;
        public decimal TotalDeposits => stocks.Deposits + cash.Deposits + metals.Deposits;
        public decimal TotalInterests => stocks.Interests + cash.Interests + metals.Interests;
        public decimal TotalTaxes => stocks.Taxes + cash.Taxes + metals.Taxes;
        public decimal TotalYearEnd => stocks.YearEnd + cash.YearEnd + metals.YearEnd;

        public int CompareTo(ResultRow? other)
        {
            if (other == null) return 1;

            if (this.Age < other.Age)
                return -1;
            else if (this.Age > other.Age)
                return 1;
            else 
                return 0;
        }
    }
}
