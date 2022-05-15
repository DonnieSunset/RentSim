using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio
{
    public class LifeAssumptions
    {
        public int ageCurrent = 42;
        public int ageStopWork = 60;
        public int ageRentStart = 67;
        public int ageEnd = 80;

        public int stocks = 88800;
        public int stocksGrowthRate = 7;
        public int stocksSaveAmount = 700;

        public int cash = 58000;
        public int cashGrowthRate = 0;
        public int cashSaveAmount = 350;

        public int metals = 21400;
        public int metalsGrowthRate = 1;
        public int metalsSaveAmount = 0;

        public decimal needsNowMinimum = 1900;
        public decimal needsNowComfort = 2600;

        //https://www.finanzrechner.org/sonstige-rechner/rentenbesteuerungsrechner/
        public decimal netStateRentFromCurrentAge = 827;
        public decimal netStateRentFromRentStartAge = 2025;
    }
}
