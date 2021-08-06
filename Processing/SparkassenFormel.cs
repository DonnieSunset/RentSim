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

        public static (double rateA, double rateB) BerechneRateMitRente(double anfangskapital, int anzahlJahreStopWorkAge, int anzahlJahreRent, double jahreszins, double endKapital, double rente)
        {
            double left = 0;
            double right = anfangskapital;
            double middle = SparkassenFormel.Middle(left, right);

            int i = 0;

            double diff;
            double rateA, rateB;
            double rateAperMonth, rateBperMonth;

            if (anzahlJahreStopWorkAge == 0)
            {
                rateA = SparkassenFormel.BerechneRate(anfangskapital, anzahlJahreRent, jahreszins, endKapital);
                rateAperMonth = -rateA / 12;
                return (rateAperMonth, 0);
            }

            do
            {
                i++;

                rateB = SparkassenFormel.BerechneRate(anfangskapital, anzahlJahreStopWorkAge, jahreszins, middle);
                rateA = SparkassenFormel.BerechneRate(middle, anzahlJahreRent, jahreszins, endKapital);

                double rateAplusRent = rateA - (rente * 12);

                diff = Math.Abs(rateAplusRent - rateB);

                Console.WriteLine($"Border left: {Math.Round(left)} middle {Math.Round(middle)} right {Math.Round(right)} ");
                Console.WriteLine($"rateA: {Math.Round(rateA)} rateB: {Math.Round(rateB)}");

                if (rateAplusRent > rateB)
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


            rateAperMonth = -rateA / 12;
            rateBperMonth = -rateB / 12;
            Console.WriteLine($"Rate pro Monat: {rateBperMonth} / {rateAperMonth} ... bei rente {rente}  und umschwungpunkt {middle}");

            return (rateAperMonth, rateBperMonth);
        }

        private static double Middle(double left, double right)
        {
            return left + (right - left) / 2;
        }
    }
}
