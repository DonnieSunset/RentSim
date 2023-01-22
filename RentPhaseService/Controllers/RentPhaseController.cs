using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace RentPhaseService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentPhaseController : ControllerBase
    {
        RentPhase myRentPhase = new RentPhase();

        public RentPhaseController()
        {
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

            var result = myRentPhase.ApproxStateRent(
                ageCurrent, 
                netRentAgeCurrent, 
                ageRentStart, 
                netRentAgeRentStart, 
                ageInQuestion);

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true }); ;
        }
    }
}