using System;
using System.Linq;

namespace Processing.Assets
{
    public class Stocks : Asset
    {
        public Stocks(Input _input) : base(_input) 
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
    }
}
