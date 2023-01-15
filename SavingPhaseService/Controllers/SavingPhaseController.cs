using Finance;
using Microsoft.AspNetCore.Mvc;
using SavingPhaseService.Contracts;
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
        [Produces("application/json")]
        public async Task<JsonResult> CalculateAsync(
            int ageFrom,
            int ageTo,
            decimal startCapital,
            int growthRate,
            decimal saveAmountPerMonth)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            decimal result = await SavingPhaseService.Calculate(
                ageFrom,
                ageTo,
                startCapital,
                growthRate,
                saveAmountPerMonth);

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true }); ;
        }

        [HttpGet("Simulate")]
        [Produces("application/json")]
        public JsonResult Simulate(
            int ageFrom,
            int ageTo,
            decimal startCapital,
            int growthRate,
            decimal saveAmountPerMonth)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            SimulationResult result = SavingPhaseService.Simulate(
                ageFrom,
                ageTo,
                startCapital,
                growthRate,
                saveAmountPerMonth);

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
