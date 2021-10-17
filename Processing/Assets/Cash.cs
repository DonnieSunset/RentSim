using Processing.Withdrawal;
using System;
using System.Linq;

namespace Processing.Assets
{
    public class Cash : Asset
    {
        public Cash(Input _input, Portfolio portfolio) : base(_input, portfolio)
        {
            switch (input.interestRateType)
            {
                case InterestRateType.Relativ:
                    growthRatePerMonth = RentSimMath.InterestPerYearToInterestPerMonthRelative(input.cashGrowthRate);
                    break;
                case InterestRateType.Konform:
                    growthRatePerMonth = RentSimMath.InterestPerYearToInterestPerMonth(input.cashGrowthRate);
                    break;
                default:
                    throw new Exception($"Unsupported Interest Rate Type: <{input.interestRateType}>.");
            }
            growthRatePerYear = input.cashGrowthRate;

            this.Protocol.Last().yearBegin = _input.cash;
            this.Protocol.Last().yearEnd = _input.cash;
        }

        public Cash Save(double amount)
        {
            return (Cash)base.ApplyInvests(amount);
        }

        public Cash Withdraw(double amount)
        {
            return (Cash)base.ApplyInvests(-amount);
        }

        public Cash ApplyInterests(double interestRate)
        {
            return (Cash)base.ApplyGrowth(interestRate);
        }

        public void PayTaxesForInterests()
        {
            throw new NotImplementedException();
        }

        public override void Process()
        {
            for (int i = input.ageCurrent; i < input.ageStopWork; i++)
            {
                for (int month = 1; month <= 12; month++)
                {
                    this
                       .Save(input.cashMonthlyInvestAmount)
                       .ApplyInterests(this.growthRatePerMonth);
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
                    .Withdraw(withdrawalAmount)
                    .ApplyInterests(this.growthRatePerYear);

                base.MoveToNextYear();
            }
        }
    }
}
