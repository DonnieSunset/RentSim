using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace FinanceMathService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FinanceMathController : ControllerBase
    {
        private readonly ILogger<FinanceMathController> _logger;

        public FinanceMathController(ILogger<FinanceMathController> logger)
        {
            _logger = logger;
        }

        [HttpGet("NonRiskAssets")]
        [Produces("application/json")]
        public JsonResult NonRiskAssets(double totalAmount, double stocksCrashFactor, double totalAmount_minNeededAfterCrash)
        {
            double result = FinanceMath.NonRiskAssetsNeededInCaseOfRiskAssetCrash(totalAmount, stocksCrashFactor, totalAmount_minNeededAfterCrash);
            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }

        [HttpGet("RateByNumericalSparkassenformel")]
        [Produces("application/json")]
        public JsonResult RateByNumericalSparkassenformel(double betrag1, double zins1, double betrag2, double zins2, double endbetrag, int jahre)
        {
            //double result = 32;
            double result = FinanceMath.RateByNumericalSparkassenformel(
                new List<double> { betrag1, betrag2 },
                new List<double> { zins1, zins2 },
                endbetrag,
                jahre
            );

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }

        [HttpGet("RateByNumericalSparkassenformel")]
        [Produces("application/json")]
        public JsonResult RateByNumericalSparkassenformel(double betrag1, double zins1, double betrag2, double zins2, double betrag3, double zins3, double endbetrag, int jahre)
        {
            double result = FinanceMath.RateByNumericalSparkassenformel(
                new List<double> { betrag1, betrag2, betrag3 },
                new List<double> { zins1, zins2, zins3 },
                endbetrag,
                jahre
            );

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}