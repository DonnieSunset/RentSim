using Moq;
using NUnit.Framework;
using SavingPhaseService;
using SavingPhaseService.Clients;
using SavingPhaseService.DTOs;

namespace SavingPhaseService_Tests
{
    public class SavingPhaseController_sTests
    {
        public static object[] inputs =
        {
            new object[]
            {
                new SavingPhaseServiceInputDTO
                {
                    AgeFrom = 40,
                    AgeTo = 50,

                    StartCapitalCash = 100000,
                    GrowthRateCash = 1,
                    SaveAmountPerMonthCash = 300,

                    StartCapitalStocks = 180000,
                    GrowthRateStocks = 8,
                    SaveAmountPerMonthStocks = 717,

                    StartCapitalMetals = 12000,
                    GrowthRateMetals = 0,
                    SaveAmountPerMonthMetals = 0,

                },
                147828.35m + 523220.27m + 12000m
            }
        };

        /// <summary>
        /// https://www.zinsen-berechnen.de/sparrechner.php
        /// Sparintervall: Jährlich
        /// Einzahlungsart: Vorschüssig
        /// </summary>
        [TestCaseSource(nameof(inputs))]
        public void Simulate_BasicInputValues_CorrectSavingResult(SavingPhaseServiceInputDTO input, decimal expectedResult)
        {
            var actualResult = new SavingPhase().Simulate(input);

            Assert.That(actualResult.FinalSavings, Is.EqualTo(expectedResult).Within(0.01));
        }
    }
}