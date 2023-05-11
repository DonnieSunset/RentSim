using Microsoft.AspNetCore.Mvc;
using RentPhaseService.Clients;
using RentPhaseService.DTOs;
using System.Text.Json;

namespace RentPhaseService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentPhaseController : ControllerBase
    {
        private readonly ILogger<RentPhaseController> myLogger;
        private readonly IHttpClientFactory myHttpClientFactory;
        private readonly IServiceProvider myServiceProvider;

        public RentPhaseController(IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory, ILogger<RentPhaseController> logger)
        {
            myServiceProvider = serviceProvider;
            myHttpClientFactory = httpClientFactory;
            myLogger = logger;
        }

        [HttpGet("ApproxStateRent")]
        [Produces("application/json")]
        public JsonResult ApproxStateRent(
            int ageCurrent,
            decimal netRentAgeCurrent,
            int ageRentStart,
            decimal netRentAgeRentStart,
            int ageInQuestion)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            HttpContext.Response.Headers.Add("Content-Type", "application/json");

            var result = myServiceProvider.GetService<IRentPhase>().ApproxStateRent(
                ageCurrent, 
                netRentAgeCurrent, 
                ageRentStart, 
                netRentAgeRentStart, 
                ageInQuestion);

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true }); ;
        }

        [HttpPost("Simulate")]
        [Produces("application/json")]
        public async Task<JsonResult> Simulate(RentPhaseServiceInputDTO input)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            RentPhaseServiceResultDTO result;
            try
            {
                result = await myServiceProvider.GetService<IRentPhase>().Simulate(
                    input,
                    myServiceProvider.GetService<IFinanceMathClient>());
            }
            catch (Exception ex)
            {
                result = new RentPhaseServiceResultDTO();
                result.Result.Type = ResultDTO.ResultType.Failure;
                result.Result.Message = ex.Message;
                result.Result.Details = ex.StackTrace;
            }

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}