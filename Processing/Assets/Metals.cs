using Processing.Withdrawal;
using System;
using System.Linq;

namespace Processing.Assets
{
    public class Metals : Asset
    {
        public Metals(Input _input, Portfolio portfolio) : base(_input, portfolio)
        {
            //todo: can this be moved to base class?
            switch (input.interestRateType)
            {
                case InterestRateType.Relativ:
                    growthRatePerMonth = RentSimMath.InterestPerYearToInterestPerMonthRelative(input.metalsGrowthRate);
                    break;
                case InterestRateType.Konform:
                    growthRatePerMonth = RentSimMath.InterestPerYearToInterestPerMonth(input.metalsGrowthRate);
                    break;
                default:
                    throw new Exception($"Unsupported Interest Rate Type: <{input.interestRateType}>.");
            }
            growthRatePerYear = input.metalsGrowthRate;

            this.Protocol.Last().yearBegin = _input.metals;
            this.Protocol.Last().yearEnd = _input.metals;
        }

        public Metals Buy(double amount)
        {
            return (Metals)base.ApplyInvests(amount);
        }

        public Metals Sell(double amount)
        {
            return (Metals)base.ApplyInvests(-amount);
        }

        public Metals ApplyWorthIncrease(double growthRate)
        {
            return (Metals)base.ApplyGrowth(growthRate);
        }

        //public override void Process()
        //{
        //    for (int i = input.ageCurrent; i < input.ageStopWork; i++)
        //    {
        //        for (int month = 1; month <= 12; month++)
        //        {
        //            this
        //               .Buy(input.metalsMonthlyInvestAmount)
        //               .ApplyWorthIncrease(this.growthRatePerMonth);
        //        }

        //        base.MoveToNextYear();
        //    }
        //}

        public override void Process2(AssetWithdrawalRateInfo withdrawalRateInfo)
        {
            double withdrawalAmount = withdrawalRateInfo.RateStopWorkGross;
            for (int i = input.ageStopWork; i < input.ageRentStart; i++)
            {
                this
                    .Sell(withdrawalAmount)
                    .ApplyWorthIncrease(this.growthRatePerYear);

                base.MoveToNextYear();
            }

            withdrawalAmount = withdrawalRateInfo.RateRentStartGross;
            for (int i = input.ageRentStart; i < input.ageEnd; i++)
            {
                this
                    .Sell(withdrawalAmount)
                    .ApplyWorthIncrease(this.growthRatePerYear);

                base.MoveToNextYear();
            }
        }
    }
}
