using Finance.Results;
using Protocol;

namespace Finance
{
    public class RentPhaseCalculator
    {
        public static StateRentResult ApproxStateRent(int ageCurrent, decimal netRentAgeCurrent, int ageRentStart, decimal netRentAgeRentStart, int ageInQuestion)
        {
            var result = new StateRentResult();

            if (ageInQuestion <= ageCurrent || ageInQuestion >= ageRentStart)
            {
                throw new InvalidDataException($"Param: {nameof(ageInQuestion)} is {ageInQuestion} but should be between {ageCurrent} and {ageRentStart}.");
            }

            result.assumedStateRent_FromStopWorkAge_PerMonth = (netRentAgeRentStart - netRentAgeCurrent) / (ageRentStart - ageCurrent) * (ageInQuestion - ageCurrent) + netRentAgeCurrent;
            return result;
        }

        /// <summary>
        /// Todo: carve out, it does not belong to here
        /// </summary>
        public static LaterNeedsResult CalculateLaterNeeds(int ageCurrent, int ageRentStart, double inflationRate, decimal needsCurrentAgeMinimal, decimal needsCurrentAgeComfort, decimal assumedStateRent_FromStopWorkAge_PerMonth)
        {
            var result = new LaterNeedsResult();

            var myRentStartInflation = new Inflation(ageCurrent, ageRentStart, inflationRate);

            result.needsMinimum_AgeRentStart_WithInflation_PerMonth = myRentStartInflation.Calc(needsCurrentAgeMinimal) - assumedStateRent_FromStopWorkAge_PerMonth;
            result.needsComfort_AgeRentStart_WithInflation_PerMonth = myRentStartInflation.Calc(needsCurrentAgeComfort) - assumedStateRent_FromStopWorkAge_PerMonth;

            result.needsMinimum_AgeRentStart_WithInflation_PerYear = result.needsMinimum_AgeRentStart_WithInflation_PerMonth * 12;
            result.needsComfort_AgeRentStart_WithInflation_PerYear = result.needsComfort_AgeRentStart_WithInflation_PerMonth * 12;

            return result;
        }

        public static RentPhaseResult CalculateResult(
           decimal interestRate_Stocks_GoodCase,
           decimal interestRate_Stocks_BadCase,
           decimal InterestRate_Cash,
           int durationInYears,
           decimal comfort_total_needed_Year,
           decimal minimum_total_needed_Year,
           decimal crashFactor_Stocks_BadCase,
           decimal stocks_taxFactor)
        {
            var result = new RentPhaseResult();

            decimal interestFactor_Stocks_GoodCase = interestRate_Stocks_GoodCase + 1;
            decimal interestFactor_Stocks_BadCase = interestRate_Stocks_BadCase + 1;
            decimal interestFactor_Cash = InterestRate_Cash + 1;

            var z_cash = FinanceCalculator.GetZFactorForSparkassenformel(durationInYears, interestFactor_Cash);
            var z_stocks_max = FinanceCalculator.GetZFactorForSparkassenformel(durationInYears, interestFactor_Stocks_GoodCase) * stocks_taxFactor;
            var z_stocks_min = FinanceCalculator.GetZFactorForSparkassenformel(durationInYears, interestFactor_Stocks_BadCase) * stocks_taxFactor;

            // Calculate the results
            result.rate_Cash = (comfort_total_needed_Year * crashFactor_Stocks_BadCase * z_stocks_max - minimum_total_needed_Year * z_stocks_min) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min);
            result.rateStocks_IncludedTaxes_GoodCase = z_stocks_min * (-comfort_total_needed_Year + minimum_total_needed_Year) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min) * stocks_taxFactor;
            result.rateStocks_IncludedTaxes_BadCase = crashFactor_Stocks_BadCase * z_stocks_max * (-comfort_total_needed_Year + minimum_total_needed_Year) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min) * stocks_taxFactor;
            result.total_Cash = z_cash * (comfort_total_needed_Year * crashFactor_Stocks_BadCase * z_stocks_max - minimum_total_needed_Year * z_stocks_min) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min);
            result.total_Stocks = z_stocks_max * z_stocks_min * (-comfort_total_needed_Year + minimum_total_needed_Year) / (crashFactor_Stocks_BadCase * z_stocks_max - z_stocks_min);

            result.taxesPerYear_GoodCase = result.rateStocks_IncludedTaxes_GoodCase * (1 - 1 / stocks_taxFactor);
            result.taxesPerYear_BadCase = result.rateStocks_IncludedTaxes_BadCase * (1 - 1 / stocks_taxFactor);

            result.rateStocks_ExcludedTaxes_GoodCase = result.rateStocks_IncludedTaxes_GoodCase - result.taxesPerYear_GoodCase;
            result.rateStocks_ExcludedTaxes_BadCase = result.rateStocks_IncludedTaxes_BadCase - result.taxesPerYear_BadCase;

            return result;
        }

        public static void Simulate(
            int rentPhaseStartAge, 
            int rentPhaseEndAge, 
            decimal totalCash, 
            decimal totalStocks, 
            decimal rateCash_perYear, 
            decimal rateStocks_ExcludedTaxes_perYear, 
            decimal interestRate_Stocks, 
            decimal interestRate_Cash, 
            decimal taxesPerYear, 
            IProtocolWriter protocolWriter)
        {
            //Console.WriteLine(Environment.NewLine);
            for (int i = rentPhaseStartAge; i < rentPhaseEndAge; i++)
            {
                //Console.WriteLine($"State Begin Year {i}: Cash: {totalCash:F2} Stocks: {totalStocks:F2} Total: {totalStocks + totalCash:F2}");
                protocolWriter.LogBalanceYearBegin(i, totalCash, totalStocks, 0);

                var interests_Cash = totalCash * interestRate_Cash;
                var interests_Stocks = totalStocks * interestRate_Stocks;
                totalCash += interests_Cash;
                totalStocks += interests_Stocks;
                //Console.WriteLine($"\tInterests: Cash: {interests_Cash:F2} Stocks: {interests_Stocks:F2} Total: {interests_Cash + interests_Stocks:F2}");
                protocolWriter.Log(i, new TransactionDetails() { cashInterests = interests_Cash, stockInterests = interests_Stocks });

                totalCash -= rateCash_perYear;
                totalStocks -= rateStocks_ExcludedTaxes_perYear;
                //Console.WriteLine($"\tWithdrawal: Cash: {interestRate_Cash:F2} Stocks: {rateStocks_ExcludedTaxes_perYear:F2} Total: {rateCash_perYear + rateStocks_ExcludedTaxes_perYear:F2}");
                protocolWriter.Log(i, new TransactionDetails() { cashWithdrawal = rateCash_perYear, stockWithdrawal = rateStocks_ExcludedTaxes_perYear });

                totalStocks -= taxesPerYear;
                //Console.WriteLine($"\tWithdrawal: taxes: {taxesPerYear:F2}");
                protocolWriter.Log(i, new TransactionDetails() { stockTaxes = taxesPerYear });

                //Console.WriteLine($"\tEnd Year {i}: Cash: {totalCash:F2} Stocks: {totalStocks:F2} Total: {totalStocks + totalCash:F2}");
            }
        }
    }
}
