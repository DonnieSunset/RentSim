using Microsoft.AspNetCore.Mvc;
using StopWorkPhaseService.Clients;
using StopWorkPhaseService.DTOs;
using System.Text.Json;

namespace StopWorkPhaseService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StopWorkPhaseController : ControllerBase
    {
        private readonly ILogger<StopWorkPhaseController> myLogger;
        private readonly IHttpClientFactory myHttpClientFactory;
        private readonly IServiceProvider myServiceProvider;

        public StopWorkPhaseController(IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory, ILogger<StopWorkPhaseController> logger)
        {
            myServiceProvider = serviceProvider;
            myHttpClientFactory = httpClientFactory;
            myLogger = logger;
        }

        [HttpPost("Simulate")]
        [Produces("application/json")]
        public async Task<JsonResult> Simulate(StopWorkPhaseServiceInputDTO input)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            StopWorkPhaseServiceResultDTO result;
            try
            {
                result = await myServiceProvider.GetService<IStopWorkPhase>().Simulate(
                               input,
                               myServiceProvider.GetService<IFinanceMathClient>());
            }
            catch (Exception ex)
            {
                result = new StopWorkPhaseServiceResultDTO();
                result.Result.Type = ResultDTO.ResultType.Failure;
                result.Result.Message = ex.Message;
            }

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}