using Domain;
using Portfolio;
using Protocol;

namespace Strategy
{
    //Annahme:
    //ich brauche mind. 3000, maximal 5000

    //rente: 500

    //brauche mind. 2500, maximal 4500

    //angenommener aktien crash: 50%

    //d.h.
    //minimal scenario:
    // 2 gleichungen mit 2 unbekannten -> lösbar

    //3000 = rente + festgeld + (aktien* 0.5)
    //5000 = rente + festgeld + aktien * 1.08^13

    //2500 = festgeld + 0.5* aktien
    //4500 = festgeld + aktien

    //festgeld = 4500 - aktien

    //2500 = 4500 -aktien + 0.5aktien

    //4000 = aktien
    //500 = festgeld

    //todo: auch zinsberechung auf festgeld + rente

    public class RentStartStrategy
    {
        //private AssetPortfolio myPortfolio;
        //private IProtocolWriter myProtocolWriter;
        //private RentPhaseInputData myRentPhaseInputData;

        //public RentStartStrategy(AssetPortfolio portfolio, RentPhaseInputData rentPhaseInputData, IProtocolWriter protocolWriter)
        //{
        //    myPortfolio = portfolio;
        //    myRentPhaseInputData = rentPhaseInputData;
        //    myProtocolWriter = protocolWriter;
        //}

        public void Process()
        {
            //initial cash amount
            //myProtocolWriter.LogBalanceYearBegin(
            //    Age.NewByAbsoluteAge(myLifeAssumptions.ageCurrent),
            //    myLifeAssumptions.cash
            //    );


            //for (int i = myRentPhaseInputData.AgeRentStart; i < myRentPhaseInputData.AgeEnd; i++)
            //{
            //    //Age age = Age.NewByAbsoluteAge(i);

            //    //TransactionDetails transAction = myPortfolio.WithdrawCash(neededMonthlyAmount * 12);
            //    //myProtocolWriter.Log(age, transAction);

            //    //transAction = myPortfolio.GetInterestsForCash();
            //    //myProtocolWriter.Log(age, transAction);
            //}
        }
    }
}
