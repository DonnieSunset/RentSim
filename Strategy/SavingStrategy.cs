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
            for (int i = myLifeAssumptions.ageCurrent; i < myLifeAssumptions.ageStopWork; i++)
            {
                Age age = Age.NewByAbsoluteAge(i);

                TransactionDetails transAction = myPortfolio.SaveCash(100);
                myProtocolWriter.Log(age, transAction);
            }
        }
    }
}