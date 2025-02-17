﻿using Blazorise.Charts;
using Domain;
using Protocol;

namespace RentSimS.Pages
{
    public partial class Index
    {
        private ChartColor chartColorCash = ChartColor.FromRgba(255, 0, 0, 0.5f);
        private ChartColor chartBorderColorCash = ChartColor.FromRgba(255, 0, 0, 1f);

        private ChartColor chartColorStocks = ChartColor.FromRgba(0, 0, 255, 0.5f);
        private ChartColor chartBorderColorStocks = ChartColor.FromRgba(0, 0, 255, 1f);

        private ChartColor chartColorMetals = ChartColor.FromRgba(255, 215, 0, 0.5f);
        private ChartColor chartBorderColorMetals = ChartColor.FromRgba(255, 215, 0, 1f);

        private ChartColor chartColorTotal = ChartColor.FromRgba(0, 0, 0, 0.5f);
        private ChartColor chartBorderColorTotal = ChartColor.FromRgba(0, 0, 0, 1f);

        private ChartColor chartColorNone = ChartColor.FromRgba(255, 255, 255, 0.0f);

        public async Task DrawBarChart(BarChart<decimal>? barChart, IReadOnlyCollection<ResultRow> resultRows, LifeAssumptions lifeAssumptions)
        {
            string[] labels = Enumerable.Range(lifeAssumptions.ageCurrent, lifeAssumptions.ageEnd - lifeAssumptions.ageCurrent + 1)
                .Select(x => x.ToString())
                .ToArray();


            BarChartDataset<decimal> dataBarChartCash = new BarChartDataset<decimal>
            {
                Label = "Cash",
                BackgroundColor = Enumerable.Repeat<string>(chartColorCash, resultRows.Count()).ToList<string>(),
                BorderColor = Enumerable.Repeat<string>(chartBorderColorCash, resultRows.Count()).ToList<string>(),
                Data = resultRows
                                .Select(x => x.Cash.YearBegin)
                                    .Append(resultRows.Last().Cash.YearEnd)
                                .ToList(),
            };

            BarChartDataset<decimal> dataBarChartStocks = new BarChartDataset<decimal>
            {
                Label = "Stocks",
                BackgroundColor = Enumerable.Repeat<string>(chartColorStocks, resultRows.Count()).ToList<string>(),
                BorderColor = Enumerable.Repeat<string>(chartBorderColorStocks, resultRows.Count()).ToList<string>(),
                Data = resultRows.Select(x => x.Stocks.YearBegin)
                                    .Append(resultRows.Last().Stocks.YearEnd)
                                .ToList(),
            };

            BarChartDataset<decimal> dataBarChartMetals = new BarChartDataset<decimal>
            {
                Label = "Metals",
                BackgroundColor = Enumerable.Repeat<string>(chartColorMetals, resultRows.Count()).ToList<string>(),
                BorderColor = Enumerable.Repeat<string>(chartBorderColorMetals, resultRows.Count()).ToList<string>(),
                Data = resultRows.Select(x => x.Metals.YearBegin)
                                    .Append(resultRows.Last().Metals.YearEnd)
                                .ToList(),
            };

            BarChartOptions dataBarChartOptions = new BarChartOptions()
            {
                Scales = new ChartScales
                {
                    X = new ChartAxis { Stacked = false },
                    Y = new ChartAxis { Stacked = false, BeginAtZero = true, Min = 0 }
                },
            };

            if (barChart != null)
            {
                await barChart.Clear();
                await barChart.SetOptions(dataBarChartOptions);
                await barChart.AddLabelsDatasetsAndUpdate(labels, dataBarChartCash, dataBarChartStocks, dataBarChartMetals);
            }
        }

        public async Task DrawLineChart(LineChart<decimal>? lineChart, IReadOnlyCollection<ResultRow> resultRows, LifeAssumptions lifeAssumptions)
        {
            string[] labels = Enumerable.Range(lifeAssumptions.ageCurrent, lifeAssumptions.ageEnd - lifeAssumptions.ageCurrent + 1)
                .Select(x => x.ToString())
                .ToArray();

            LineChartDataset<decimal> datalineChartCash = new LineChartDataset<decimal>
            {
                BackgroundColor = new List<string> { chartColorNone },
                Label = "Cash",
                BorderColor = new List<string> { chartColorCash },
                Fill = true,
                PointRadius = 2,
                BorderDash = new List<int> { },
                Data = resultRows
                                .Select(x => x.Cash.YearBegin)
                                    .Append(resultRows.Last().Cash.YearEnd)
                                .ToList(),
            };

            LineChartDataset<decimal> datalineChartStocks = new LineChartDataset<decimal>
            {
                BackgroundColor = new List<string> { chartColorNone },
                Label = "Stocks",
                BorderColor = new List<string> { chartColorStocks },
                Fill = true,
                PointRadius = 2,
                BorderDash = new List<int> { },
                Data = resultRows
                    .Select(x => x.Stocks.YearBegin)
                        .Append(resultRows.Last().Stocks.YearEnd)
                    .ToList(),
            };

            LineChartDataset<decimal> datalineChartMetals = new LineChartDataset<decimal>
            {
                BackgroundColor = new List<string> { chartColorNone },
                Label = "Metals",
                BorderColor = new List<string> { chartColorMetals },
                Fill = true,
                PointRadius = 2,
                BorderDash = new List<int> { },
                Data = resultRows
                    .Select(x => x.Metals.YearBegin)
                        .Append(resultRows.Last().Metals.YearEnd)
                    .ToList(),
            };

            LineChartDataset<decimal> datalineChartTotal = new LineChartDataset<decimal>
            {
                BackgroundColor = new List<string> { chartColorNone },
                Label = "Total",
                BorderColor = new List<string> { chartColorTotal },
                Fill = true,
                PointRadius = 2,
                BorderDash = new List<int> { },
                Data = resultRows
                    .Select(x => x.TotalYearBegin)
                        .Append(resultRows.Last().TotalYearEnd)
                    .ToList(),
            };

            LineChartOptions lineBarChartOptions = new LineChartOptions()
            {
                Scales = new ChartScales
                {
                    X = new ChartAxis { Stacked = false },
                    Y = new ChartAxis { Stacked = false, BeginAtZero = true, Min = 0 }
                },
            };

            if (lineChart != null)
            {
                await lineChart.Clear();
                await lineChart.SetOptions(lineBarChartOptions);
                await lineChart.AddLabelsDatasetsAndUpdate(labels, datalineChartCash, datalineChartStocks, datalineChartMetals, datalineChartTotal);
            }
        }
    }
}
