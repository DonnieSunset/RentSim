using System.Collections.Generic;
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
        protected Input input;

        public List<ProtocolEntry> protocol = new List<ProtocolEntry>();

        protected Asset(Input _input)
        {
            input = _input;

            protocol.Add(new ProtocolEntry {
                age = input.ageCurrent,
            });
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
