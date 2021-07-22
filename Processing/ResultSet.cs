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
        private double cashGrowthRatePerMonth;
        private double metalsGrowthRatePerMonth;

        public ResultSet(Input _input)
        {
            input = _input;
            stocksGrowthRatePerMonth = RentSimMath.InterestPerYearToInterestPerMonthRelative(input.stocksGrowthRate);
            cashGrowthRatePerMonth = RentSimMath.InterestPerYearToInterestPerMonthRelative(input.cashGrowthRate);
            metalsGrowthRatePerMonth = RentSimMath.InterestPerYearToInterestPerMonthRelative(input.metalsGrowthRate);
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

                for (int month = 1; month <= 12; month++)
                {
                    _curSnap.stocks
                        .ApplyInvests(input.stocksMonthlyInvestAmount)
                        .ApplyGrowth(this.stocksGrowthRatePerMonth);
                    
                    _curSnap.cash
                       .ApplyInvests(input.cashMonthlyInvestAmount)
                       .ApplyGrowth(this.cashGrowthRatePerMonth);

                    _curSnap.metals
                       .ApplyInvests(input.metalsMonthlyInvestAmount)
                       .ApplyGrowth(this.metalsGrowthRatePerMonth);

                    _curSnap.total.yearBegin = _curSnap.stocks.yearBegin + _curSnap.cash.yearBegin + _curSnap.metals.yearBegin;
                    _curSnap.total.invests = _curSnap.stocks.invests + _curSnap.cash.invests + _curSnap.metals.invests;
                    _curSnap.total.growth = _curSnap.stocks.growth + _curSnap.cash.growth + _curSnap.metals.growth;
                    _curSnap.total.yearEnd = _curSnap.stocks.yearEnd + _curSnap.cash.yearEnd + _curSnap.metals.yearEnd;
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
