using Finance;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SavingPhaseService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SavingPhaseController : ControllerBase
    {
        private readonly ILogger<SavingPhaseController> _logger;

        public SavingPhaseController(ILogger<SavingPhaseController> logger)
        {
            _logger = logger;
        }


        [HttpGet("Calculate")]
        public decimal Calculate(
            int ageCurrent,
            int ageStopWork,
            decimal startCapital,
            int growthRate,
            decimal saveAmountPerMonth)
        {
            int duration = ageStopWork - ageCurrent;

            double interestFactor = 1 + (growthRate / 100d);
            decimal result = FinanceCalculator.SparkassenFormel(startCapital, saveAmountPerMonth * 12, interestFactor, duration);

            return result;
        }

        public readonly record struct YearlyRecord(int age, decimal amount);

        [HttpGet("Simulate")]
        [Produces("application/json")]
        public JsonResult Simulate(
            int savingPhaseStartAge,
            int savingPhaseEndAge,
            decimal startCapital,
            int growthRate,
            decimal saveAmountPerMonth)
        {
            decimal currentCapital = startCapital;
            List<YearlyRecord> result = new();

            for (int i = savingPhaseStartAge; i < savingPhaseEndAge; i++)
            {
                decimal interests = currentCapital * growthRate / 100m;

                currentCapital += interests;
                decimal savings = saveAmountPerMonth * 12;
                currentCapital += savings;

                result.Add(new YearlyRecord(i, currentCapital));
            }

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
