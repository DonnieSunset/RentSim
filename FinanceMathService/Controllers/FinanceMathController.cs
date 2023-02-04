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

        [HttpGet("RateByNumericalSparkassenformel")]
        [Produces("application/json")]
        public JsonResult RateByNumericalSparkassenformel(decimal betrag_cash, decimal zins_cash, decimal betrag_stocks, decimal zins_stocks, decimal betrag_metals, decimal zins_metals, decimal endbetrag, int yearStart, int yearEnd)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            
            SimulationResultDTO proto;
            decimal result = myFinanceMath.RateByNumericalSparkassenformel(
                betrag_cash, betrag_stocks, betrag_metals,
                zins_cash, zins_stocks, zins_metals,
                endbetrag,
                yearStart, yearEnd,
                out proto
            );

            return new JsonResult(proto, new JsonSerializerOptions { WriteIndented = true });
        }

        [HttpGet("StartCapitalByNumericalSparkassenformel")]
        [Produces("application/json")]
        public JsonResult StartCapitalByNumericalSparkassenformel(decimal rateTotal_perYear, double factor_cash, double zins_cash, double factor_stocks, double zins_stocks, double factor_metals, double zins_metals, decimal endbetrag, int yearStart, int yearEnd)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            SimulationResultDTO proto;
            decimal result = myFinanceMath.StartCapitalByNumericalSparkassenformel(
                rateTotal_perYear,
                factor_cash, factor_stocks, factor_metals,
                zins_cash, zins_stocks, zins_metals,
                endbetrag,
                yearStart, yearEnd, 
                out proto
            );

            return new JsonResult(proto, new JsonSerializerOptions { WriteIndented = true });
        }

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
            decimal result = myFinanceMath.AmountWithInflation(ageStart, ageEnd, amount, inflationRate);

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}