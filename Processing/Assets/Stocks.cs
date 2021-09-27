using Processing.Withdrawal;
using System;
using System.Linq;

namespace Processing.Assets
{
    public class Stocks : Asset, IMustPayTaxesAfterWithdrawal
    {
        public double WithdrawalTaxRate { get => 0.26d; }

        public Stocks(Input _input, Portfolio portfolio) : base(_input, portfolio) 
        {
            switch (input.interestRateType)
            {
                case InterestRateType.Relativ:
                    growthRatePerMonth = RentSimMath.InterestPerYearToInterestPerMonthRelative(input.stocksGrowthRate);
                    break;
                case InterestRateType.Konform:
                    growthRatePerMonth = RentSimMath.InterestPerYearToInterestPerMonth(input.stocksGrowthRate);
                    break;
                default:
                    throw new Exception($"Unsupported Interest Rate Type: <{input.interestRateType}>.");
            }

            this.protocol.Last().yearBegin = _input.stocks;
            this.protocol.Last().yearEnd = _input.stocks;
        }

        public Stocks Buy(double amount)
        {
            return (Stocks)base.ApplyInvests(amount);
        }

        public Stocks GetWorthIncrease(double amount)
        {
            return (Stocks)base.ApplyGrowth(amount);
        }

        public void SellAndPayTaxes(double amount)
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
                       .Buy(input.stocksMonthlyInvestAmount)
                       .GetWorthIncrease(this.growthRatePerMonth);
                }

                base.MoveToNextYear();
            }
        }

        public double GetTaxesAfterWithdrawal(double amount)
        {
            return amount * this.WithdrawalTaxRate;
        }

        public override void Process2()
        {
            for (int i = input.ageStopWork; i < input.ageRentStart; i++)
            {
                double withdrawalAmount = BasePortfolio.WithdrawalStrategy.GetWithdrawalAmount(i, this.GetType());

                this
                    .Buy(-withdrawalAmount);

                base.MoveToNextYear();
            }
        }
    }
}
