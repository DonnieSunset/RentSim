using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processing;
using System;

namespace Processing_uTest
{
    [TestClass]
    public class SparkassenFormelTests
    {
        // stopWorkAge = 60; rentAge = 67; endAge=75
        // stopWorkPhase = 67-60=7; rentPhase = 75-67=8
        //
        // 60  61  62  63  64  65  66  67  68  69  70  71  72  73  74 75
        // |_1_|_2_|_3_|_4_|_5_|_6_|_7_|_1_|_2_|_3_|_4_|_5_|_6_|_7_|_8_|
        //
        //
        //TODO: das ist jetzt brutto, da muss noch die kapitalertragssteuer weg
        //TODO: testen bei kleinem anfangskapital wo die rente einfach garnicht erst erriecht werden kann
        //TODO: das klappt jetzt mit vorschüssigem zins, aber will ich das nicht eigneltich mit nachschüssigem zins? oder konfigurierbar?
        [TestMethod]
        public void BerechneRateMitRente_ReasonableValues_RatesLeadToEndKapital()
        {
            double anfangskapital = 600000;
            int anzahlJahreStopWorkAge = 7;
            int anzahlJahreRent = 13;
            double jahreszins = 8;
            double endKapital = 0;
            double rente = 1800;

            

            (double rateRent, double rateStopWork) = SparkassenFormel.BerechneRateMitRente(anfangskapital, anzahlJahreStopWorkAge, anzahlJahreRent, jahreszins, endKapital, rente);
            Assert.AreEqual(rente, rateStopWork - rateRent, 0.1);

            double aktuellesKapital = anfangskapital;
            for (int i = 1; i <= anzahlJahreStopWorkAge; i++)
            {
                aktuellesKapital -= rateStopWork * 12;
                aktuellesKapital *= 1 + (jahreszins / 100d);
                Console.WriteLine($"jahr {i} Aktuelles Kapital: {aktuellesKapital}");
            }
            for (int j = 1; j <= anzahlJahreRent; j++)
            {
                aktuellesKapital -= rateRent * 12;
                aktuellesKapital *= 1 + (jahreszins / 100d);
                Console.WriteLine($"jahr {j} Aktuelles Kapital: {aktuellesKapital}");
            }

            Assert.AreEqual(endKapital, aktuellesKapital, 0.01);
        }
    }
}
