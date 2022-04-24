using Processing.Withdrawal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Processing.Assets
{
    public class ProtocolEntry
    {
        public int age;
        public double yearBegin;
        public double invests;
        public double growth;

        /// <summary>
        /// The percentage 
        /// </summary>
        public double withdrawalRate;

        public double yearEnd;
    }

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

        protected Input input;

        //private List<ProtocolEntry> protocol = new List<ProtocolEntry>();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// We need to ensure that protocol entries can only be written in the Asset class.
        /// </remarks>
        //public ReadOnlyCollection<ProtocolEntry> Protocol
        //{
        //    get { return protocol.AsReadOnly(); }
        //}

        /// <summary>
        /// For testing purposes only.
        /// </summary>
        //internal List<ProtocolEntry> ProtocolInternal
        //{
        //    get { return protocol; }
        //}


        public double GrowthRatePerYear
        {
            get => growthRatePerYear;
        }

        protected Asset(Input _input, Portfolio portfolio)
        {
            input = _input;
            BasePortfolio = portfolio;

            //protocol.Add(new ProtocolEntry {
            //    age = input.ageCurrent,
            //});
        }

        public Portfolio BasePortfolio
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
