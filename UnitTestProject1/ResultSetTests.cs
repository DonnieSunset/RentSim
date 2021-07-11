using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processing;
using System;
using System.Collections;
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
                stocks = 60000,
                stocksGrowthRate = 7,
                stocksMonthlyInvestAmount = 700
            };

            //calculated with https://www.zinsen-berechnen.de/sparrechner.php
            List<RentSimResultRow> expectedResult = new List<RentSimResultRow>()
            {
                new RentSimResultRow { age=41, stocksYearBegin=60000, stocksInvests=8400, stocksGrowth=4662.82, stocksYearEnd=73062.82 },
                new RentSimResultRow { age=42, stocksYearBegin=73062.82, stocksInvests=8400, stocksGrowth=5607.13, stocksYearEnd=87069.95 },
                new RentSimResultRow { age=43, stocksYearBegin=87069.95, stocksInvests=8400, stocksGrowth=6619.71, stocksYearEnd=102089.65 },
                new RentSimResultRow { age=44, stocksYearBegin=102089.65, stocksInvests=8400, stocksGrowth=7705.48, stocksYearEnd=118195.14 },
                new RentSimResultRow { age=45, stocksYearBegin=118195.14, stocksInvests=8400, stocksGrowth=8869.75, stocksYearEnd=135464.88 },
                new RentSimResultRow { age=46, stocksYearBegin=135464.88, stocksInvests=8400, stocksGrowth=10118.18, stocksYearEnd=153983.06 },
                new RentSimResultRow { age=47, stocksYearBegin=153983.06, stocksInvests=8400, stocksGrowth=11456.86, stocksYearEnd=173839.93 },
                new RentSimResultRow { age=48, stocksYearBegin=173839.93, stocksInvests=8400, stocksGrowth=12892.32, stocksYearEnd=195132.24 },
                new RentSimResultRow { age=49, stocksYearBegin=195132.24, stocksInvests=8400, stocksGrowth=14431.54, stocksYearEnd=217963.78 },
                new RentSimResultRow { age=50, stocksYearBegin=217963.78, stocksInvests=8400, stocksGrowth=16082.03, stocksYearEnd=242445.81 },
                new RentSimResultRow { age=51, stocksYearBegin=242445.81, stocksInvests=8400, stocksGrowth=17851.84, stocksYearEnd=268697.65 },
                new RentSimResultRow { age=52, stocksYearBegin=268697.65, stocksInvests=8400, stocksGrowth=19749.59, stocksYearEnd=296847.24 },
                new RentSimResultRow { age=53, stocksYearBegin=296847.24, stocksInvests=8400, stocksGrowth=21784.52, stocksYearEnd=327031.76 },
                new RentSimResultRow { age=54, stocksYearBegin=327031.76, stocksInvests=8400, stocksGrowth=23966.57, stocksYearEnd=359398.33 },
                new RentSimResultRow { age=55, stocksYearBegin=359398.33, stocksInvests=8400, stocksGrowth=26306.35, stocksYearEnd=394104.67 },
                new RentSimResultRow { age=56, stocksYearBegin=394104.67, stocksInvests=8400, stocksGrowth=28815.27, stocksYearEnd=431319.95 },
                new RentSimResultRow { age=57, stocksYearBegin=431319.95, stocksInvests=8400, stocksGrowth=31505.57, stocksYearEnd=471225.51 },
                new RentSimResultRow { age=58, stocksYearBegin=471225.51, stocksInvests=8400, stocksGrowth=34390.34, stocksYearEnd=514015.86 },
                new RentSimResultRow { age=59, stocksYearBegin=514015.86, stocksInvests=8400, stocksGrowth=37483.66, stocksYearEnd=559899.52 },
            };

            //act
            var testee = new ResultSet(input);
            var actualResult = testee.ProcessStocks();

            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].age, actualResult[i].age);
                Assert.AreEqual(expectedResult[i].stocksYearBegin, Math.Round(actualResult[i].stocksYearBegin, 2));
                Assert.AreEqual(expectedResult[i].stocksInvests, Math.Round(actualResult[i].stocksInvests, 2));
                Assert.AreEqual(expectedResult[i].stocksGrowth, Math.Round(actualResult[i].stocksGrowth, 2));
                Assert.AreEqual(expectedResult[i].stocksYearEnd, Math.Round(actualResult[i].stocksYearEnd, 2));
            }
        }
    }
}
