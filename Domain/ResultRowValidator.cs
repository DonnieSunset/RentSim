using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ResultRowValidator
    {
        public static void ValidateAll(IEnumerable<ResultRow> resultRows, int ageCurrent, int ageEnd)
        {
            AllAgesAvailable(resultRows, ageCurrent, ageEnd);
            TransitionBetweenRows(resultRows, ageCurrent, ageEnd);
            AllEndsUpInZero(resultRows);
            AllNumbersHaveTheCorrectSign(resultRows);
            EndTotalsAreTheSUmOfAllSingleValues(resultRows);
        }

        public static void AllAgesAvailable(IEnumerable<ResultRow> resultRows, int ageCurrent, int ageEnd)
        { 
            int duration = ageEnd - ageCurrent;
            if (resultRows.Count() != duration)
            {
                throw new Exception($"ResultRowValidator: Wrong number of rows. Is <{resultRows.Count()}>, should be <{duration}>.");
            }

            for (int i = ageCurrent; i < ageEnd; i++)
            {
                var currentRow = resultRows.Single(x => x.age == i);
            }
        }

        public static void TransitionBetweenRows(IEnumerable<ResultRow> resultRows, int ageCurrent, int ageEnd)
        {
            for (int i = ageCurrent; i < ageEnd - 1; i++)
            {
                var current = resultRows.Single(x => x.age == i);
                var next = resultRows.Single(x => x.age == i + 1);

                if (Decimal.Round(current.cashYearEnd, 3) != Decimal.Round(next.cashYearBegin, 3))
                {
                    throw new Exception($"ResultRowValidator: Cash YearEnd and YearBegin mismatch at index {i} ({current.cashYearEnd}) and {i + 1} ({next.cashYearBegin}).");
                }

                if (Decimal.Round(current.stocksYearEnd, 3) != Decimal.Round(next.stocksYearBegin, 3))
                {
                    throw new Exception($"ResultRowValidator: Stocks YearEnd and YearBegin mismatch at index {i} ({current.stocksYearEnd}) and {i + 1} ({next.stocksYearBegin}).");
                }

                if (Decimal.Round(current.metalsYearEnd, 3) != Decimal.Round(next.metalsYearBegin, 3))
                {
                    throw new Exception($"ResultRowValidator: Metals YearEnd and YearBegin mismatch at index {i} ({current.metalsYearEnd}) and {i + 1} ({next.metalsYearBegin}).");
                }

                if (Decimal.Round(current.TotalYearEnd, 3) != Decimal.Round(next.TotalYearBegin, 3))
                {
                    throw new Exception($"ResultRowValidator: Total YearEnd and YearBegin mismatch at index {i} ({current.TotalYearEnd}) and {i + 1} ({next.TotalYearBegin}).");
                }
            }
        }

        public static void AllEndsUpInZero(IEnumerable<ResultRow> resultRows)
        {
            var totalYearEnd = resultRows.MaxBy(x => x.age).TotalYearEnd;

            if (Decimal.Round(totalYearEnd, 3) != 0)
            {
                throw new Exception($"ResultRowValidator: Last row does not end up in zero. Actual value: {totalYearEnd}.");
            }
        }

        public static void AllNumbersHaveTheCorrectSign(IEnumerable<ResultRow> resultRows)
        {
            foreach (var resultRow in resultRows)
            {
                if (resultRow.age < 0 ||
                    Decimal.Round(resultRow.cashInterests, 3) < 0 ||
                    Decimal.Round(resultRow.cashTaxes, 3) > 0 ||
                    Decimal.Round(resultRow.cashYearBegin, 3) < 0 ||
                    Decimal.Round(resultRow.cashYearEnd, 3) < 0 ||
                    Decimal.Round(resultRow.stocksInterests, 3) < 0 ||
                    Decimal.Round(resultRow.stocksTaxes, 3) > 0 ||
                    Decimal.Round(resultRow.stocksYearBegin, 3) < 0 ||
                    Decimal.Round(resultRow.stocksYearEnd, 3) < 0 ||
                    Decimal.Round(resultRow.metalsInterests, 3) < 0 ||
                    Decimal.Round(resultRow.metalsTaxes, 3) > 0 ||
                    Decimal.Round(resultRow.metalsYearBegin, 3) < 0 ||
                    Decimal.Round(resultRow.metalsYearEnd, 3) < 0 ||
                    Decimal.Round(resultRow.TotalInterests, 3) < 0 ||
                    Decimal.Round(resultRow.TotalTaxes, 3) > 0 ||
                    Decimal.Round(resultRow.TotalYearBegin, 3) < 0 ||
                    Decimal.Round(resultRow.TotalYearEnd, 3) < 0)
                {
                    throw new Exception($"ResultRowValidator: row of age {resultRow.age} contains an entry with wrong sign.");
                }
            }
        }

        public static void EndTotalsAreTheSUmOfAllSingleValues(IEnumerable<ResultRow> resultRows)
        {
            foreach (var resultRow in resultRows)
            {
                bool eqCash = Decimal.Round(resultRow.cashYearBegin + resultRow.cashDeposits.Sum() + resultRow.cashInterests + resultRow.cashTaxes, 3) == Decimal.Round(resultRow.cashYearEnd, 3);
                bool eqStocks = Decimal.Round(resultRow.stocksYearBegin + resultRow.stocksDeposits.Sum() + resultRow.stocksInterests + resultRow.stocksTaxes, 3) == Decimal.Round(resultRow.stocksYearEnd, 3);
                bool eqMetals = Decimal.Round(resultRow.metalsYearBegin + resultRow.metalsDeposits.Sum() + resultRow.metalsInterests + resultRow.metalsTaxes, 3) == Decimal.Round(resultRow.metalsYearEnd, 3);
                bool eqTotals = Decimal.Round(resultRow.TotalYearBegin + resultRow.TotalDeposits + resultRow.TotalInterests + resultRow.TotalTaxes, 3) == Decimal.Round(resultRow.TotalYearEnd, 3);

                if (!eqCash || !eqStocks || !eqMetals || !eqTotals)
                {
                    throw new Exception($"ResultRowValidator: sum of single values at age {resultRow.age} does not sum up to total value.");
                }
            }
        }
            //todo: no metals after saving phase
    }
}
