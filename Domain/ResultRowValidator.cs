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

            if (totalYearEnd != 0)
            {
                throw new Exception($"ResultRowValidator: Last row does not end up in zero. Actual value: {totalYearEnd}.");
            }
        }

        public static void AllNumbersHaveTheCorrectSign(IEnumerable<ResultRow> resultRows)
        {
            foreach (var resultRow in resultRows)
            {
                if (resultRow.age < 0 ||
                    resultRow.cashDeposits < 0 ||
                    resultRow.cashInterests < 0 ||
                    resultRow.cashTaxes < 0 ||
                    resultRow.cashWithdrawals < 0 ||
                    resultRow.cashYearBegin < 0 ||
                    resultRow.cashYearEnd < 0 ||
                    resultRow.stocksDeposits < 0 ||
                    resultRow.stocksInterests < 0 ||
                    resultRow.stocksTaxes < 0 ||
                    resultRow.stocksWithdrawals < 0 ||
                    resultRow.stocksYearBegin < 0 ||
                    resultRow.stocksYearEnd < 0 ||
                    resultRow.metalsDeposits < 0 ||
                    resultRow.metalsInterests < 0 ||
                    resultRow.metalsTaxes < 0 ||
                    resultRow.metalsWithdrawals < 0 ||
                    resultRow.metalsYearBegin < 0 ||
                    resultRow.metalsYearEnd < 0 ||
                    resultRow.TotalDeposits < 0 ||
                    resultRow.TotalInterests < 0 ||
                    resultRow.TotalTaxes < 0 ||
                    resultRow.TotalWithdrawals < 0 ||
                    resultRow.TotalYearBegin < 0 ||
                    resultRow.TotalYearEnd < 0)
                {
                    throw new Exception($"ResultRowValidator: row of age {resultRow.age} contains an entry with wrong sign.");
                }
            }
        }
            //todo: totals are the sum of all others
            //todo: no metals after saving phase

    }
}
