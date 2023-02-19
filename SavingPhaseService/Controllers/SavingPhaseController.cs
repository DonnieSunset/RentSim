using Microsoft.AspNetCore.Mvc;
using SavingPhaseService.Clients;
using SavingPhaseService.DTOs;
using System.Net.Http;
using System.Text.Json;

namespace SavingPhaseService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SavingPhaseController : ControllerBase
    {
        private readonly ILogger<SavingPhaseController> myLogger;
        private readonly IHttpClientFactory myHttpClientFactory;
        private readonly IServiceProvider myServiceProvider;

        public SavingPhaseController(IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory, ILogger<SavingPhaseController> logger)
        {
            myServiceProvider = serviceProvider;
            myHttpClientFactory = httpClientFactory;
            myLogger = logger;
        }

        [HttpPost("Simulate")]
        [Produces("application/json")]
        public JsonResult Simulate(SavingPhaseServiceInputDTO input)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            return new JsonResult(
                myServiceProvider.GetService<ISavingPhase>().Simulate(input), 
                new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
