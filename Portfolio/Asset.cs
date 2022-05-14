namespace Portfolio
{
    public abstract class Asset
    {
        //protected double growthRatePerMonth;
        protected double growthRatePerYear;
        
        public double CurrentAmount
        {
            get; protected set;
        }

        public double CurrentGrowth
        {
            get; protected set;
        }

        //protected Input input;

        public double GrowthRatePerYear
        {
            get => growthRatePerYear;
        }

        //protected Asset(Input _input, AssetPortfolio portfolio)
        //{
        //    input = _input;
        //    BasePortfolio = portfolio;

        //    //protocol.Add(new ProtocolEntry {
        //    //    age = input.ageCurrent,
        //    //});
        //}

        public AssetPortfolio BasePortfolio
        {
            get;
            private set;
        }

        protected Asset ApplyInvests(double invest)
        {
            //protocol.Last().invests += invest;
            //protocol.Last().yearEnd += invest;
            this.CurrentAmount += invest;

            return this;
        }

        /// <summary>
        /// Applies growth rate to asset, typically in terms of interests for cash
        /// or growth rate in terms of stocks / metals.
        /// </summary>
        /// <param name="growthRate">The growth rate in % (e.g. 3 corresponds to 3% groths)</param>
        /// <returns></returns>
        protected Asset ApplyYearlyGrowth(double growthRate)
        {
            //var current = protocol.Last();
            //double thisMonthGrowth = current.yearEnd * (growthRate / 100d);
            double growthFactor = 1 + (growthRate / 100);
            double growth = this.CurrentAmount * growthFactor;

            this.CurrentAmount += growth;
            this.CurrentGrowth += growth;

            return this;
        }

        //protected double GetAllGrowth()
        //{
        //    return this.CurrentGrowth;
        //}

        protected double GetFractionOfGrowthAccordingToAmount(double amount)
        {
            throw new NotImplementedException();
        }

        //public Asset MoveToNextYear()
        //{
        //    protocol.Add(new ProtocolEntry
        //    {
        //        age = protocol.Last().age + 1,
        //        yearBegin = protocol.Last().yearEnd,
        //        yearEnd = protocol.Last().yearEnd
        //    });

        //    return this;
        //}

        //public abstract void Process();

        //public abstract void Process2(AssetWithdrawalRateInfo withdrawalRateInfo);
    }
}
