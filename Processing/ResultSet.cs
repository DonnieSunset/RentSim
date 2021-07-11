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

        public List<RentSimResultRow> ProcessStocks()
        {
            RentSimResultRow _curSnap = null;
            RentSimResultRow _lastSnap = null;

            var resultSet = new List<RentSimResultRow>();

            for (int i = input.ageCurrent; i < input.ageStopWork; i++)
            {
                _curSnap = new RentSimResultRow(input);
                _curSnap.age = i;
                _curSnap.stocksYearBegin = i == input.ageCurrent ? input.stocks : _lastSnap.stocksYearEnd;
                _curSnap.stocksYearEnd = _curSnap.stocksYearBegin;

                for (int month = 1; month <= 12; month++)
                {
                    _curSnap
                        .ApplyStockInvests(input.stocksMonthlyInvestAmount)
                        .ApplyStocksGrowth(this.stocksGrowthRatePerMonth);
                }

                resultSet.Add(_curSnap);
                _lastSnap = _curSnap;
            }

            return resultSet;
        }
    }
}
