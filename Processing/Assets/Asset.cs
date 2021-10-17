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
        protected double growthRatePerMonth;
        protected double growthRatePerYear;
        protected Input input;

        private List<ProtocolEntry> protocol = new List<ProtocolEntry>();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// We need to ensure that protocol entries can only be written in the Asset class.
        /// </remarks>
        public ReadOnlyCollection<ProtocolEntry> Protocol
        {
            get { return protocol.AsReadOnly(); }
        }

        protected Asset(Input _input, Portfolio portfolio)
        {
            input = _input;
            BasePortfolio = portfolio;

            protocol.Add(new ProtocolEntry {
                age = input.ageCurrent,
            });
        }

        public Portfolio BasePortfolio
        {
            get;
            private set;
        }

        protected Asset ApplyInvests(double invest)
        {
            protocol.Last().invests += invest;
            protocol.Last().yearEnd += invest;

            return this;
        }

        protected Asset ApplyGrowth(double growthRate)
        {
            var current = protocol.Last();
            double thisMonthGrowth = current.yearEnd * (growthRate / 100d);
            current.growth += thisMonthGrowth;
            current.yearEnd += thisMonthGrowth;

            return this;
        }

        public Asset MoveToNextYear()
        {
            protocol.Add(new ProtocolEntry
            {
                age = protocol.Last().age + 1,
                yearBegin = protocol.Last().yearEnd,
                yearEnd = protocol.Last().yearEnd
            });

            return this;
        }

        public abstract void Process();

        public abstract void Process2();
    }
}
