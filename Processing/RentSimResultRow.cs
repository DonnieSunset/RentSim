namespace Processing
{
    public class Asset
    {
        public double yearBegin;
        public double invests;
        public double growth;
        public double yearEnd;

        public Asset ApplyInvests(double invest)
        {
            this.invests += invest;
            this.yearEnd += invest;
            return this;
        }

        public Asset ApplyGrowth(double growthRate)
        {
            double thisMonthGrowth = yearEnd * (growthRate / 100d);
            this.growth += thisMonthGrowth;
            this.yearEnd += thisMonthGrowth;
            return this;
        }
    }



    public class RentSimResultRow
    {
        public int age;

        public Asset cash = new Asset();
        public Asset stocks = new Asset();
        public Asset metals = new Asset();
        
        public Asset total = new Asset();

        public RentSimResultRow(Input _input)
        {
            stocks.yearBegin = stocks.yearEnd = _input.stocks;
            cash.yearBegin = cash.yearEnd = _input.cash;
            metals.yearBegin = metals.yearEnd = _input.metals;
        }

        // For testability
        internal RentSimResultRow()
        {
            stocks.yearEnd = stocks.yearBegin;
            cash.yearEnd = cash.yearBegin;
            metals.yearEnd = metals.yearBegin;
        }
    }
}
