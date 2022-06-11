using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processing;
using Processing.Assets;
using System;
using System.Collections.Generic;

namespace Processing_uTest
{
    [TestClass]
    public class ResultSetTests
    {
        //[TestMethod]
        //public void ProcessStocks_DefaultStockGrowth_CalculationsAreCorrect()
        //{
        //    //arrange
        //    Input input = new Input
        //    {
        //        ageCurrent = 41,
        //        ageStopWork = 60,

        //        interestRateType = InterestRateType.Relativ,

        //        stocks = 60000,
        //        stocksGrowthRate = 7,
        //        stocksMonthlyInvestAmount = 700
        //    };

        //    //calculated with https://www.zinsen-berechnen.de/sparrechner.php
        //    List<ResultRow> expectedResult = new List<ResultRow>()
        //    {
        //        new ResultRow { age=41, stocks = new ProtocolEntry { yearBegin=60000, invests=8400, growth=4662.82, yearEnd=73062.82 } },
        //        new ResultRow { age=42, stocks = new ProtocolEntry { yearBegin=73062.82, invests=8400, growth=5607.13, yearEnd=87069.95 } },
        //        new ResultRow { age=43, stocks = new ProtocolEntry { yearBegin=87069.95, invests=8400, growth=6619.71, yearEnd=102089.65 }  },
        //        new ResultRow { age=44, stocks = new ProtocolEntry { yearBegin=102089.65, invests=8400, growth=7705.48, yearEnd=118195.14 }  },
        //        new ResultRow { age=45, stocks = new ProtocolEntry { yearBegin=118195.14, invests=8400, growth=8869.75, yearEnd=135464.88 } },
        //        new ResultRow { age=46, stocks = new ProtocolEntry { yearBegin=135464.88, invests=8400, growth=10118.18, yearEnd=153983.06 } },
        //        new ResultRow { age=47, stocks = new ProtocolEntry { yearBegin=153983.06, invests=8400, growth=11456.86, yearEnd=173839.93 } },
        //        new ResultRow { age=48, stocks = new ProtocolEntry { yearBegin=173839.93, invests=8400, growth=12892.32, yearEnd=195132.24 } },
        //        new ResultRow { age=49, stocks = new ProtocolEntry { yearBegin=195132.24, invests=8400, growth=14431.54, yearEnd=217963.78 } },
        //        new ResultRow { age=50, stocks = new ProtocolEntry { yearBegin=217963.78, invests=8400, growth=16082.03, yearEnd=242445.81 } },
        //        new ResultRow { age=51, stocks = new ProtocolEntry { yearBegin=242445.81, invests=8400, growth=17851.84, yearEnd=268697.65 } },
        //        new ResultRow { age=52, stocks = new ProtocolEntry { yearBegin=268697.65, invests=8400, growth=19749.59, yearEnd=296847.24 } },
        //        new ResultRow { age=53, stocks = new ProtocolEntry { yearBegin=296847.24, invests=8400, growth=21784.52, yearEnd=327031.76 } },
        //        new ResultRow { age=54, stocks = new ProtocolEntry { yearBegin=327031.76, invests=8400, growth=23966.57, yearEnd=359398.33 } },
        //        new ResultRow { age=55, stocks = new ProtocolEntry { yearBegin=359398.33, invests=8400, growth=26306.35, yearEnd=394104.67 } },
        //        new ResultRow { age=56, stocks = new ProtocolEntry { yearBegin=394104.67, invests=8400, growth=28815.27, yearEnd=431319.95 } },
        //        new ResultRow { age=57, stocks = new ProtocolEntry { yearBegin=431319.95, invests=8400, growth=31505.57, yearEnd=471225.51 } },
        //        new ResultRow { age=58, stocks = new ProtocolEntry { yearBegin=471225.51, invests=8400, growth=34390.34, yearEnd=514015.86 } },
        //        new ResultRow { age=59, stocks = new ProtocolEntry { yearBegin=514015.86, invests=8400, growth=37483.66, yearEnd=559899.52 } },
        //        new ResultRow { age=60, stocks = new ProtocolEntry { yearBegin=559899.52, invests=0, growth=0, yearEnd=559899.52 } },
        //    };

        //    var testee = new Stocks(input, null);
        //    testee.Process();

        //    Assert.AreEqual(expectedResult.Count, testee.Protocol.Count);
        //    for (int i = 0; i < expectedResult.Count; i++)
        //    {
        //        Assert.AreEqual(expectedResult[i].age, testee.Protocol[i].age);
        //        Assert.AreEqual(expectedResult[i].stocks.yearBegin, Math.Round(testee.Protocol[i].yearBegin, 2));
        //        Assert.AreEqual(expectedResult[i].stocks.invests, Math.Round(testee.Protocol[i].invests, 2));
        //        Assert.AreEqual(expectedResult[i].stocks.growth, Math.Round(testee.Protocol[i].growth, 2));
        //        Assert.AreEqual(expectedResult[i].stocks.yearEnd, Math.Round(testee.Protocol[i].yearEnd, 2));
        //    }
        //}
    }
}
