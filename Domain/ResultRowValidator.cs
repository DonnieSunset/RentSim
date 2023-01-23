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
            EndTotalsAreTheSUmOfAllSingleValues(resultRows);
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
                var currentRow = resultRows.Single(x => x.age == i);
            }
        }

        private static void TransitionBetweenRows(IEnumerable<ResultRow> resultRows, int ageCurrent, int ageEnd)
        {
            for (int i = ageCurrent; i < ageEnd - 1; i++)
            {
                var current = resultRows.Single(x => x.age == i);
                var next = resultRows.Single(x => x.age == i + 1);

                if (Decimal.Round(current.cashYearEnd, roundingAccuracy) != Decimal.Round(next.cashYearBegin, roundingAccuracy))
                {
                    throw new Exception($"ResultRowValidator: Cash YearEnd and YearBegin mismatch at index {i} ({current.cashYearEnd}) and {i + 1} ({next.cashYearBegin}).");
                }

                if (Decimal.Round(current.stocksYearEnd, roundingAccuracy) != Decimal.Round(next.stocksYearBegin, roundingAccuracy))
                {
                    throw new Exception($"ResultRowValidator: Stocks YearEnd and YearBegin mismatch at index {i} ({current.stocksYearEnd}) and {i + 1} ({next.stocksYearBegin}).");
                }

                if (Decimal.Round(current.metalsYearEnd, roundingAccuracy) != Decimal.Round(next.metalsYearBegin, roundingAccuracy))
                {
                    throw new Exception($"ResultRowValidator: Metals YearEnd and YearBegin mismatch at index {i} ({current.metalsYearEnd}) and {i + 1} ({next.metalsYearBegin}).");
                }

                if (Decimal.Round(current.TotalYearEnd, roundingAccuracy) != Decimal.Round(next.TotalYearBegin, roundingAccuracy))
                {
                    throw new Exception($"ResultRowValidator: Total YearEnd and YearBegin mismatch at index {i} ({current.TotalYearEnd}) and {i + 1} ({next.TotalYearBegin}).");
                }
            }
        }

        private static void AllEndsUpInZero(IEnumerable<ResultRow> resultRows, int ageEnd)
        {
            var totalYearEnd = resultRows.Single(x => x.age == ageEnd-1).TotalYearEnd;

            if (Decimal.Round(totalYearEnd, roundingAccuracy) != 0)
            {
                throw new Exception($"ResultRowValidator: Last row does not end up in zero. Actual value: {totalYearEnd}.");
            }
        }

        private static void AllNumbersHaveTheCorrectSign(IEnumerable<ResultRow> resultRows)
        {
            foreach (var resultRow in resultRows)
            {
                if (resultRow.age < 0 ||
                    Decimal.Round(resultRow.cashInterests, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.cashTaxes, roundingAccuracy) > 0 ||
                    Decimal.Round(resultRow.cashYearBegin, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.cashYearEnd, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.stocksInterests, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.stocksTaxes, roundingAccuracy) > 0 ||
                    Decimal.Round(resultRow.stocksYearBegin, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.stocksYearEnd, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.metalsInterests, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.metalsTaxes, roundingAccuracy) > 0 ||
                    Decimal.Round(resultRow.metalsYearBegin, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.metalsYearEnd, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.TotalInterests, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.TotalTaxes, roundingAccuracy) > 0 ||
                    Decimal.Round(resultRow.TotalYearBegin, roundingAccuracy) < 0 ||
                    Decimal.Round(resultRow.TotalYearEnd, roundingAccuracy) < 0)
                {
                    throw new Exception($"ResultRowValidator: row of age {resultRow.age} contains an entry with wrong sign.");
                }
            }
        }

        private static void EndTotalsAreTheSUmOfAllSingleValues(IEnumerable<ResultRow> resultRows)
        {
            foreach (var resultRow in resultRows)
            {
                bool eqCash = Decimal.Round(resultRow.cashYearBegin + resultRow.cashDeposits.Sum() + resultRow.cashInterests + resultRow.cashTaxes, roundingAccuracy) == Decimal.Round(resultRow.cashYearEnd, roundingAccuracy);
                bool eqStocks = Decimal.Round(resultRow.stocksYearBegin + resultRow.stocksDeposits.Sum() + resultRow.stocksInterests + resultRow.stocksTaxes, roundingAccuracy) == Decimal.Round(resultRow.stocksYearEnd, roundingAccuracy);
                bool eqMetals = Decimal.Round(resultRow.metalsYearBegin + resultRow.metalsDeposits.Sum() + resultRow.metalsInterests + resultRow.metalsTaxes, roundingAccuracy) == Decimal.Round(resultRow.metalsYearEnd, roundingAccuracy);
                bool eqTotals = Decimal.Round(resultRow.TotalYearBegin + resultRow.TotalDeposits + resultRow.TotalInterests + resultRow.TotalTaxes, roundingAccuracy) == Decimal.Round(resultRow.TotalYearEnd, roundingAccuracy);

                if (!eqCash || !eqStocks || !eqMetals || !eqTotals)
                {
                    throw new Exception($"ResultRowValidator: sum of single values at age {resultRow.age} does not sum up to total value.");
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

        private static void TaxesArePaidAccordingToDeposits(IEnumerable<ResultRow> resultRows, decimal stocksTaxFactor)
        {
            foreach (var resultRow in resultRows)
            {
                //tax relevant stocks deposits are only sells, not buys!
                var stocksDeposits = resultRow.stocksDeposits.Sum(x => { return x < 0 ? x : 0; });
                
                if (stocksDeposits < 0)
                {
                    var actualTaxes = decimal.Round(resultRow.stocksTaxes, roundingAccuracy);
                    var assumedTaxes = decimal.Round(stocksDeposits * (stocksTaxFactor - 1), roundingAccuracy);
                    if (actualTaxes != assumedTaxes)
                    {
                        throw new Exception($"ResultRowValidator: Taxes for stocks deposits of {stocksDeposits} at age {resultRow.age} are {actualTaxes} but should be {assumedTaxes}.");
                    }
                }
            }
        }

        //todo: interests are not zero is asset is not zero and iterest factor is positive
    }
}
