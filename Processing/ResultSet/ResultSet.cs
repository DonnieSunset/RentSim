using Processing.Assets;
using System;
using System.Collections.Generic;

namespace Processing
{
    //public class ResultSet
    //{
    //    private Input input;

    //    private Cash cash;
    //    private Stocks stocks;
    //    private Metals metals;
    //    private Total total;

    //    private int processingAges;

    //    public List<ResultRow> resultSet;

    //    public ResultSet(Input input, Cash cash, Stocks stocks, Metals metals, Total total)
    //    {
    //        this.input = input;
    //        this.cash = cash;
    //        this.stocks = stocks;
    //        this.metals = metals;
    //        this.total = total;

    //        processingAges = input.ageEnd - input.ageCurrent + 1;
    //        foreach (Asset a in new Asset[] { cash, stocks, metals, total })
    //        {
    //            if (a.Protocol.Count != processingAges)
    //            {
    //                string errorMsg = $"Asset <{a.GetType()}> cannot be processed in ResultSet because intended years span is <{processingAges}>, but asset index count is <{a.Protocol.Count}>";
    //                throw new Exception(errorMsg);
    //            }
    //        }
    //    }

    //    public List<ResultRow> ProcessAssets()
    //    {
    //        resultSet = new List<ResultRow>();

    //        for (int i = 0; i < processingAges; i++)
    //        {
    //            ResultRow row = new ResultRow();

    //            row.age = input.ageCurrent + i;
    //            row.cash = cash.Protocol[i];
    //            row.stocks = stocks.Protocol[i];
    //            row.metals = metals.Protocol[i];
    //            row.total = total.Protocol[i];

    //            if (row.age != row.cash.age
    //                || row.age != row.stocks.age
    //                || row.age != row.metals.age
    //                || row.age != row.total.age)
    //            {
    //                throw new Exception($"Age {row.age} not consistent in protocol entries.");
    //            }

    //            resultSet.Add(row);
    //        }

    //        return resultSet;
    //    }
    //}
}
