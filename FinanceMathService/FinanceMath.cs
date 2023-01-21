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

        public double RateByNumericalSparkassenformel(List<double> betrag, List<double> zins, double endbetrag, int jahre)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            const int MaxIterations = 100;
            const double Precision = 0.001;

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

                for (int i = 0; i < jahre; i++)
                {
                    // rate runter
                    restBetrag -= angenommeneRate;

                    // zinsen drauf
                    List<double> anteiligeBetraege = faktors.Select(faktor => faktor * restBetrag).ToList();

                    for (int j = 0; j < anteiligeBetraege.Count; j++)
                        anteiligeBetraege[j] *= zins[j];

                    restBetrag = anteiligeBetraege.Sum();
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
