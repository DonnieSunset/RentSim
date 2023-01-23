using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

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

            //double lowRiskAmount = (totalAmount_minNeededAfterCrash - totalAmount + (totalAmount * stocksCrashFactor)) / stocksCrashFactor;
            double lowRiskAmount = (totalAmount_minNeededAfterCrash - (totalAmount * stocksCrashFactor)) / ( 1- stocksCrashFactor);

            // wenn stocksCrashFactor=1, heisst uberhaut kein crash, man kann also alles auf aktien setzen
            // lowRiskAmount = (totalAmount_minNeededAfterCrash - totalAmount + (totalAmount * stocksCrashFactor)) / stocksCrashFactor
            // lowRiskAmount = (totalAmount_minNeededAfterCrash - totalAmount + (totalAmount * 1)) / 1
            // lowRiskAmount = (totalAmount_minNeededAfterCrash - totalAmount + totalAmount )
            // lowRiskAmount = totalAmount_minNeededAfterCrash
            // das kann nicht sein, eigentlich müsste 0 rauskommen

            // wenn stocksCrashFactor=0, heisst kompletter crash, man muss also mindestens den mindestbetrag auf lowRisk setzen
            // lowRiskAmount = (totalAmount_minNeededAfterCrash - totalAmount + (totalAmount * stocksCrashFactor)) / stocksCrashFactor
            // lowRiskAmount = (totalAmount_minNeededAfterCrash - totalAmount + (totalAmount * 0)) / 0
            // lowRiskAmount = (totalAmount_minNeededAfterCrash - totalAmount ) / 0
            // lowRiskAmount = (totalAmount_minNeededAfterCrash - totalAmount ) / 0
            // das kann nicht sein, eigentlich müsste totalAmount_minNeededAfterCrash rauskommen


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

        public double RateByNumericalSparkassenformel(List<double> betrag, List<double> zins, double endbetrag, int yearStart, int yearEnd)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            const int MaxIterations = 100;
            const double Precision = 0.001;

            List<object> result = new();

            int numIterations = 0;

            double gesamtBetrag = betrag.Sum();
            double angenommeneRate_min = 0;
            double angenommeneRate_max = gesamtBetrag;
            double restBetrag;
            double angenommeneRate;
            do
            {
                angenommeneRate = (angenommeneRate_min + angenommeneRate_max) / 2d;

                restBetrag = betrag.Sum();
                List<double> faktors = betrag.Select(betrag => betrag / gesamtBetrag).ToList();

                for (int i = yearStart; i < yearEnd; i++)
                {
                    // rate runter
                    //restBetrag -= angenommeneRate;
                    List<double> subRaten = faktors.Select(faktor => faktor * angenommeneRate).ToList();
                    restBetrag -= subRaten.Sum();

                    // zinsen drauf
                    List<double> anteiligeBetraege = faktors.Select(faktor => faktor * restBetrag).ToList();

                    for (int j = 0; j < anteiligeBetraege.Count; j++)
                        anteiligeBetraege[j] *= zins[j];

                    restBetrag = anteiligeBetraege.Sum();

                    //var v = new { Age = i, Rates = subRaten };
                    //result.Add(v);
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
            double factor_cash, 
            double factor_stocks, 
            double factor_metals, 
            double zinsRate_cash, 
            double zinsRate_stocks,
            double zinsRate_metals,
            decimal endbetrag, 
            int yearStart, int yearEnd,
            out List<object> protocol)
        {
            if (Math.Abs(factor_cash + factor_stocks + factor_metals - 1) > 0.0001)
            {
                throw new ArgumentException("Zinsen dont sum up to 1");
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            protocol = new List<object>();

            const int MaxIterations = 100;
            const double Precision = 0.001;

            int numIterations = 0;

            decimal angenommenesStartKapital_min = 0;
            decimal angenommenesStartKapital_max = rateTotal_perYear * (yearEnd - yearStart) * 2 + endbetrag; // simple heuristic
            decimal restBetrag;
            
            decimal angenommenesStartKapital;
            do
            {
                angenommenesStartKapital = (angenommenesStartKapital_min + angenommenesStartKapital_max) / 2m;
                restBetrag = angenommenesStartKapital;
                protocol.Clear();

                for (int i = yearStart; i < yearEnd; i++)
                {
                    // rate runter
                    decimal yearBegin_cash = (decimal)factor_cash * restBetrag;
                    decimal yearBegin_stocks = (decimal)factor_stocks * restBetrag;
                    decimal yearBegin_metals = (decimal)factor_metals * restBetrag;

                    decimal rate_cash = (decimal)factor_cash * rateTotal_perYear;
                    decimal rate_stocks = (decimal)factor_stocks * rateTotal_perYear;
                    decimal rate_metals = (decimal)factor_metals * rateTotal_perYear;

                    restBetrag -= rate_cash + rate_stocks + rate_metals;

                    // zinsen drauf
                    decimal restAnteil_cash = (decimal)factor_cash * restBetrag;
                    decimal restAnteil_stocks = (decimal)factor_stocks * restBetrag;
                    decimal restAnteil_metals = (decimal)factor_metals * restBetrag;

                    decimal zinsen_cash = restAnteil_cash * ((decimal)zinsRate_cash / 100m);
                    decimal zinsen_stocks = restAnteil_stocks * ((decimal)zinsRate_stocks / 100m);
                    decimal zinsen_metals = restAnteil_metals * ((decimal)zinsRate_metals / 100m);

                    restAnteil_cash += zinsen_cash;
                    restAnteil_stocks += zinsen_stocks;
                    restAnteil_metals += zinsen_metals;

                    restBetrag = restAnteil_cash + restAnteil_stocks + restAnteil_metals;

                    var vs = new { 
                        Age = i, 
                        YearBegin = new {
                            Cash = yearBegin_cash, 
                            Stocks = yearBegin_stocks, 
                            Metals = yearBegin_metals
                        },
                        Rates = new { 
                            Cash = rate_cash, 
                            Stocks = rate_stocks, 
                            Metals = rate_metals
                        }, 
                        Zins = new { 
                            Cash = zinsen_cash,
                            Stocks = zinsen_stocks,
                            Metals = zinsen_metals
                        } 
                    };
                    protocol.Add(vs);
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

                //todo: log
                Console.WriteLine($"{nameof(angenommenesStartKapital)}: {angenommenesStartKapital}  //  {nameof(restBetrag)}: {restBetrag}");

            } while (Math.Abs(restBetrag - endbetrag) > (decimal)Precision);

            //todo: log
            Console.WriteLine("Duration: " + sw.Elapsed);
            Console.WriteLine("NumIterations: " + numIterations);
            //Console.WriteLine("SH1: Nach erster iter: " + temp_nachErsterIter);

            return angenommenesStartKapital;
            //return result;
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
