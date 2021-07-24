using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processing;
using System;
using System.Collections.Generic;

namespace Processing_uTest
{
    [TestClass]
    public class ResultSetTests
    {
        [TestMethod]
        public void ProcessStocks_DefaultStockGrowth_CalculationsAreCorrect()
        {
            //arrange
            Input input = new Input
            {
                ageCurrent = 41,
                ageStopWork = 60,

                interestRateType = InterestRateType.Relativ,

                stocks = 60000,
                stocksGrowthRate = 7,
                stocksMonthlyInvestAmount = 700
            };

            //calculated with https://www.zinsen-berechnen.de/sparrechner.php
            List<RentSimResultRow> expectedResult = new List<RentSimResultRow>()
            {
                new RentSimResultRow { age=41, stocks = new Asset { yearBegin=60000, invests=8400, growth=4662.82, yearEnd=73062.82 } },
                new RentSimResultRow { age=42, stocks = new Asset { yearBegin=73062.82, invests=8400, growth=5607.13, yearEnd=87069.95 } },
                new RentSimResultRow { age=43, stocks = new Asset { yearBegin=87069.95, invests=8400, growth=6619.71, yearEnd=102089.65 }  },
                new RentSimResultRow { age=44, stocks = new Asset { yearBegin=102089.65, invests=8400, growth=7705.48, yearEnd=118195.14 }  },
                new RentSimResultRow { age=45, stocks = new Asset { yearBegin=118195.14, invests=8400, growth=8869.75, yearEnd=135464.88 } },
                new RentSimResultRow { age=46, stocks = new Asset { yearBegin=135464.88, invests=8400, growth=10118.18, yearEnd=153983.06 } },
                new RentSimResultRow { age=47, stocks = new Asset { yearBegin=153983.06, invests=8400, growth=11456.86, yearEnd=173839.93 } },
                new RentSimResultRow { age=48, stocks = new Asset { yearBegin=173839.93, invests=8400, growth=12892.32, yearEnd=195132.24 } },
                new RentSimResultRow { age=49, stocks = new Asset { yearBegin=195132.24, invests=8400, growth=14431.54, yearEnd=217963.78 } },
                new RentSimResultRow { age=50, stocks = new Asset { yearBegin=217963.78, invests=8400, growth=16082.03, yearEnd=242445.81 } },
                new RentSimResultRow { age=51, stocks = new Asset { yearBegin=242445.81, invests=8400, growth=17851.84, yearEnd=268697.65 } },
                new RentSimResultRow { age=52, stocks = new Asset { yearBegin=268697.65, invests=8400, growth=19749.59, yearEnd=296847.24 } },
                new RentSimResultRow { age=53, stocks = new Asset { yearBegin=296847.24, invests=8400, growth=21784.52, yearEnd=327031.76 } },
                new RentSimResultRow { age=54, stocks = new Asset { yearBegin=327031.76, invests=8400, growth=23966.57, yearEnd=359398.33 } },
                new RentSimResultRow { age=55, stocks = new Asset { yearBegin=359398.33, invests=8400, growth=26306.35, yearEnd=394104.67 } },
                new RentSimResultRow { age=56, stocks = new Asset { yearBegin=394104.67, invests=8400, growth=28815.27, yearEnd=431319.95 } },
                new RentSimResultRow { age=57, stocks = new Asset { yearBegin=431319.95, invests=8400, growth=31505.57, yearEnd=471225.51 } },
                new RentSimResultRow { age=58, stocks = new Asset { yearBegin=471225.51, invests=8400, growth=34390.34, yearEnd=514015.86 } },
                new RentSimResultRow { age=59, stocks = new Asset { yearBegin=514015.86, invests=8400, growth=37483.66, yearEnd=559899.52 } },
                new RentSimResultRow { age=60, stocks = new Asset { yearBegin=559899.52, invests=0, growth=0, yearEnd=559899.52 } },
            };

            //act
            var testee = new ResultSet(input);
            var actualResult = testee.ProcessAssets();

            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].age, actualResult[i].age);
                Assert.AreEqual(expectedResult[i].stocks.yearBegin, Math.Round(actualResult[i].stocks.yearBegin, 2));
                Assert.AreEqual(expectedResult[i].stocks.invests, Math.Round(actualResult[i].stocks.invests, 2));
                Assert.AreEqual(expectedResult[i].stocks.growth, Math.Round(actualResult[i].stocks.growth, 2));
                Assert.AreEqual(expectedResult[i].stocks.yearEnd, Math.Round(actualResult[i].stocks.yearEnd, 2));
            }
        }
    }
}
