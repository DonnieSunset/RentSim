using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance
{
    public enum InterestRateType
    {
        Relativ,
        Konform
    }

    public class MarketAssumptions
    {
        public double inflationRate = 3.0;
    }
}
