using FinanceMathService.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FinanceMathService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FinanceMathController : ControllerBase
    {
        private IFinanceMath myFinanceMath;

        public FinanceMathController(IFinanceMath financeMath)
        {
            myFinanceMath = financeMath;
        }

        [HttpGet("NonRiskAssets")]
        [Produces("application/json")]
        public JsonResult NonRiskAssets(double totalAmount, double stocksCrashFactor, double totalAmount_minNeededAfterCrash)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            double result = myFinanceMath.NonRiskAssetsNeededInCaseOfRiskAssetCrash(totalAmount, stocksCrashFactor, totalAmount_minNeededAfterCrash);
            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }

        [HttpPost("RateByNumericalSparkassenformel")]
        [Produces("application/json")]
        public JsonResult RateByNumericalSparkassenformel(RateByNumericalSparkassenformelInputDTO input)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            SimulationResultDTO result;
            try
            {
                result = myFinanceMath.RateByNumericalSparkassenformel(input);
            }
            catch (Exception ex)
            {
                result = new SimulationResultDTO();
                result.Result.Type = ResultDTO.ResultType.Failure;
                result.Result.Message = ex.Message;
                result.Result.Details = ex.StackTrace;
            }

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }

        [HttpPost("StartCapitalByNumericalSparkassenformel")]
        [Produces("application/json")]
        public JsonResult StartCapitalByNumericalSparkassenformel(StartCapitalByNumericalSparkassenformelInputDTO input)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            SimulationResultDTO result;
            try
            {
                result = myFinanceMath.StartCapitalByNumericalSparkassenformel(input);
            }
            catch (Exception ex)
            {
                result = new SimulationResultDTO();
                result.Result.Type = ResultDTO.ResultType.Failure;
                result.Result.Message = ex.Message;
                result.Result.Details = ex.StackTrace;
            }

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }

        [HttpGet("AmountWithInflation")]
        [Produces("application/json")]
        public JsonResult AmountWithInflation(int ageStart, int ageEnd, decimal amount, double inflationRate)
        {
            decimal result = myFinanceMath.AmountWithInflation(ageStart, ageEnd, amount, inflationRate);

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}