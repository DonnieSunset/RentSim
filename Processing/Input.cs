using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing
{
    public enum InterestRateType
    {
        Konform,
        Relativ
    }

    public class Input
    {
        public int ageCurrent;
        public int ageStopWork;
        public int ageRentStart;
        public int ageEnd;

        public InterestRateType interestRateType = InterestRateType.Konform;

        public int stocks;
        public int stocksGrowthRate;
        public int stocksMonthlyInvestAmount;

        public int cash;
        public int cashGrowthRate;
        public int cashMonthlyInvestAmount;

        public int metals;
        public int metalsGrowthRate;
        public int metalsMonthlyInvestAmount;
    }
}
