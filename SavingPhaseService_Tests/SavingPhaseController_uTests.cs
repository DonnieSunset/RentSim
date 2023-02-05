using Moq;
using NUnit.Framework;
using SavingPhaseService;
using SavingPhaseService.Clients;

namespace SavingPhaseService_Tests
{
    public class SavingPhaseController_uTests
    {
        [TestCase(42, 60, 160000, 3, 1000, 553362.51)]
        public void Simulate_BasicInputValues_CorrectSavingResult(
            int ageCurrent,
            int ageStopWork,
            decimal startCapital,
            int growthRate,
            decimal saveAmountPerMonth,
            decimal expectedResult)
        {
            var savingPhase = new SavingPhase();

            var mockedFinanceMathClient = new Mock<IFinanceMathClient>();
            mockedFinanceMathClient
                .Setup(x => x.GetSparkassenFormelAsync(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<double>(), It.IsAny<int>()))
                .ReturnsAsync(553362.514294344m);

            var actualResult = savingPhase.Simulate(
                ageCurrent,
                ageStopWork,
                startCapital,
                growthRate,
                saveAmountPerMonth,
                false
                );

            Assert.That(actualResult.FinalSavings, Is.EqualTo(expectedResult).Within(0.01));
        }
    }
}