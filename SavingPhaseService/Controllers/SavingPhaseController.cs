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
        public JsonResult Calculate(
            int ageCurrent,
            int ageStopWork,
            decimal startCapital,
            int growthRate,
            decimal saveAmountPerMonth)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            int duration = ageStopWork - ageCurrent;

            double interestFactor = 1 + (growthRate / 100d);
            decimal result = FinanceCalculator.SparkassenFormel(startCapital, saveAmountPerMonth * 12, interestFactor, duration);

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true }); ;
        }

        public readonly record struct YearlyRecord(int Age, decimal Amount);

        [HttpGet("Simulate")]
        [Produces("application/json")]
        public JsonResult Simulate(
            int savingPhaseStartAge,
            int savingPhaseEndAge,
            decimal startCapital,
            int growthRate,
            decimal saveAmountPerMonth)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            List<YearlyRecord> result = new();
            decimal currentCapital = startCapital;

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
