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
        public int stocksInterestRate;
        public int stocksSaveAmount;
    }

    public class Snapshot
    {
        public int age;
        public double stocksYearBegin;
        public double stocksInvests;
        public double stocksInterests;
        public double stocksYearEnd;
    }

    public class ResultSet
    {
        //public ResultSet()
        //{
        //    Total = new List<Snapshot>();
        //}

        //public List<Snapshot> Total;

        public List<Snapshot> Process(Input _input)
        {
            Snapshot _curSnap = null;
            Snapshot _lastSnap = null;

            var resultSet = new List<Snapshot>();

            for (int i = _input.ageCurrent; i < _input.ageStopWork; i++)
            {
                _curSnap = new Snapshot();
                _curSnap.age = i;
                _curSnap.stocksYearBegin = i == _input.ageCurrent ? _input.stocks : _lastSnap.stocksYearEnd;
                _curSnap.stocksInvests = _input.stocksSaveAmount * 12;
                _curSnap.stocksInterests = (_curSnap.stocksYearBegin + _curSnap.stocksInvests) * _input.stocksInterestRate / 100;
                _curSnap.stocksYearEnd = _curSnap.stocksYearBegin + _curSnap.stocksInvests + _curSnap.stocksInterests;

                resultSet.Add(_curSnap);
                _lastSnap = _curSnap;
            }

            return resultSet;
        }
    }
}
