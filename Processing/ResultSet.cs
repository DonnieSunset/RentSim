using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing
{
    public class ResultSet
    {
        private Input input;
        private double stocksGrowthRatePerMonth;

        public ResultSet(Input _input)
        {
            input = _input;
            stocksGrowthRatePerMonth = RentSimMath.InterestPerYearToInterestPerMonthRelative(input.stocksGrowthRate);
        }

        //idee: zweistufiger approcach: erste stufe ist zeimlich lineatr nicht in jahren denken, sondern einfach kontinuierluich jeden monat
        // dann kommt postp-processing. dann wird aus dem kontinuierlichen zeitreihen die diskrete, auf jahre basierende statistik generiert.
        // und das alles dfann für jede asset klasse separat
        // welche veränderunge ngibts es: 
        // cash: -gewinn durch sparrate -gewinn durch zinsen -verlust durch entnahme
        // aktien: -gewinn durch sparrate (-gewinn durch zinsen -verlust durch kurzverlust) -verlust durch entnahme 
        // em: -gewinn durch kursschwankung -verlust durch entnahme
        // => nicht unterscheiden zwischen gewinn oder verlust, einfach in änderung messen: kursgewinn/crash ist das selbe, sparrate/entnahme ist das selbe

        public List<RentSimResultRow> ProcessAssets()
        {
            var resultSet = new List<RentSimResultRow>();

            //initialize the first row with input variables
            var _curSnap = new RentSimResultRow(input);

            for (int i = input.ageCurrent; i < input.ageStopWork; i++)
            {
                _curSnap.age = i;

                foreach (var asset in new Asset[] { _curSnap.stocks })
                {
                    for (int month = 1; month <= 12; month++)
                    {
                        asset
                            .ApplyInvests(input.stocksMonthlyInvestAmount)
                            .ApplyGrowth(this.stocksGrowthRatePerMonth);
                    }
                }

                resultSet.Add(_curSnap);
                _curSnap = new RentSimResultRow()
                {
                    stocks = new Asset() { yearBegin =_curSnap.stocks.yearEnd, yearEnd = _curSnap.stocks.yearEnd },
                    cash = new Asset() { yearBegin = _curSnap.cash.yearEnd, yearEnd = _curSnap.cash.yearEnd },
                    metals = new Asset() { yearBegin = _curSnap.metals.yearEnd, yearEnd = _curSnap.metals.yearEnd },
                };
            }

            //add the last (non processed) result set to the list in order to indicate the beginning of the last++ year
            _curSnap.age = resultSet.Last().age + 1;
            resultSet.Add(_curSnap);

            return resultSet;
        }

    }
    
}
