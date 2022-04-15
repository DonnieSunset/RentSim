using Processing.Withdrawal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Processing.Assets
{
    public class Total : Asset
    {
        public List<Asset> listOfAssets;

        public Total(Input _input, List<Asset> assets, Portfolio portfolio) : base(_input, portfolio)
        {
            listOfAssets = assets;
        }

        //public override void Process()
        //{

        //    var current = Protocol.Last();

        //    for (int i = 0; i < input.ageStopWork - input.ageCurrent; i++)
        //    {
        //        current.age = listOfAssets.First().Protocol[i].age;
        //        current.yearBegin = listOfAssets.Select(x => x.Protocol[i].yearBegin).Sum();
        //        current.growth = listOfAssets.Select(x => x.Protocol[i].growth).Sum();
        //        current.invests = listOfAssets.Select(x => x.Protocol[i].invests).Sum();
        //        current.yearEnd = listOfAssets.Select(x => x.Protocol[i].yearEnd).Sum();

        //        current = base.MoveToNextYear().Protocol.Last();
        //    }
        //}

        public override void Process2(AssetWithdrawalRateInfo withdrawalRateInfo)
        {
            var current = Protocol.Last();

            for (int i = input.ageStopWork - input.ageCurrent; i < input.ageEnd - input.ageCurrent; i++)
            {
                current.age = listOfAssets.First().Protocol[i].age;
                current.yearBegin = listOfAssets.Select(x => x.Protocol[i].yearBegin).Sum();
                current.growth = listOfAssets.Select(x => x.Protocol[i].growth).Sum();
                current.invests = listOfAssets.Select(x => x.Protocol[i].invests).Sum();
                current.yearEnd = listOfAssets.Select(x => x.Protocol[i].yearEnd).Sum();

                current = base.MoveToNextYear().Protocol.Last();
            }
        }
    }
}
