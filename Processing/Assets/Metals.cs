using Processing.Withdrawal;
using System;
using System.Linq;

namespace Processing.Assets
{
    public class Metals : Asset
    {
        public Metals(Input _input, Portfolio portfolio) : base(_input, portfolio)
        {
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

            this.protocol.Last().yearBegin = _input.metals;
            this.protocol.Last().yearEnd = _input.metals;
        }

        public Metals Buy(double amount)
        {
            return (Metals)base.ApplyInvests(amount);
        }

        public Metals Sell(double amount)
        {
            return (Metals)base.ApplyInvests(-amount);
        }

        public Metals GetWorthIncrease(double amount)
        {
            return (Metals)base.ApplyGrowth(amount);
        }

        public override void Process()
        {
            for (int i = input.ageCurrent; i < input.ageStopWork; i++)
            {
                for (int month = 1; month <= 12; month++)
                {
                    this
                       .Buy(input.metalsMonthlyInvestAmount)
                       .GetWorthIncrease(this.growthRatePerMonth);
                }

                base.MoveToNextYear();
            }
        }

        public override void Process2()
        {
            double withdrawalAmount = BasePortfolio.WithdrawalStrategy.GetWithdrawalAmount(input.ageStopWork, this.GetType());

            for (int i = input.ageStopWork; i < input.ageRentStart; i++)
            {
                this
                    .Sell(withdrawalAmount);

                base.MoveToNextYear();
            }
        }
    }
}
