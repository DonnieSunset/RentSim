using Moq;
using NUnit.Framework;
using SavingPhaseService;
using SavingPhaseService.Clients;
using SavingPhaseService.DTOs;

namespace SavingPhaseService_Tests
{
    public class SavingPhaseController_uTests
    {
        private ISavingPhase mySavingPhase = null;

        [OneTimeSetUp]
        public void OneTimeSetUpMethod()
        { 
            mySavingPhase = new SavingPhase();
        }

        public static object[] inputs =
        {
            new object[]
            {
                new SavingPhaseServiceInputDTO
                {
                    AgeFrom = 40,
                    AgeTo = 50,

                    StartCapitalCash = { FromDeposits = 100000 },
                    GrowthRateCash = 1,
                    SaveAmountPerMonthCash = 300,

                    StartCapitalStocks = { FromDeposits = 0 },
                    GrowthRateStocks = 0,
                    SaveAmountPerMonthStocks = 0,

                    StartCapitalMetals = { FromDeposits = 0 },
                    GrowthRateMetals = 0,
                    SaveAmountPerMonthMetals = 0,

                },
                147702.77m
            }
        };

        [TestCaseSource(nameof(inputs))]
        public void Simulate_BasicInputValues_CorrectSavingResult(SavingPhaseServiceInputDTO input, decimal expectedResult)
        {
            //some random number returned from mock, just to get any result
            var mockResult = 553362;

            var mockedFinanceMathClient = new Mock<IFinanceMathClient>();
            mockedFinanceMathClient
                .Setup(x => x.GetSparkassenFormelAsync(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<double>(), It.IsAny<int>()))
                .ReturnsAsync(mockResult);

            var actualResult = mySavingPhase.Simulate(input);

            Assert.That(actualResult, Is.Not.Null);
            Assert.That(actualResult, Is.TypeOf<SavingPhaseServiceResultDTO>());
        }
    }
}