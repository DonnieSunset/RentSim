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
                Age.NewByAbsoluteAge(myLifeAssumptions.ageCurrent),
                myLifeAssumptions.cash 
                );

            for (int i = myLifeAssumptions.ageCurrent; i < myLifeAssumptions.ageStopWork; i++)
            {
                Age age = Age.NewByAbsoluteAge(i);

                TransactionDetails transAction = myPortfolio.SaveCash(myLifeAssumptions.cashSaveAmountPerMonth * 12);
                myProtocolWriter.Log(age, transAction);

                transAction = myPortfolio.GetInterestsForCash();
                myProtocolWriter.Log(age, transAction);
            }
        }
    }
}