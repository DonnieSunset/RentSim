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

        /// <summary>
        /// Gets the fraction of a given asset type
        /// compared to the sum of all assets.
        /// </summary>
        /// <param name="assetType">The asset type.</param>
        /// <returns>The fraction of the asset.</returns>
        public double GetAssetFraction(AgePhase agePhase, Type assetType)
        {
            //Scheisse: das hier darf nur aufgerufen werden wenn ALLE asset klassen schon prozessiert wurden sonst gibts ungleichgewichte!

            //TODO: remove the age parameter, replace with a enum. then do exstensive checks.
            int index;
            if (agePhase == AgePhase.StopWork)
            {
                index = Input.ageStopWork - Input.ageCurrent - 1;
            }
            else if (agePhase == AgePhase.RentStart)
            {
                index = Input.ageRentStart - Input.ageCurrent - 1;
            }
            else
            {
                throw new Exception($"Unknown age phase <{agePhase}>.");
            }

            //Console.WriteLine("S: xxx " + index + " / " + Cash.Protocol.Count + " / " + Stocks.Protocol.Count + " / " + Metals.Protocol.Count);

            double total = Cash.Protocol[index].yearEnd
                + Stocks.Protocol[index].yearEnd
                + Metals.Protocol[index].yearEnd;

            double result;
            if (assetType == typeof(Cash))
                result = Cash.Protocol[index].yearEnd / total;
            else if (assetType == typeof(Stocks))
                result = Stocks.Protocol[index].yearEnd / total;
            else if (assetType == typeof(Metals))
                result = Metals.Protocol[index].yearEnd / total;
            else
                throw new Exception($"Unknown asset type <{assetType}>.");

            return result;
        }

        /// <summary>
        /// Gets the average growth rate over all assets.
        /// </summary>
        /// <returns>The average growth rate over all assets.</returns>
        public double GetAverageGrowthRate(AgePhase agePhase)
        {
            var assetFractionCash = GetAssetFraction(agePhase, typeof(Cash)) * Input.cashGrowthRate;
            var assetFractionStocks = GetAssetFraction(agePhase, typeof(Stocks)) * Input.stocksGrowthRate;
            var assetFractionMetals = GetAssetFraction(agePhase, typeof(Metals)) * Input.metalsGrowthRate;

            var result = assetFractionCash + assetFractionStocks + assetFractionMetals;

             return result;
        }

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




            for (int i = Input.ageCurrent; i < Input.ageStopWork; i++)
            {
                for (int month = 1; month <= 12; month++)
                {
                    Cash
                       .Save(Input.cashMonthlyInvestAmount)
                       .ApplyInterests(Cash.GrowthRatePerMonth);

                    Stocks
                       .Buy(Input.stocksMonthlyInvestAmount)
                       .ApplyWorthIncrease(Stocks.GrowthRatePerMonth);

                    Metals
                       .Buy(Input.metalsMonthlyInvestAmount)
                       .ApplyWorthIncrease(Metals.GrowthRatePerMonth);

                }

                Cash.MoveToNextYear();
                Stocks.MoveToNextYear();
                Metals.MoveToNextYear();

                int protocolIndex = i - Input.ageCurrent;
                Total.Protocol.Last().age = i;
                Total.Protocol.Last().yearBegin = Total.listOfAssets.Select(x => x.Protocol[protocolIndex].yearBegin).Sum();
                Total.Protocol.Last().growth = Total.listOfAssets.Select(x => x.Protocol[protocolIndex].growth).Sum();
                Total.Protocol.Last().invests = Total.listOfAssets.Select(x => x.Protocol[protocolIndex].invests).Sum();
                Total.Protocol.Last().yearEnd = Total.listOfAssets.Select(x => x.Protocol[protocolIndex].yearEnd).Sum();
                Total.MoveToNextYear();
            }


            //WithdrawalStrategy.Calculate();
            //var withdrawalResults = WithdrawalStrategy.GetResults();

            //Cash.Process2(withdrawalResults.Cash);
            //Stocks.Process2(withdrawalResults.Stocks);
            //Metals.Process2(withdrawalResults.Metals);
            //Total.Process2(null);

        }
    }
}
