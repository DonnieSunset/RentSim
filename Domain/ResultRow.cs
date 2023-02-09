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
            public decimal YearEnd { get; set; }
        }

        public int Age { get; set; }

        public AssetInfo cash = new AssetInfo();
        public AssetInfo stocks = new AssetInfo();
        public AssetInfo metals = new AssetInfo();

        //public decimal cashYearBegin;
        //public List<decimal> cashDeposits = new();
        //public decimal cashInterests;
        //public decimal cashTaxes;
        //public decimal cashYearEnd;

        //public decimal stocksYearBegin;
        //public List<decimal> stocksDeposits = new();
        //public decimal stocksInterests;
        //public decimal stocksTaxes;
        //public decimal stocksYearEnd;

        //public decimal metalsYearBegin;
        //public List<decimal> metalsDeposits = new();
        //public decimal metalsInterests;
        //public decimal metalsTaxes;
        //public decimal metalsYearEnd;

        
        public decimal TotalYearBegin => stocks.YearBegin + cash.YearBegin + metals.YearBegin;
        public decimal TotalDeposits => stocks.Deposits + cash.Deposits + metals.Deposits;
        public decimal TotalInterests => stocks.Interests + cash.Interests + metals.Interests;
        public decimal TotalTaxes => stocks.Taxes + cash.Taxes + metals.Taxes;
        public decimal TotalYearEnd => stocks.YearEnd + cash.YearEnd + metals.YearEnd;

        //public decimal CashDepositsTotal => cash.Deposits.Sum();
        //public decimal StocksDepositsTotal => stocks.Deposits.Sum();
        //public decimal MetalsDepositsTotal => metals.Deposits.Sum();

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
