namespace Domain
{
    public class ResultRowValidator
    {
        private static int roundingAccuracy = 2;

        public static void ValidateAll(IEnumerable<ResultRow> resultRows, int ageCurrent, int ageStopWork, int ageEnd/*, decimal stocksTaxFactor*/)
        {
            AllAgesAvailable(resultRows, ageCurrent, ageEnd);
            TransitionBetweenRows(resultRows, ageCurrent, ageEnd);
            AllEndsUpInZero(resultRows, ageEnd);
            AllNumbersHaveTheCorrectSign(resultRows);
            EndTotalsAreTheSumOfAllSingleValues(resultRows);
            //NoMetalsAfterSavingPhase(resultRows, ageStopWork);

            //TaxesArePaidAccordingToDeposits(resultRows, stocksTaxFactor);
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

                if (Decimal.Round(current.cash.YearEnd, roundingAccuracy) != Decimal.Round(next.cash.YearBegin, roundingAccuracy))
                {
                    throw new Exception($"ResultRowValidator: Cash YearEnd and YearBegin mismatch at index {i} ({current.cash.YearEnd}) and {i + 1} ({next.cash.YearBegin}).");
                }

                if (Decimal.Round(current.stocks.YearEnd, roundingAccuracy) != Decimal.Round(next.stocks.YearBegin, roundingAccuracy))
                {
                    throw new Exception($"ResultRowValidator: Stocks YearEnd and YearBegin mismatch at index {i} ({current.stocks.YearEnd}) and {i + 1} ({next.stocks.YearBegin}).");
                }

                if (Decimal.Round(current.metals.YearEnd, roundingAccuracy) != Decimal.Round(next.metals.YearBegin, roundingAccuracy))
                {
                    throw new Exception($"ResultRowValidator: Metals YearEnd and YearBegin mismatch at index {i} ({current.metals.YearEnd}) and {i + 1} ({next.metals.YearBegin}).");
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
                    Decimal.Round(resultRow.cash.Interests, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.cash.Taxes, roundingAccuracy) > 0 ||
                    Decimal.Round(resultRow.cash.YearBegin, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.cash.YearEnd, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.stocks.Interests, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.stocks.Taxes, roundingAccuracy) > 0 ||
                    Decimal.Round(resultRow.stocks.YearBegin, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.stocks.YearEnd, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.metals.Interests, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.metals.Taxes, roundingAccuracy) > 0 ||
                    Decimal.Round(resultRow.metals.YearBegin, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.metals.YearEnd, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.TotalInterests, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.TotalTaxes, roundingAccuracy) > 0 ||
                    Decimal.Round(resultRow.TotalYearBegin, roundingAccuracy) < 0 ||
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
                bool eqCash = Decimal.Round(resultRow.cash.YearBegin + resultRow.cash.Deposits + resultRow.cash.Interests + resultRow.cash.Taxes, roundingAccuracy) == Decimal.Round(resultRow.cash.YearEnd, roundingAccuracy);
                bool eqStocks = Decimal.Round(resultRow.stocks.YearBegin + resultRow.stocks.Deposits + resultRow.stocks.Interests + resultRow.stocks.Taxes, roundingAccuracy) == Decimal.Round(resultRow.stocks.YearEnd, roundingAccuracy);
                bool eqMetals = Decimal.Round(resultRow.metals.YearBegin + resultRow.metals.Deposits + resultRow.metals.Interests + resultRow.metals.Taxes, roundingAccuracy) == Decimal.Round(resultRow.metals.YearEnd, roundingAccuracy);
                bool eqTotals = Decimal.Round(resultRow.TotalYearBegin + resultRow.TotalDeposits + resultRow.TotalInterests + resultRow.TotalTaxes, roundingAccuracy) == Decimal.Round(resultRow.TotalYearEnd, roundingAccuracy);

                if (!eqCash || !eqStocks || !eqMetals || !eqTotals)
                {
                    throw new Exception($"ResultRowValidator: sum of single values at age {resultRow.Age} does not sum up to total value.");
                }
            }
        }

        //private static void NoMetalsAfterSavingPhase(IEnumerable<ResultRow> resultRows, int ageStopWork)
        //{
        //    var lastSavingPhaseRow = resultRows.Single(x => x.age == ageStopWork - 1);

        //    if (Decimal.Round(lastSavingPhaseRow.metalsYearEnd, 3) != 0)
        //    {
        //        throw new Exception($"ResultRowValidator: After saving phase at age {lastSavingPhaseRow.age} metals should be zero, but was {lastSavingPhaseRow.metalsYearEnd}.");
        //    }
        //}

        //private static void TaxesArePaidAccordingToDeposits(IEnumerable<ResultRow> resultRows, decimal stocksTaxFactor)
        //{
        //    foreach (var resultRow in resultRows)
        //    {
        //        //tax relevant stocks deposits are only sells, not buys!
        //        var stocksDeposits = resultRow.stocks.Deposits.Sum(x => { return x < 0 ? x : 0; });
                
        //        if (stocksDeposits < 0)
        //        {
        //            var actualTaxes = decimal.Round(resultRow.stocks.Taxes, roundingAccuracy);
        //            var assumedTaxes = decimal.Round(stocksDeposits * (stocksTaxFactor - 1), roundingAccuracy);
        //            if (actualTaxes != assumedTaxes)
        //            {
        //                throw new Exception($"ResultRowValidator: Taxes for stocks deposits of {stocksDeposits} at age {resultRow.age} are {actualTaxes} but should be {assumedTaxes}.");
        //            }
        //        }
        //    }
        //}

        //todo: interests are not zero is asset is not zero and iterest factor is positive
    }
}
