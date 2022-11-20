using Microsoft.Extensions.Logging;
using Moq;
using SavingPhaseService.Controllers;

namespace SavingPhaseService_Tests
{
    public class SavingPhaseController_uTests
    {
        [Theory]
        [InlineData(42, 60, 160000, 3, 1000, 553362.51)]
        public void Calculate_BasicInputValues_CorrectCalculation(
            int ageCurrent,
            int ageStopWork,
            decimal startCapital,
            int growthRate,
            decimal saveAmountPerMonth,
            decimal expectedResult)
        {
            var loggerMock = new Mock<ILogger<SavingPhaseController>>();
            var controller = new SavingPhaseController(loggerMock.Object);

            var actualResult = controller.Calculate(
                ageCurrent,
                ageStopWork,
                startCapital,
                growthRate,
                saveAmountPerMonth);

            Assert.Equal(actualResult, expectedResult, 2);
        }
    }
}