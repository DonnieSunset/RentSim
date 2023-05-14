using FinanceMathService.DTOs;
using System.Diagnostics;

namespace FinanceMathService
{
    internal class FinanceMath : IFinanceMath
    {
        //todo: duplicated in SavingPhase
        public const decimal CAPITAL_YIELDS_TAX_FACTOR = 0.26375m;
        public const decimal TAX_FREE_AMOUNT_PER_YEAR = 1000;

        const int MaxIterations = 100;
        public const decimal Precision = 0.001m;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalAmount"></param>
        /// <param name="stocksCrashFactor">0.8 means that stocks crash to 80%, an stocks amount of 1000€ reduces afterwards to 800€.</param>
        /// <param name="totalAmount_minNeededAfterCrash"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public double NonRiskAssetsNeededInCaseOfRiskAssetCrash(double totalAmount, double stocksCrashFactor, double totalAmount_minNeededAfterCrash)
        {
            if (stocksCrashFactor > 1 || stocksCrashFactor < 0 || totalAmount_minNeededAfterCrash < 0 || totalAmount < 0)
            {
                throw new ArgumentException();
            }

            if (totalAmount < totalAmount_minNeededAfterCrash)
            {
                throw new ArgumentException($"{nameof(totalAmount)} must not be smaller than {nameof(totalAmount_minNeededAfterCrash)}.");
            }

            if (stocksCrashFactor == 0)
            {
                return totalAmount_minNeededAfterCrash;
            }

            if (stocksCrashFactor == 1)
            {
                return 0;
            }

            double lowRiskAmount = (totalAmount_minNeededAfterCrash - (totalAmount * stocksCrashFactor)) / ( 1- stocksCrashFactor);
            double highRiskAmount = totalAmount - lowRiskAmount;

            if (highRiskAmount < 0)
            {
                throw new ArgumentException("Not solvable with parameters...");
            }

            if (lowRiskAmount < 0)
            {
                return 0;
            }

            return lowRiskAmount;
        }

        public SimulationResultDTO RateByNumericalSparkassenformel(RateByNumericalSparkassenformelInputDTO input)
        {
            int numIterations = 0;

            decimal gesamtBetrag = input.StartCapitalCash.Total + input.StartCapitalStocks.Total + input.StartCapitalMetals.Total;
            
            decimal angenommeneRate_min = 0;
            decimal angenommeneRate_max = -gesamtBetrag;
            decimal restBetrag;
            
            decimal angenommeneRate;
            SimulationResultDTO result;

            do
            {
                angenommeneRate = (angenommeneRate_min + angenommeneRate_max) / 2m;

                result = new()
                {
                    FirstYearBeginValues = new SimulationResultDTO.AssetsDTO()
                    {
                        Cash = input.StartCapitalCash.Total,
                        Stocks = input.StartCapitalStocks.Total,
                        Metals = input.StartCapitalMetals.Total,
                    },

                    MonthlyDepositRate = angenommeneRate
                };

                restBetrag = gesamtBetrag;
                //decimal factorCashDyn = input.FractionCash;
                //decimal factorStocksDyn = input.FractionStocks;
                //decimal factorMetalsDyn = input.FractionMetals;

                decimal restAnteil_cash = input.StartCapitalCash.Total;
                CAmount restAnteil_stocks = new CAmount(input.StartCapitalStocks);
                decimal restAnteil_metals = input.StartCapitalMetals.Total;

                for (int i = input.AgeFrom; i < input.AgeTo; i++)
                {
                    decimal factorCashDyn = restAnteil_cash / restBetrag;
                    decimal factorStocksDyn = restAnteil_stocks.Total / restBetrag;
                    decimal factorMetalsDyn = restAnteil_metals / restBetrag;

                    // rate runter
                    decimal rate_cash = factorCashDyn * angenommeneRate;
                    decimal rate_stocks = factorStocksDyn * angenommeneRate;
                    decimal rate_metals = factorMetalsDyn * angenommeneRate;

                    //restBetrag += rate_cash + rate_stocks + rate_metals;

                    restAnteil_cash += rate_cash;
                    restAnteil_stocks.DistributeEqually(rate_stocks);
                    restAnteil_metals += rate_metals;

                    // todo: duplicate code
                    // pay taxes
                    // Step 1) calculate CAmount out of rate_stocks, based on input.
                    //         We assume that we dispose the same fraction of deposits and interests as in the input
                    CAmount rateStocksCAmount = CAmount.From(rate_stocks, restAnteil_stocks);
                    // Step 2) From this CAmount, we can calculate the amount of tax that has to be payed
                    decimal taxes_stocks = 0;
                    decimal taxRelevantInterests = Math.Abs(rateStocksCAmount.FromInterests) - TAX_FREE_AMOUNT_PER_YEAR;
                    if (taxRelevantInterests > 0)
                    {
                        taxes_stocks = taxRelevantInterests * CAPITAL_YIELDS_TAX_FACTOR;
                        taxes_stocks = -taxes_stocks;
                    }
                    // Step 3) We substract this from restBetrag (taxes are negative)
                    restAnteil_stocks.DistributeEqually(taxes_stocks);

                    // zinsen drauf
                    //restAnteil_cash = factorCashDyn * restBetrag;
                    //restAnteil_stocks = factorStocksDyn * restBetrag;
                    //restAnteil_metals = factorMetalsDyn * restBetrag;

                    //decimal zinsen_cash = restAnteil_cash * (input.GrowthRateCash / 100m);
                    //decimal zinsen_stocks = restAnteil_stocks * (input.GrowthRateStocks / 100m);
                    //decimal zinsen_metals = restAnteil_metals * (input.GrowthRateMetals / 100m);

                    decimal zinsen_cash = restAnteil_cash * (input.GrowthRateCash / 100m);
                    decimal zinsen_stocks = restAnteil_stocks.Total * (input.GrowthRateStocks / 100m);
                    decimal zinsen_metals = restAnteil_metals * (input.GrowthRateMetals / 100m);

                    //restAnteil_cash += zinsen_cash;
                    //restAnteil_stocks += zinsen_stocks;
                    //restAnteil_metals += zinsen_metals;

                    restAnteil_cash += zinsen_cash;
                    restAnteil_stocks.FromInterests += zinsen_stocks;
                    restAnteil_metals += zinsen_metals;

                    restBetrag = restAnteil_cash + restAnteil_stocks.Total + restAnteil_metals;

                    result.Entities.Add(
                        new SimulationResultDTO.Entity
                        {
                            Age = i,

                            Deposits = new SimulationResultDTO.AssetsDTO
                            {
                                Cash = rate_cash,
                                Stocks = rate_stocks,
                                Metals = rate_metals,
                            },
                            Interests = new SimulationResultDTO.AssetsDTO
                            {
                                Cash = zinsen_cash,
                                Stocks = zinsen_stocks,
                                Metals = zinsen_metals
                            },
                            Taxes = new SimulationResultDTO.AssetsDTO
                            {
                                Cash = 0,
                                Stocks = taxes_stocks,
                                Metals = 0
                            },
                        }
                    );
                }

                if (restBetrag >= input.EndCapitalTotal)
                {
                    angenommeneRate_min = angenommeneRate;
                }
                else
                {
                    angenommeneRate_max = angenommeneRate;
                }

                if (numIterations++ > MaxIterations)
                {
                    throw new Exception($"Too many iterations: {numIterations}");
                }

                //todo: log
                Console.WriteLine($"{nameof(angenommeneRate)}: {angenommeneRate}  //  {nameof(restBetrag)}: {restBetrag}");

            } while (Math.Abs(restBetrag - input.EndCapitalTotal) > Precision);

            //todo: log
            Console.WriteLine("NumIterations: " + numIterations);
            result.Result.Type = ResultDTO.ResultType.Success;
            return result;
        }

        public SimulationResultDTO StartCapitalByNumericalSparkassenformel(StartCapitalByNumericalSparkassenformelInputDTO input)
        {
            const decimal endbetrag = 0;
            input.Validate();

            decimal angenommenesStartKapital_min = 0;
            decimal angenommenesStartKapital_max = - input.TotalRateNeeded_PerYear * (input.AgeTo - input.AgeFrom) * 2 + endbetrag; // simple heuristic
            decimal angenommenesStartKapital;
            decimal restBetrag;

            int numIterations = 0;
            SimulationResultDTO result;
            do
            {
                result = new SimulationResultDTO();
                angenommenesStartKapital = (angenommenesStartKapital_min + angenommenesStartKapital_max) / 2m;

                restBetrag = angenommenesStartKapital;
                decimal restAnteil_cash = restBetrag * input.FractionCash;
                CAmount restAnteil_stocks = CAmount.From(restBetrag * input.FractionStocks, input.StartCapitalStocks);
                decimal restAnteil_metals = restBetrag * input.FractionMetals;

                result.FirstYearBeginValues = new SimulationResultDTO.AssetsDTO
                {
                    Cash = restAnteil_cash,
                    Stocks = restAnteil_stocks.Total,
                    Metals = restAnteil_metals,
                };
                result.MonthlyDepositRate = input.TotalRateNeeded_PerYear / 12m;

                for (int i = input.AgeFrom; i < input.AgeTo; i++)
                {
                    // Faktoren müssen dynamisch sein, da sich wegen der unterschiedlichen zinsen auch die assetzusammensetzung ändert
                    decimal factorCashDyn = restAnteil_cash / restBetrag;
                    decimal factorStocksDyn = restAnteil_stocks.Total / restBetrag; //hier müssten die taxes eingerechnet werden!
                    decimal factorMetalsDyn = restAnteil_metals / restBetrag;

                    // cash rate muss dynamisch sein, da wegen der unterschiedlichen zinsen auch die assetzusammensetzung schwankt
                    decimal rate_cash = factorCashDyn * input.TotalRateNeeded_PerYear;
                    decimal rate_stocks = factorStocksDyn * input.TotalRateNeeded_PerYear;
                    decimal rate_metals = factorMetalsDyn * input.TotalRateNeeded_PerYear;

                    restAnteil_cash += rate_cash;
                    restAnteil_stocks.DistributeEqually(rate_stocks);
                    restAnteil_metals += rate_metals;

                    // for the last iteration, we have to rebalance the withdrawal fraction, 
                    // since the tax calculation disturbs the balance at the end of one iteration.
                    if (i == input.AgeTo - 1)
                    {
                        if (Math.Abs(restAnteil_cash) > Precision)
                        {
                            restAnteil_stocks.DistributeEqually(restAnteil_cash);
                            rate_cash -= restAnteil_cash;
                            rate_stocks += restAnteil_cash;

                            restAnteil_cash = 0;

                        }
                        if (Math.Abs(restAnteil_metals) > Precision)
                        {
                            restAnteil_stocks.DistributeEqually(restAnteil_metals);
                            rate_metals -= restAnteil_metals;
                            rate_stocks += restAnteil_metals;

                            restAnteil_metals = 0;
                        }
                    }

                    // todo: duplicate code
                    // pay taxes
                    // Step 1) calculate CAmount out of rate_stocks, based on input.
                    //         We assume that we dispose the same fraction of deposits and interests as in the input
                    CAmount rateStocksCAmount = CAmount.From(rate_stocks, restAnteil_stocks);
                    // Step 2) From this CAmount, we can calculate the amount of tax that has to be payed
                    decimal taxes_stocks = 0;
                    decimal taxRelevantInterests = Math.Abs(rateStocksCAmount.FromInterests) - TAX_FREE_AMOUNT_PER_YEAR;
                    if (taxRelevantInterests > 0)
                    {
                        taxes_stocks = taxRelevantInterests * CAPITAL_YIELDS_TAX_FACTOR;
                        taxes_stocks = -taxes_stocks;
                    }

                    // Step 3) We substract this from restBetrag (taxes are negative)
                    restAnteil_stocks.DistributeEqually(taxes_stocks);

                    // zinsen drauf
                    decimal zinsen_cash = restAnteil_cash * (input.GrowthRateCash / 100m);
                    decimal zinsen_stocks = restAnteil_stocks.Total * (input.GrowthRateStocks / 100m);
                    decimal zinsen_metals = restAnteil_metals * (input.GrowthRateMetals / 100m);

                    restAnteil_cash += zinsen_cash;
                    restAnteil_stocks.FromInterests += zinsen_stocks;
                    restAnteil_metals += zinsen_metals;

                    restBetrag = restAnteil_cash + restAnteil_stocks.Total + restAnteil_metals;

                    result.Entities.Add(
                        new SimulationResultDTO.Entity
                        {
                            Age = i,

                            Deposits = new SimulationResultDTO.AssetsDTO
                            {
                                Cash = rate_cash,
                                Stocks = rate_stocks,
                                Metals = rate_metals,
                            },
                            Interests = new SimulationResultDTO.AssetsDTO
                            {
                                Cash = zinsen_cash,
                                Stocks = zinsen_stocks,
                                Metals = zinsen_metals
                            },
                            Taxes = new SimulationResultDTO.AssetsDTO
                            {
                                Cash = 0,
                                Stocks = taxes_stocks,
                                Metals = 0
                            },
                        }
                    );
                }

                if (restBetrag < endbetrag)
                {
                    angenommenesStartKapital_min = angenommenesStartKapital;
                }
                else
                {
                    angenommenesStartKapital_max = angenommenesStartKapital;
                }

                if (numIterations++ > MaxIterations)
                {
                    throw new Exception($"Too many iterations: {numIterations}");
                }
            } 
            while (Math.Abs(restBetrag - endbetrag) > Precision);

            Console.WriteLine("NumIterations: " + numIterations);
            result.Result.Type = ResultDTO.ResultType.Success;
            return result;
        }

        public decimal AmountWithInflation(int ageStart, int ageEnd, decimal amount, double inflationRate)
        {
            double inflationFactor = inflationRate + 1;
            int numYears = ageEnd - ageStart;

            if (inflationFactor < 1 || inflationFactor > 2)
            {
                throw new ArgumentException($"{nameof(inflationFactor)}: {inflationFactor}");
            }

            double finalInflationFactor = Math.Pow(inflationFactor, numYears);
            decimal result = amount * (decimal)finalInflationFactor;

            return result;
        }

        internal decimal CalculateStocksTaxes(decimal withdrawalAmount, CAmount totalAmount)
        {
            // Step 1) calculate CAmount out of rate_stocks, based on input.
            //         We assume that we dispose the same fraction of deposits and interests as in the input
            CAmount rateStocksCAmount = CAmount.From(withdrawalAmount, totalAmount);
            // Step 2) From this CAmount, we can calculate the amount of tax that has to be payed
            decimal taxes_stocks = Math.Max(Math.Abs(rateStocksCAmount.FromInterests) - TAX_FREE_AMOUNT_PER_YEAR, 0) * CAPITAL_YIELDS_TAX_FACTOR;

            return -taxes_stocks;
        }
    }
}
