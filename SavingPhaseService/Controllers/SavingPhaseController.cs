using Microsoft.AspNetCore.Mvc;
using SavingPhaseService.Clients;
using SavingPhaseService.DTOs;
using System.Text.Json;

namespace SavingPhaseService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SavingPhaseController : ControllerBase
    {
        //private readonly ILogger<SavingPhaseController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceProvider _serviceProvider;

        private ISavingPhase mySavingPhase;

        //public SavingPhaseController(ILogger<SavingPhaseController> logger)
        //{
        //    _logger = logger;
        //}

        //public SavingPhaseController(IHttpClientFactory httpClientFactory)
        //{
        //    _httpClientFactory = httpClientFactory;
        //}
        public SavingPhaseController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            mySavingPhase = new SavingPhase();
        }

        [HttpGet("Simulate")]
        [Produces("application/json")]
        public JsonResult Simulate(
            int ageFrom,
            int ageTo,
            decimal startCapital,
            int growthRate,
            decimal saveAmountPerMonth,
            bool capitalYieldsTax
            )
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            SavingPhaseServiceResult result = mySavingPhase.Simulate(
                ageFrom,
                ageTo,
                startCapital,
                growthRate,
                saveAmountPerMonth,
                capitalYieldsTax
                );

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
