using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processing;
using System;

namespace Processing_uTest
{
    [TestClass]
    public class SparkassenFormelTests
    {
        //TODO: das ist jetzt brutto, da muss noch die kapitalertragssteuer weg
        //TODO: testen bei kleinem anfangskapital wo die rente einfach garnicht erst erriecht werden kann
        //TODO: das klappt jetzt mit vorschüssigem zins, aber will ich das nicht eigneltich mit nachschüssigem zins? oder konfigurierbar?
        [TestMethod]
        public void BerechneRateMitRente_ReasonableValues_RatesLeadToEndKapital()
        {
            double anfangskapital = 120000;
            int anzahlJahreStopWorkAge = 7;
            int anzahlJahreRent = 13;
            double jahreszins = 8;
            double endKapital = 0;
            double rente = 1800;


            (double rateRent, double rateStopWork) = SparkassenFormel.BerechneRateMitRente(anfangskapital, anzahlJahreStopWorkAge, anzahlJahreRent, jahreszins, endKapital, rente);
            Assert.AreEqual(rente, rateStopWork - rateRent, 0.01);

            double aktuellesKapital = anfangskapital;
            for (int i = 1; i <= anzahlJahreStopWorkAge; i++)
            {
                aktuellesKapital *= 1 + (jahreszins / 100d);
                aktuellesKapital -= rateStopWork * 12;
                Console.WriteLine($"jahr {i} Aktuelles Kapital: {aktuellesKapital}");
            }
            for (int j = 1; j <= anzahlJahreRent; j++)
            {
                aktuellesKapital *= 1 + (jahreszins / 100d);
                aktuellesKapital -= rateRent * 12;
                Console.WriteLine($"jahr {j} Aktuelles Kapital: {aktuellesKapital}");
            }

            Assert.AreEqual(endKapital, aktuellesKapital, 0.01);
        }
    }
}
