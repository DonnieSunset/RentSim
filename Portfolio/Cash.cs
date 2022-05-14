namespace Portfolio
{
    public class Cash 
    {
        private decimal myAmount;

        public decimal Amount => myAmount;

        public Cash() 
        {
            //switch (input.interestRateType)
            //{
            //    case InterestRateType.Relativ:
            //        growthRatePerMonth = RentSimMath.InterestPerYearToInterestPerMonthRelative(input.cashGrowthRate);
            //        break;
            //    case InterestRateType.Konform:
            //        growthRatePerMonth = RentSimMath.InterestPerYearToInterestPerMonth(input.cashGrowthRate);
            //        break;
            //    default:
            //        throw new Exception($"Unsupported Interest Rate Type: <{input.interestRateType}>.");
            //}
            //growthRatePerYear = input.cashGrowthRate;

            //this.Protocol.Last().yearBegin = _input.cash;
            //this.Protocol.Last().yearEnd = _input.cash;
        }

        public Cash Save(decimal amount)
        {
            myAmount += amount;
            return this;
        }

        public Cash Withdraw(decimal amount)
        {
            myAmount -= amount;
            return this;
        }

        //public Cash ApplyInterests(double interestRate)
        //{
        //    return (Cash)base.ApplyYearlyGrowth(interestRate);
        //}

        //public void PayTaxesForInterests()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
