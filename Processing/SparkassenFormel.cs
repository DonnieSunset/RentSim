using System;

namespace Processing
{
    public class SparkassenFormel
    {
        /// <summary>
        /// https://de.wikipedia.org/wiki/Sparkassenformel
        /// https://www.onlinemathe.de/forum/Umformung-der-Sparkassenformel
        /// </summary>
        public static double BerechneRate(double anfangskapital, int anzahlJahre, double jahreszins, double endKapital)
        {
            double q = 1 + (jahreszins / 100d);
            double rateNachschuessig = (endKapital - (anfangskapital * Math.Pow(q, anzahlJahre))) * (q-1) / (Math.Pow(q, anzahlJahre) - 1);
            
            double rateVorschuessig = (endKapital - (anfangskapital * Math.Pow(q, anzahlJahre))) * (q - 1)       /      (q * (Math.Pow(q, anzahlJahre) - 1));
            //double rate = 1000;

            return rateVorschuessig;
        }

        public static (double ratePhaseRent, double ratePhaseStopWork) CalculatePayoutRateWithRent(double startCapital, int yearsStopWorkPhase, int yearsRentPhase, double interestRate, double endCapital, double rent)
        {
            double left = 0;
            double right = startCapital;
            double middle = SparkassenFormel.Middle(left, right);

            int i = 0;

            double diff;
            double ratePhaseRent, ratePhaseStopWork;
            double ratePhaseRentperMonth, ratePhaseStopWorkperMonth;

            // Prüfe ob das Anfangskapital überhaupt ausreicht. Angenommen das komplette Kapital würde in der StopWork-Phase
            // verbraucht werden dürfen und die Rate würde immernoch under der erwarteten Rente liegen, dann haben wir
            // zu wenig Kapital um eine gleichmäßige Rente zu erzielen.
            var rateCompleteStopWorkPhase = SparkassenFormel.BerechneRate(startCapital, yearsStopWorkPhase, interestRate, endCapital);
            rateCompleteStopWorkPhase = Math.Round(-rateCompleteStopWorkPhase / 12, 2); //todo: maybe this rounding can be avoided once we migrate from double to decimal
            if (rateCompleteStopWorkPhase < rent)
            {
                throw new Exception($"Vermögen <{startCapital}> mit zu erzielbarem Restkapital von <{endCapital}> reicht nicht zur Mindestrente von <{rent}> aus bei StopWork Phase von <{yearsStopWorkPhase}> Jahren. Erzielbare monatliche Rate wäre maximal <{rateCompleteStopWorkPhase}>.");
            }


            if (yearsStopWorkPhase == 0)
            {
                ratePhaseRent = SparkassenFormel.BerechneRate(startCapital, yearsRentPhase, interestRate, endCapital);
                ratePhaseRentperMonth = -ratePhaseRent / 12;
                return (ratePhaseRentperMonth, 0);
            }

            do
            {
                i++;

                ratePhaseStopWork = SparkassenFormel.BerechneRate(startCapital, yearsStopWorkPhase, interestRate, middle);
                ratePhaseRent = SparkassenFormel.BerechneRate(middle, yearsRentPhase, interestRate, endCapital);

                //TODO: von beiden raten müssen aktien steuern abgezogen werden. am besten eine abstraktion entnahmestrategie, die genau dieses verhältnis berechnen kann
                // IEntnahmestrategie.Get

                //korrektur: auf eine rate muss rente ausaddiert werden
                double ratePhaseRentplusRent = ratePhaseRent - (rent * 12);

                diff = Math.Abs(ratePhaseRentplusRent - ratePhaseStopWork);

                string s = $"ratePhaseRent11: {Math.Round(ratePhaseRent)} ratePhaseStopWork: {Math.Round(ratePhaseStopWork)}";
                Console.WriteLine($"Border left: {Math.Round(left)} middle {Math.Round(middle)} right {Math.Round(right)} ");
                Console.WriteLine(s);

                if (ratePhaseRentplusRent > ratePhaseStopWork)
                {
                    Console.WriteLine("Moving to the right! ------>");
                    left = middle;
                    middle = SparkassenFormel.Middle(left, right);
                }
                else
                {
                    Console.WriteLine("<------ Moving to the left!");
                    right = middle;
                    middle = SparkassenFormel.Middle(left, right);
                }
                Console.WriteLine("");
            }
            while (diff >= 1);

            Console.WriteLine("Geschafft! nach iteration " + i);


            ratePhaseRentperMonth = -ratePhaseRent / 12;
            ratePhaseStopWorkperMonth = -ratePhaseStopWork / 12;
            Console.WriteLine($"Rate pro Monat: {ratePhaseStopWorkperMonth} / {ratePhaseRentperMonth} ... bei rente {rent}  und umschwungpunkt {middle}");

            return (ratePhaseRentperMonth, ratePhaseStopWorkperMonth);
        }

        private static double Middle(double left, double right)
        {
            return left + (right - left) / 2;
        }
    }
}
