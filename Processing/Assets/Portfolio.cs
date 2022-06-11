using Processing.Withdrawal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing.Assets
{
    public class Portfolio
    {
        public Input Input
        {
            get;
            private set;
        }

        public IWithdrawalStrategy WithdrawalStrategy
        {
            get;
            private set;
        }

        public Cash Cash
        {
            get;
            private set;
        }

        public Stocks Stocks
        {
            get;
            private set;
        }

        public Metals Metals
        {
            get;
            private set;
        }

        public Total Total
        {
            get;
            private set;
        }

        public Portfolio(Input input)
        {
            Input = input;

            Cash = new Cash(Input, this);
            Stocks = new Stocks(Input, this);
            Metals = new Metals(Input, this);
            Total = new Total(Input, new List<Asset> { Cash, Stocks, Metals }, this);

            //WithdrawalStrategy = new UniformWithdrawalStrategy(this);
            WithdrawalStrategy = new SellAllWithdrawalStrategy(this);
        }

        public List<Asset> GetAssets()
        {
            return new List<Asset>() { Cash, Stocks, Metals };
        }

        ///// <summary>
        ///// Gets the fraction of a given asset type
        ///// compared to the sum of all assets.
        ///// </summary>
        ///// <param name="assetType">The asset type.</param>
        ///// <returns>The fraction of the asset.</returns>
        //public double GetAssetFraction(AgePhase agePhase, Type assetType)
        //{
        //    //Scheisse: das hier darf nur aufgerufen werden wenn ALLE asset klassen schon prozessiert wurden sonst gibts ungleichgewichte!

        //    //TODO: remove the age parameter, replace with a enum. then do exstensive checks.
        //    int index;
        //    if (agePhase == AgePhase.StopWork)
        //    {
        //        index = Input.ageStopWork - Input.ageCurrent - 1;
        //    }
        //    else if (agePhase == AgePhase.RentStart)
        //    {
        //        index = Input.ageRentStart - Input.ageCurrent - 1;
        //    }
        //    else
        //    {
        //        throw new Exception($"Unknown age phase <{agePhase}>.");
        //    }

        //    //Console.WriteLine("S: xxx " + index + " / " + Cash.Protocol.Count + " / " + Stocks.Protocol.Count + " / " + Metals.Protocol.Count);

        //    double total = Cash.Protocol[index].yearEnd
        //        + Stocks.Protocol[index].yearEnd
        //        + Metals.Protocol[index].yearEnd;

        //    double result;
        //    if (assetType == typeof(Cash))
        //        result = Cash.Protocol[index].yearEnd / total;
        //    else if (assetType == typeof(Stocks))
        //        result = Stocks.Protocol[index].yearEnd / total;
        //    else if (assetType == typeof(Metals))
        //        result = Metals.Protocol[index].yearEnd / total;
        //    else
        //        throw new Exception($"Unknown asset type <{assetType}>.");

        //    return result;
        //}

        ///// <summary>
        ///// Gets the average growth rate over all assets.
        ///// </summary>
        ///// <returns>The average growth rate over all assets.</returns>
        //public double GetAverageGrowthRate(AgePhase agePhase)
        //{
        //    var assetFractionCash = GetAssetFraction(agePhase, typeof(Cash)) * Input.cashGrowthRate;
        //    var assetFractionStocks = GetAssetFraction(agePhase, typeof(Stocks)) * Input.stocksGrowthRate;
        //    var assetFractionMetals = GetAssetFraction(agePhase, typeof(Metals)) * Input.metalsGrowthRate;

        //    var result = assetFractionCash + assetFractionStocks + assetFractionMetals;

        //     return result;
        //}

        public void Process()
        {
            //Cash.Process();
            //Stocks.Process();
            //Metals.Process();
            //Total.Process();

            //TODO PRIO: Hier muss EINMALIG!!! der withdrawal amount berechnet werden! dnach niewieder! und zwar für stopwork age. wir haben hier schon aöle infos die wir brahcen um alles zu berechnen. also machen wir es auch hier.vereinfach alles und macht den code lesbarer.

            //Auch einmalig berechnen: getAverageGrowthRate!!

            // darf eigentlich nicht hier berechnet werden, da obige annahme nur für die uniformWithdrawalStrategy gilt. also muss alles EINMALUG in der withdrawalstrategy berechnet werden.

            //double withdrawalAmount_Cash_AgeStopWork_Net = WithdrawalStrategy.GetWithdrawalAmount(Input.ageStopWork, Cash.GetType());
            //double withdrawalAmount_Cash_AgeRentStart_Net = WithdrawalStrategy.GetWithdrawalAmount(Input.ageRentStart, Cash.GetType());

            //double withdrawalAmount_Stocks_AgeStopWork_Net = WithdrawalStrategy.GetWithdrawalAmount(Input.ageStopWork, Stocks.GetType());
            //double withdrawalAmount_Stocks_AgeRentStart_Net = WithdrawalStrategy.GetWithdrawalAmount(Input.ageRentStart, Stocks.GetType());

            //double withdrawalAmount_Metals_AgeStopWork_Net = WithdrawalStrategy.GetWithdrawalAmount(Input.ageStopWork, Metals.GetType());
            //double withdrawalAmount_Metals_AgeRentStart_Net = WithdrawalStrategy.GetWithdrawalAmount(Input.ageRentStart, Metals.GetType());


            //int protocolIndex = 0;

            //(double amountNeededForRentPhaseMinimum, double amountNeededForRentPhaseComfort) = CalculateAmountNeededForRentPhase();
            
            // 1st step, calculate the two amounts for the phases 2 and 3. 
            // 1. What I need net : Rent->End
            // 2. What I need after taxes: Rent-> End
            // 3. How much is left for StopWork->Rent
            // 4. Depending on whats left, move stopWorkAge to lower or higher. StopWorkAge influences the available rent!

            //double amountNeededForRentPhase = CalculateAmountNeededForRentPhase();
            
            
            
            
            
            //(double rentPhaseWithdrawalRateLowRisk, double rentPhaseWithdrawalRateHighRisk) = CalculateRentPhaseWithdrawalRate();




            //for (int i = Input.ageCurrent; i < Input.ageStopWork; i++)
            //{
            //    for (int month = 1; month <= 12; month++)
            //    {
            //        Cash.Save(Input.cashMonthlyInvestAmount);
            //        Stocks.Buy(Input.stocksMonthlyInvestAmount);
            //        Metals.Buy(Input.metalsMonthlyInvestAmount);
                       
            //    }

            //    Cash.ApplyInterests(Cash.GrowthRatePerYear);
            //    Stocks.ApplyWorthIncrease(Stocks.GrowthRatePerYear);
            //    Metals.ApplyWorthIncrease(Metals.GrowthRatePerYear);
            //}

            //Metals.Sell(Metals.CurrentAmount, Cash);

            //for (int i = Input.ageStopWork; i < Input.ageRentStart; i++)
            //{
            //}

            //for (int i = Input.ageRentStart; i < Input.ageEnd; i++)
            //{
            //    var receivedAmountAfterTaxes = Stocks.SellSuchThatAfterTaxesTheGivenAmountIsAvailable(rentPhaseWithdrawalRateHighRisk, Cash);

            //    //hmm hier ist nicht ganz klar an welcher stelle die 26% weggehen. schon bei CalculateRentPhaseWithdrawalRate?

            //    Cash.Withdraw(receivedAmountAfterTaxes);
            //    Cash.Withdraw(rentPhaseWithdrawalRateLowRisk);

            //}

            ////Sell all
            //var stocksAmount = Stocks.Protocol.Last().yearBegin;
            //Stocks.SellAndPayTaxes(stocksAmount);

            //var metalsAmount = Metals.Protocol.Last().yearBegin;
            //Metals.Sell(metalsAmount);

            //Cash.Save(stocksAmount);
            //Cash.Save(metalsAmount);

            //protocolIndex++;
            //Total.Protocol.Last().yearBegin = Total.listOfAssets.Select(x => x.Protocol[protocolIndex].yearBegin).Sum();

            ////calculate rent amounts
            //double approxStopWorkAgeNetRent = RentSimMath.RentStopWorkAgeApproximation(
            //    Input.ageCurrent,
            //    Input.ageStopWork,
            //    Input.ageRentStart,
            //    Input.netStateRentFromCurrentAge,
            //    Input.netStateRentFromRentStartAge);

            //(double ratePhaseRent, double ratePhaseStopWork) = SparkassenFormel.CalculateGrossPayoutRateWithRent(
            //    startCapital: Total.Protocol.Last().yearBegin,
            //    yearsStopWorkPhase: Input.ageRentStart - Input.ageStopWork,
            //    yearsRentPhase: Input.ageEnd - Input.ageRentStart,
            //    interestRate: 0,
            //    endCapital: 0,
            //    rent: approxStopWorkAgeNetRent,
            //    calcTaxes: (x) => 0);
        }
    }
}
