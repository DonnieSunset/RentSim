using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SavingPhaseService.Controllers;
using SavingPhaseService;

namespace SavingPhaseService_Tests
{
    public class SavingPhaseController_uTests
    {
        [TestCase(42, 60, 160000, 3, 1000, 553362.51)]
        public void Calculate_BasicInputValues_CorrectCalculation(
            int ageCurrent,
            int ageStopWork,
            decimal startCapital,
            int growthRate,
            decimal saveAmountPerMonth,
            decimal expectedResult)
        {
            //var loggerMock = new Mock<IHttpClientFactory>();
            //var controller = new SavingPhaseController(loggerMock.Object);

            //var svaingPhaseService = new SavingPhaseService();

            var actualResult = SavingPhase.Calculate(
                ageCurrent,
                ageStopWork,
                startCapital,
                growthRate,
                saveAmountPerMonth).Result;

            Assert.That(actualResult, Is.EqualTo(expectedResult).Within(0.01));
        }
    }
}