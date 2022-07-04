using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ResultRowValidator
    {
        public static void ValidateAll(IEnumerable<ResultRow> resultRows, int startAge, int endAge)
        {
            resultRows = resultRows.OrderBy(x => x.age);
            AllAgesAvailable(resultRows, startAge, endAge);
            TransitionBetweenRows(resultRows);
        }

        public static void AllAgesAvailable(IEnumerable<ResultRow> resultRows, int startAge, int endAge)
        { 
            int duration = endAge - startAge;
            if (resultRows.Count() != duration)
            {
                throw new Exception($"ResultRowValidator: Wrong number of rows. Is <{resultRows.Count()}>, should be <{duration}>.");
            }

            for (int i = startAge; i < endAge; i++)
            {
                var currentRow = resultRows.Single(x => x.age == i);
            }
        }

        public static void TransitionBetweenRows(IEnumerable<ResultRow> resultRows)
        {
            var resultRowsList = resultRows.ToList();
            for (int i = 0; i < resultRowsList.Count() - 1; i++)
            {
                var current = resultRowsList[i];
                var next = resultRowsList[i + 1];

                if (current.cashYearEnd != next.cashYearBegin ||
                    current.stocksYearEnd != next.stocksYearBegin ||
                    current.metalsYearEnd != next.metalsYearBegin ||
                    current.TotalYearEnd != next.TotalYearBegin)
                {
                    throw new Exception($"ResultRowValidator: YearEnd and YearBegin mismatch at index {i} and {i + 1}.");
                }
            }
        }

        //todo: all ends up in 0
        //todo: all numbers have correct sign
        //todo: totals are the sum of all others
        //todo: no metals after saving phase

    }
}
