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

        public void CalculateTotal()
        {
            total.yearBegin = stocks.yearBegin + cash.yearBegin + metals.yearBegin;
            total.invests = stocks.invests + cash.invests + metals.invests;
            total.growth = stocks.growth + cash.growth + metals.growth;
            total.yearEnd = stocks.yearEnd + cash.yearEnd + metals.yearEnd;
        }

        public RentSimResultRow CreateFollowUpRow()
        { 
            return new  RentSimResultRow()
            {
                stocks = new Asset() { yearBegin = this.stocks.yearEnd, yearEnd = this.stocks.yearEnd },
                cash = new Asset() { yearBegin = this.cash.yearEnd, yearEnd = this.cash.yearEnd },
                metals = new Asset() { yearBegin = this.metals.yearEnd, yearEnd = this.metals.yearEnd },
            };
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
