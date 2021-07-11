using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing
{
    public class Input
    {
        public int ageCurrent;
        public int ageStopWork;

        public int stocks;
        public int stocksGrowthRate;
        public int stocksMonthlyInvestAmount;
    }

 

    public class ResultSet
    {
        //public ResultSet()
        //{
        //    Total = new List<Snapshot>();
        //}

        //public List<Snapshot> Total;

        public List<RentSimResultRow> Process(Input _input)
        {
            RentSimResultRow _curSnap = null;
            RentSimResultRow _lastSnap = null;

            var resultSet = new List<RentSimResultRow>();

            for (int i = _input.ageCurrent; i <= _input.ageStopWork; i++)
            {
                _curSnap = new RentSimResultRow(_input);
                _curSnap.age = i;
                _curSnap.stocksYearBegin = i == _input.ageCurrent ? _input.stocks : _lastSnap.stocksYearEnd;
                _curSnap.stocksYearEnd = _curSnap.stocksYearBegin;

                _curSnap
                    .ApplyStockInvests()
                    .ApplyStocksGrowth();

                resultSet.Add(_curSnap);
                _lastSnap = _curSnap;
            }

            return resultSet;
        }
    }
}
