namespace Domain
{
    public class ResultRowValidator
    {
        private static int roundingAccuracy = 2;

        public static void ValidateAll(IEnumerable<ResultRow> resultRows, int ageCurrent, int ageStopWork, int ageEnd)
        {
            AllAgesAvailable(resultRows, ageCurrent, ageEnd);
            TransitionBetweenRows(resultRows, ageCurrent, ageEnd);
            AllNumbersHaveTheCorrectSign(resultRows);
            EndTotalsAreTheSumOfAllSingleValues(resultRows);
            AllEndsUpInZero(resultRows, ageEnd);
        }

        private static void AllAgesAvailable(IEnumerable<ResultRow> resultRows, int ageCurrent, int ageEnd)
        { 
            int duration = ageEnd - ageCurrent;
            if (resultRows.Count() != duration)
            {
                throw new Exception($"ResultRowValidator: Wrong number of rows. Is <{resultRows.Count()}>, should be <{duration}>.");
            }

            for (int i = ageCurrent; i < ageEnd; i++)
            {
                var currentRow = resultRows.Single(x => x.Age == i);
            }
        }

        private static void TransitionBetweenRows(IEnumerable<ResultRow> resultRows, int ageCurrent, int ageEnd)
        {
            for (int i = ageCurrent; i < ageEnd - 1; i++)
            {
                var current = resultRows.Single(x => x.Age == i);
                var next = resultRows.Single(x => x.Age == i + 1);

                if (Decimal.Round(current.Cash.YearEnd, roundingAccuracy) != Decimal.Round(next.Cash.YearBegin, roundingAccuracy))
                {
                    throw new Exception($"ResultRowValidator: Cash YearEnd and YearBegin mismatch at index {i} ({current.Cash.YearEnd}) and {i + 1} ({next.Cash.YearBegin}).");
                }

                if (Decimal.Round(current.Stocks.YearEnd, roundingAccuracy) != Decimal.Round(next.Stocks.YearBegin, roundingAccuracy))
                {
                    throw new Exception($"ResultRowValidator: Stocks YearEnd and YearBegin mismatch at index {i} ({current.Stocks.YearEnd}) and {i + 1} ({next.Stocks.YearBegin}).");
                }

                if (Decimal.Round(current.Metals.YearEnd, roundingAccuracy) != Decimal.Round(next.Metals.YearBegin, roundingAccuracy))
                {
                    throw new Exception($"ResultRowValidator: Metals YearEnd and YearBegin mismatch at index {i} ({current.Metals.YearEnd}) and {i + 1} ({next.Metals.YearBegin}).");
                }

                if (Decimal.Round(current.TotalYearEnd, roundingAccuracy) != Decimal.Round(next.TotalYearBegin, roundingAccuracy))
                {
                    throw new Exception($"ResultRowValidator: Total YearEnd and YearBegin mismatch at index {i} ({current.TotalYearEnd}) and {i + 1} ({next.TotalYearBegin}).");
                }
            }
        }

        private static void AllEndsUpInZero(IEnumerable<ResultRow> resultRows, int ageEnd)
        {
            var totalYearEnd = resultRows.Single(x => x.Age == ageEnd-1).TotalYearEnd;

            if (Decimal.Round(totalYearEnd, roundingAccuracy) != 0)
            {
                throw new Exception($"ResultRowValidator: Last row does not end up in zero. Actual value: {totalYearEnd}.");
            }
        }

        private static void AllNumbersHaveTheCorrectSign(IEnumerable<ResultRow> resultRows)
        {
            foreach (var resultRow in resultRows)
            {
                if (resultRow.Age < 0 ||
                    Decimal.Round(resultRow.Cash.Interests, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.Stocks.Interests, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.Metals.Interests, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.TotalInterests, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.Cash.Taxes, roundingAccuracy) > 0 ||
                    Decimal.Round(resultRow.Stocks.Taxes, roundingAccuracy) > 0 ||
                    Decimal.Round(resultRow.Metals.Taxes, roundingAccuracy) > 0 ||
                    Decimal.Round(resultRow.TotalTaxes, roundingAccuracy) > 0 ||
                    Decimal.Round(resultRow.Cash.YearBegin, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.Stocks.YearBegin, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.Metals.YearBegin, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.TotalYearBegin, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.Cash.YearEnd, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.Stocks.YearEnd, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.Metals.YearEnd, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.TotalYearEnd, roundingAccuracy) < 0)
                {
                    throw new Exception($"ResultRowValidator: row of age {resultRow.Age} contains an entry with wrong sign.");
                }
            }
        }

        private static void EndTotalsAreTheSumOfAllSingleValues(IEnumerable<ResultRow> resultRows)
        {
            foreach (var resultRow in resultRows)
            {
                bool eqCash = Decimal.Round(resultRow.Cash.YearBegin + resultRow.Cash.Deposits + resultRow.Cash.Interests + resultRow.Cash.Taxes, roundingAccuracy) == Decimal.Round(resultRow.Cash.YearEnd, roundingAccuracy);
                bool eqStocks = Decimal.Round(resultRow.Stocks.YearBegin + resultRow.Stocks.Deposits + resultRow.Stocks.Interests + resultRow.Stocks.Taxes, roundingAccuracy) == Decimal.Round(resultRow.Stocks.YearEnd, roundingAccuracy);
                bool eqMetals = Decimal.Round(resultRow.Metals.YearBegin + resultRow.Metals.Deposits + resultRow.Metals.Interests + resultRow.Metals.Taxes, roundingAccuracy) == Decimal.Round(resultRow.Metals.YearEnd, roundingAccuracy);
                bool eqTotals = Decimal.Round(resultRow.TotalYearBegin + resultRow.TotalDeposits + resultRow.TotalInterests + resultRow.TotalTaxes, roundingAccuracy) == Decimal.Round(resultRow.TotalYearEnd, roundingAccuracy);

                if (!eqCash || !eqStocks || !eqMetals || !eqTotals)
                {
                    throw new Exception($"ResultRowValidator: The sum of single values at age {resultRow.Age} does not sum up to total value.");
                }
            }
        }
    }
}
