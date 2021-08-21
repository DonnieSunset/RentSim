using System.Collections.Generic;
using System.Linq;

namespace Processing.Assets
{
    public class Total : Asset
    {
        List<Asset> listOfAssets;

        public Total(Input _input, List<Asset> assets) : base(_input)
        {
            listOfAssets = assets;
        }

        public override void Process()
        {
            protocol.Clear();
            protocol.Add(new ProtocolEntry
            {
            });

            var current = protocol.Last();

            for (int i = 0; i < input.ageStopWork - input.ageCurrent; i++)
            {
                current.age = listOfAssets.First().protocol[i].age;
                current.yearBegin = listOfAssets.Select(x => x.protocol[i].yearBegin).Sum();
                current.growth = listOfAssets.Select(x => x.protocol[i].growth).Sum();
                current.invests = listOfAssets.Select(x => x.protocol[i].invests).Sum();
                current.yearEnd = listOfAssets.Select(x => x.protocol[i].yearEnd).Sum();

                current = base.MoveToNextYear().protocol.Last();
            }
        }
    }
}
