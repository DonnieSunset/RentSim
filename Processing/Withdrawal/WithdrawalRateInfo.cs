using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing.Withdrawal
{
    public class AssetWithdrawalRateInfo
    {
        public double RateStopWorkNet { get; set; }
        public double RateStopWorkGross { get; set; }
        public double RateRentStartNet { get; set; }
        public double RateRentStartGross { get; set; }
    }

    public class WithdrawalRateInfo
    {
        public WithdrawalRateInfo()
        {
            Cash = new AssetWithdrawalRateInfo();
            Stocks = new AssetWithdrawalRateInfo();
            Metals = new AssetWithdrawalRateInfo();
            Total = new AssetWithdrawalRateInfo();
        }

        public AssetWithdrawalRateInfo Cash { get; set; }
        public AssetWithdrawalRateInfo Stocks { get; set; }
        public AssetWithdrawalRateInfo Metals { get; set; }
        public AssetWithdrawalRateInfo Total { get; set; }
    }
}
