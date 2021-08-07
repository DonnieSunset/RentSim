using System;

namespace Processing
{
    public class SparkassenFormel
    {
        public static double BerechneRate(double anfangskapital, int anzahlJahre, double jahreszins, double endKapital)
        {
            double q = 1 + (jahreszins / 100d);
            double rate = (endKapital - anfangskapital * Math.Pow(q, anzahlJahre)) * ((q - 1) / (Math.Pow(q, anzahlJahre) - 1));

            return rate;
        }

        public static (double ratePhaseRent, double ratePhaseStopWork) BerechneRateMitRente(double anfangskapital, int anzahlJahreStopWorkAge, int anzahlJahreRent, double jahreszins, double endKapital, double rente)
        {
            double left = 0;
            double right = anfangskapital;
            double middle = SparkassenFormel.Middle(left, right);

            int i = 0;

            double diff;
            double ratePhaseRent, ratePhaseStopWork;
            double ratePhaseRentperMonth, ratePhaseStopWorkperMonth;

            if (anzahlJahreStopWorkAge == 0)
            {
                ratePhaseRent = SparkassenFormel.BerechneRate(anfangskapital, anzahlJahreRent, jahreszins, endKapital);
                ratePhaseRentperMonth = -ratePhaseRent / 12;
                return (ratePhaseRentperMonth, 0);
            }

            do
            {
                i++;

                ratePhaseStopWork = SparkassenFormel.BerechneRate(anfangskapital, anzahlJahreStopWorkAge, jahreszins, middle);
                ratePhaseRent = SparkassenFormel.BerechneRate(middle, anzahlJahreRent, jahreszins, endKapital);

                //TODO: von beiden raten müssen aktien steuern abgezogen werden. am besten eine abstraktion entnahmestrategie, die genau dieses verhältnis berechnen kann
                // IEntnahmestrategie.Get

                //korrektur: auf eine rate muss rente ausaddiert werden
                double ratePhaseRentplusRent = ratePhaseRent - (rente * 12);

                diff = Math.Abs(ratePhaseRentplusRent - ratePhaseStopWork);

                Console.WriteLine($"Border left: {Math.Round(left)} middle {Math.Round(middle)} right {Math.Round(right)} ");
                Console.WriteLine($"ratePhaseRent: {Math.Round(ratePhaseRent)} ratePhaseStopWork: {Math.Round(ratePhaseStopWork)}");

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
            Console.WriteLine($"Rate pro Monat: {ratePhaseStopWorkperMonth} / {ratePhaseRentperMonth} ... bei rente {rente}  und umschwungpunkt {middle}");

            return (ratePhaseRentperMonth, ratePhaseStopWorkperMonth);
        }

        private static double Middle(double left, double right)
        {
            return left + (right - left) / 2;
        }
    }
}
