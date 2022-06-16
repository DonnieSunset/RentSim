using Domain;
using Portfolio;
using Protocol;

namespace Strategy
{
    public class SavingStrategy
    {
        private AssetPortfolio myPortfolio;
        private IProtocolWriter myProtocolWriter;
        private LifeAssumptions myLifeAssumptions;

        public SavingStrategy(AssetPortfolio portfolio, LifeAssumptions lifeAssumptions, IProtocolWriter protocolWriter)
        { 
            myPortfolio = portfolio;
            myLifeAssumptions = lifeAssumptions;
            myProtocolWriter = protocolWriter;
        }

        public void Process()
        {
            //initial cash amount
            myProtocolWriter.LogBalanceYearBegin(
                myLifeAssumptions.ageCurrent,
                myLifeAssumptions.cash,
                myLifeAssumptions.stocks
                );

            for (int age = myLifeAssumptions.ageCurrent; age < myLifeAssumptions.ageStopWork; age++)
            {
                //Age age = Age.NewByAbsoluteAge(i);

                TransactionDetails transAction = myPortfolio.SaveCash(myLifeAssumptions.cashSaveAmountPerMonth * 12);
                myProtocolWriter.Log(age, transAction);

                //transAction = myPortfolio.GetInterestsForCash();
                //myProtocolWriter.Log(age, transAction);
            }
        }
    }
}