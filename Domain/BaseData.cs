using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public abstract class BaseData
    {
        public decimal myNeedsNowMinimum = 1900;
        public decimal myNeedsNowComfort = 2600;

        public abstract AmountInternal NeedsMinimum { get; set; }
        public abstract AmountInternal NeedsComfort { get; set; }
    }
}
