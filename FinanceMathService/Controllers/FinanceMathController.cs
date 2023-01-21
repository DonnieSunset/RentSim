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

        [HttpGet("RateByNumericalSparkassenformel")]
        [Produces("application/json")]
        public JsonResult RateByNumericalSparkassenformel(double betrag1, double zins1, double betrag2, double zins2, double endbetrag, int jahre)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            double result = myFinanceMath.RateByNumericalSparkassenformel(
                new List<double> { betrag1, betrag2 },
                new List<double> { zins1, zins2 },
                endbetrag,
                jahre
            );

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }

        //[HttpGet("RateByNumericalSparkassenformel")]
        //[Produces("application/json")]
        //public JsonResult RateByNumericalSparkassenformel(double betrag1, double zins1, double betrag2, double zins2, double betrag3, double zins3, double endbetrag, int jahre)
        //{
        //    HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

        //    double result = FinanceMath.RateByNumericalSparkassenformel(
        //        new List<double> { betrag1, betrag2, betrag3 },
        //        new List<double> { zins1, zins2, zins3 },
        //        endbetrag,
        //        jahre
        //    );

        //    return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        //}

        [HttpGet("Sparkassenformel")]
        [Produces("application/json")]
        public JsonResult SparkassenFormel(decimal anfangskapital, decimal rateProJahr, double zinsFaktor, int anzahlJahre)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            decimal result = myFinanceMath.SparkassenFormel(anfangskapital, rateProJahr, zinsFaktor, anzahlJahre);

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }

        [HttpGet("AmountWithInflation")]
        [Produces("application/json")]
        public JsonResult AmountWithInflation(int ageStart, int ageEnd, decimal amount, double inflationRate)
        {
            double inflationFactor = inflationRate + 1;
            int numYears = ageEnd - ageStart;

            if (inflationFactor < 1 || inflationFactor > 2)
            {
                throw new ArgumentException($"{nameof(inflationFactor)}: {inflationFactor}");
            }

            double finalInflationFactor = Math.Pow(inflationFactor, numYears);
            decimal result = amount * (decimal)finalInflationFactor;

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}