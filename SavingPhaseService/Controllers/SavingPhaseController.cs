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

        public readonly record struct YearlyRecord(int Age, decimal Amount);
        readonly List<YearlyRecord> result = new();

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

            result.Clear();
            for (int age = savingPhaseStartAge; age < savingPhaseEndAge; age++)
            {
                decimal interests = currentCapital * growthRate / 100m;

                currentCapital += interests;
                decimal savings = saveAmountPerMonth * 12;
                currentCapital += savings;

                result.Add(new YearlyRecord(age, currentCapital));
            }

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
