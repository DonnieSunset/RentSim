using Microsoft.AspNetCore.Mvc;
using RentPhaseService.Clients;
using System.Text.Json;
using System.IO;

namespace RentPhaseService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentPhaseController : ControllerBase
    {
        RentPhase myRentPhase = new RentPhase();
        private readonly IServiceProvider _serviceProvider;
        private IFinanceMathClient myMathClient;

        public RentPhaseController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            myMathClient = _serviceProvider.GetService<IFinanceMathClient>();
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

            var result = myRentPhase.ApproxStateRent(
                ageCurrent, 
                netRentAgeCurrent, 
                ageRentStart, 
                netRentAgeRentStart, 
                ageInQuestion);

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true }); ;
        }

        [HttpGet("Simulate")]
        [Produces("application/json")]
        public async Task<JsonResult> Simulate(
            int ageStart,
            int ageEnd,
            decimal totalRateNeeded_perYear,
            decimal capitalCash, double growthRateCash,
            decimal capitalStocks, double growthRateStocks,
            decimal capitalMetals, double growthRateMetals)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            var result = await myRentPhase.Simulate(
                ageStart,
                ageEnd,
                totalRateNeeded_perYear,
                capitalCash, growthRateCash,
                capitalStocks, growthRateStocks,
                capitalMetals, growthRateMetals,
                myMathClient);

            // This is a workaround!!
            // 'result' delivers a correct json as string. However, if we just sent it here as string (i.e. as IResult),
            // the consumer will receive a JSON string with escape characters, which then is not parsable anymore.
            // So we deserialize and serialize it again :(
            //return new JsonResult(result);
            var jo = JsonSerializer.Deserialize<object>(result);
            return new JsonResult(jo, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}