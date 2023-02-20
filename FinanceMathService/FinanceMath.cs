using FinanceMathService.DTOs;
using System.Diagnostics;

namespace FinanceMathService
{
    internal class FinanceMath : IFinanceMath
    {
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

        public decimal RateByNumericalSparkassenformel(
            decimal betrag_cash,
            decimal betrag_stocks,
            decimal betrag_metals,
            decimal zins_cash,
            decimal zins_stocks,
            decimal zins_metals,
            decimal endbetrag, 
            int yearStart, int yearEnd,
            out SimulationResultDTO result)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            const int MaxIterations = 100;
            const decimal Precision = 0.001m;

            int numIterations = 0;

            decimal gesamtBetrag = betrag_cash + betrag_stocks + betrag_metals;
            
            decimal angenommeneRate_min = 0;
            decimal angenommeneRate_max = -gesamtBetrag;
            decimal restBetrag;
            
            decimal angenommeneRate;
            do
            {
                result = new SimulationResultDTO()
                {
                    FirstYearBeginValues = new SimulationResultDTO.AssetsDTO()
                    {
                        Cash = betrag_cash,
                        Stocks = betrag_stocks,
                        Metals = betrag_metals,
                    }
                };
                angenommeneRate = (angenommeneRate_min + angenommeneRate_max) / 2m;

                restBetrag = betrag_cash + betrag_stocks + betrag_metals;
                decimal factorCashDyn = betrag_cash / gesamtBetrag;
                decimal factorStocksDyn = betrag_stocks / gesamtBetrag;
                decimal factorMetalsDyn = betrag_metals / gesamtBetrag;

                decimal restAnteil_cash = factorCashDyn * restBetrag;
                decimal restAnteil_stocks = factorStocksDyn * restBetrag;
                decimal restAnteil_metals = factorMetalsDyn * restBetrag;


                for (int i = yearStart; i < yearEnd; i++)
                {
                    // rate runter
                    decimal yearBegin_cash = restAnteil_cash;
                    decimal yearBegin_stocks = restAnteil_stocks;
                    decimal yearBegin_metals = restAnteil_metals;

                    decimal rate_cash = factorCashDyn * angenommeneRate;
                    decimal rate_stocks = factorStocksDyn * angenommeneRate;
                    decimal rate_metals = factorMetalsDyn * angenommeneRate;

                    restBetrag += rate_cash + rate_stocks + rate_metals;

                    // zinsen drauf
                    restAnteil_cash = factorCashDyn * restBetrag;
                    restAnteil_stocks = factorStocksDyn * restBetrag;
                    restAnteil_metals = factorMetalsDyn * restBetrag;

                    decimal zinsen_cash = restAnteil_cash * (zins_cash / 100m);
                    decimal zinsen_stocks = restAnteil_stocks * (zins_stocks / 100m);
                    decimal zinsen_metals = restAnteil_metals * (zins_metals / 100m);

                    restAnteil_cash += zinsen_cash;
                    restAnteil_stocks += zinsen_stocks;
                    restAnteil_metals += zinsen_metals;

                    restBetrag = restAnteil_cash + restAnteil_stocks + restAnteil_metals;

                    factorCashDyn = restAnteil_cash / restBetrag;
                    factorStocksDyn = restAnteil_stocks / restBetrag;
                    factorMetalsDyn = restAnteil_metals / restBetrag;

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
                                Stocks = 0,
                                Metals = 0
                            },
                        }
                    );
                }

                if (restBetrag >= endbetrag)
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

            } while (Math.Abs(restBetrag - endbetrag) > Precision);

            //todo: log
            Console.WriteLine("Duration: " + sw.Elapsed);
            Console.WriteLine("NumIterations: " + numIterations);
            return angenommeneRate;
        }

        public decimal StartCapitalByNumericalSparkassenformel(
            decimal rateTotal_perYear,
            decimal betrag_cash,
            decimal betrag_stocks,
            decimal betrag_metals,
            decimal zinsRate_cash,
            decimal zinsRate_stocks,
            decimal zinsRate_metals,
            decimal endbetrag, 
            int yearStart, int yearEnd,
            out SimulationResultDTO result)
        {
            decimal gesamtBetrag = betrag_cash + betrag_stocks + betrag_metals;
            decimal factor_cash = betrag_cash / gesamtBetrag;
            decimal factor_stocks = betrag_stocks / gesamtBetrag;
            decimal factor_metals = betrag_metals / gesamtBetrag;

            if (Math.Abs(factor_cash + factor_stocks + factor_metals - 1m) > 0.0001m)
            {
                throw new ArgumentException("Zinsen dont sum up to 1");
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            const int MaxIterations = 100;
            const double Precision = 0.001;

            int numIterations = 0;

            decimal angenommenesStartKapital_min = 0;
            decimal angenommenesStartKapital_max = -rateTotal_perYear * (yearEnd - yearStart) * 2 + endbetrag; // simple heuristic
            decimal restBetrag;
            
            decimal angenommenesStartKapital;
            do
            {
                result = new SimulationResultDTO();
                angenommenesStartKapital = (angenommenesStartKapital_min + angenommenesStartKapital_max) / 2m;

                restBetrag = angenommenesStartKapital;
                decimal restAnteil_cash = (decimal)factor_cash * restBetrag;
                decimal restAnteil_stocks = (decimal)factor_stocks * restBetrag;
                decimal restAnteil_metals = (decimal)factor_metals * restBetrag;

                result.FirstYearBeginValues = new SimulationResultDTO.AssetsDTO
                {
                    Cash = restAnteil_cash,
                    Stocks = restAnteil_stocks,
                    Metals = restAnteil_metals,
                };

                //Faktoren müssen dynamisch sein, da aich wegen der unterschiedlichen zinsen auch die assetzusammensetzung ändert
                decimal factorCashDyn = factor_cash;
                decimal factorStocksDyn = factor_stocks;
                decimal factorMetalsDyn = factor_metals;

                for (int i = yearStart; i < yearEnd; i++)
                {
                    //// rate runter
                    //decimal yearBegin_cash = restAnteil_cash;
                    //decimal yearBegin_stocks = restAnteil_stocks;
                    //decimal yearBegin_metals = restAnteil_metals;

                    //cash rate muss dynamisch sein, da wegen der unterschiedlichen zinsen auch die assetzusammensetzung schwankt
                    decimal rate_cash = (decimal)factorCashDyn * rateTotal_perYear;
                    decimal rate_stocks = (decimal)factorStocksDyn * rateTotal_perYear;
                    decimal rate_metals = (decimal)factorMetalsDyn * rateTotal_perYear;

                    restAnteil_cash += rate_cash;
                    restAnteil_stocks += rate_stocks;
                    restAnteil_metals += rate_metals;

                    // zinsen drauf
                    decimal zinsen_cash = restAnteil_cash * (zinsRate_cash / 100m);
                    decimal zinsen_stocks = restAnteil_stocks * (zinsRate_stocks / 100m);
                    decimal zinsen_metals = restAnteil_metals * (zinsRate_metals / 100m);

                    restAnteil_cash += zinsen_cash;
                    restAnteil_stocks += zinsen_stocks;
                    restAnteil_metals += zinsen_metals;

                    restBetrag = restAnteil_cash + restAnteil_stocks + restAnteil_metals;

                    factorCashDyn = restAnteil_cash / restBetrag;
                    factorStocksDyn = restAnteil_stocks / restBetrag;
                    factorMetalsDyn = restAnteil_metals / restBetrag;

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
                                Stocks = 0,
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
            } while (Math.Abs(restBetrag - endbetrag) > (decimal)Precision);

            Console.WriteLine("Duration: " + sw.Elapsed);
            Console.WriteLine("NumIterations: " + numIterations);

            return angenommenesStartKapital;
        }

        public decimal SparkassenFormel(decimal anfangskapital, decimal rate_proJahr, double zinsFaktor, int anzahlJahre)
        {
            decimal zinsFaktor_d = (decimal)zinsFaktor;

            decimal endKapital;
            if (zinsFaktor_d == 1)
            {
                endKapital = anfangskapital + rate_proJahr * anzahlJahre;
            }
            else
            {
                endKapital = anfangskapital * Pow(zinsFaktor_d, anzahlJahre) + (rate_proJahr * 1 * ((Pow(zinsFaktor_d, anzahlJahre)) - 1) / (zinsFaktor_d - 1));
            }

            return endKapital;
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

        private static decimal Pow(decimal a, int b)
        {
            return (decimal)Math.Pow((double)a, b);
        }
    }
}
