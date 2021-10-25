using System;

namespace Processing.Withdrawal
{
    /// <summary>
    /// This class provides helper methods for fincanial calculations.
    /// It should not have dependencies to the domain specific classes.
    /// </summary>
    public class SparkassenFormel
    {
        /// <summary>
        /// https://de.wikipedia.org/wiki/Sparkassenformel
        /// https://www.onlinemathe.de/forum/Umformung-der-Sparkassenformel
        /// </summary>
        public static double BerechneRate(double anfangskapital, int anzahlJahre, double jahreszins, double endKapital)
        {
            double q = 1 + (jahreszins / 100d);
            double rateNachschuessig = (endKapital - (anfangskapital * Math.Pow(q, anzahlJahre))) * (q-1) /      (Math.Pow(q, anzahlJahre) - 1);
            double rateVorschuessig  = (endKapital - (anfangskapital * Math.Pow(q, anzahlJahre))) * (q-1) / (q * (Math.Pow(q, anzahlJahre) - 1));

            return rateVorschuessig;
        }

        public static (double ratePhaseRent, double ratePhaseStopWork) CalculateGrossPayoutRateWithRent(double startCapital, int yearsStopWorkPhase, int yearsRentPhase, double interestRate, double endCapital, double rent, Func<double, double> calcTaxes)
        {
            double left = 0;
            double right = startCapital;
            double middle = RentSimMath.Middle(left, right);

            int i = 0;

            double diff;
            double ratePhaseRentNet, ratePhaseStopWorkNet;
            double ratePhaseRentGross, ratePhaseStopWorkGross;
            //double ratePhaseRentperMonth, ratePhaseStopWorkperMonth;

            double taxesRatePhaseStopWork, taxesratePhaseRent;

            //todo: check if startCapital equals the sum of all asset capitals

            // Prüfe ob das Anfangskapital überhaupt ausreicht. Angenommen das komplette Kapital würde in der StopWork-Phase
            // verbraucht werden dürfen und die Rate würde immernoch under der erwarteten Rente liegen, dann haben wir
            // zu wenig Kapital um eine gleichmäßige Rente zu erzielen.
            var rateCompleteStopWorkPhase = SparkassenFormel.BerechneRate(startCapital, yearsStopWorkPhase, interestRate, endCapital);
            rateCompleteStopWorkPhase = Math.Round(-rateCompleteStopWorkPhase / 12d, 2); //todo: maybe this rounding can be avoided once we migrate from double to decimal
            if (rateCompleteStopWorkPhase < rent)
            {
                throw new Exception($"Vermögen <{startCapital}> mit zu erzielbarem Restkapital von <{endCapital}> reicht nicht zur Mindestrente von <{rent}> aus bei StopWork Phase von <{yearsStopWorkPhase}> Jahren. Erzielbare monatliche Rate wäre maximal <{rateCompleteStopWorkPhase}>.");
            }


            //if (yearsStopWorkPhase == 0)
            //{
            //    ratePhaseRent = SparkassenFormel.BerechneRate(startCapital, yearsRentPhase, interestRate, endCapital);
            //    taxesratePhaseRent = calcTaxes(ratePhaseRent);
            //    ratePhaseRent -= taxesratePhaseRent;
            //    ratePhaseRentperMonth = -ratePhaseRent / 12d;
            //    return (ratePhaseRentperMonth, 0);
            //}

            do
            {
                i++;

                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //TODO: zum berechnen muss natürlich die brutto rente genutzt werden, aber als ergebnis müsste man die netto rente
                //zurückliefern, am besten als tuple (nettorate, taxes)
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                ratePhaseStopWorkGross = SparkassenFormel.BerechneRate(startCapital, yearsStopWorkPhase, interestRate, middle);
                ratePhaseRentGross = SparkassenFormel.BerechneRate(middle, yearsRentPhase, interestRate, endCapital);

                //we have to substract taxes from both rates to be realistic
                // be careful, tax results are negative numbers
                taxesRatePhaseStopWork = calcTaxes(ratePhaseStopWorkGross);
                taxesratePhaseRent = calcTaxes(ratePhaseRentGross);

                ratePhaseStopWorkNet = ratePhaseStopWorkGross - taxesRatePhaseStopWork;
                ratePhaseRentNet = ratePhaseRentGross - taxesratePhaseRent;

                //korrektur: auf eine rate muss rente ausaddiert werden
                double ratePhaseRentplusRent = ratePhaseRentNet - (rent * 12);

                diff = Math.Abs(ratePhaseRentplusRent - ratePhaseStopWorkNet);

                if (ratePhaseRentplusRent > ratePhaseStopWorkNet)
                {
                    //Console.WriteLine("Moving to the right! ------>");
                    left = middle;
                    middle = RentSimMath.Middle(left, right);
                }
                else
                {
                    //Console.WriteLine("<------ Moving to the left!");
                    right = middle;
                    middle = RentSimMath.Middle(left, right);
                }
                //Console.WriteLine("");
            }
            while (diff >= 1);

            //Console.WriteLine("Geschafft! nach iteration " + i);


            var ratePhaseRentperMonth = -ratePhaseRentGross / 12d;
            var ratePhaseStopWorkperMonth = -ratePhaseStopWorkGross / 12d;
            Console.WriteLine($"Rate pro Monat: {ratePhaseStopWorkperMonth} / {ratePhaseRentperMonth} ... bei rente {rent}  und umschwungpunkt {middle}");
            Console.WriteLine($"Tax rate {taxesRatePhaseStopWork} / {taxesratePhaseRent}");

            return (ratePhaseRentGross,  ratePhaseStopWorkGross);
        }
    }
}
