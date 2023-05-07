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

        public AssetInfo Cash { get; set; } = new AssetInfo();
        public AssetInfo Stocks { get; set; } = new AssetInfo();
        public AssetInfo Metals { get; set; } = new AssetInfo();
        
        public decimal TotalYearBegin => Stocks.YearBegin + Cash.YearBegin + Metals.YearBegin;
        public decimal TotalDeposits => Stocks.Deposits + Cash.Deposits + Metals.Deposits;
        public decimal TotalInterests => Stocks.Interests + Cash.Interests + Metals.Interests;
        public decimal TotalTaxes => Stocks.Taxes + Cash.Taxes + Metals.Taxes;
        public decimal TotalYearEnd => Stocks.YearEnd + Cash.YearEnd + Metals.YearEnd;

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
