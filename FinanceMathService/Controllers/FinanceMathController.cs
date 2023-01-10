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
            double result = NonRiskAssetsNeededInCaseOfRiskAssetCrash(totalAmount, stocksCrashFactor, totalAmount_minNeededAfterCrash);
            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }

        [HttpGet("RateByNumericalSparkassenformel")]
        [Produces("application/json")]
        public JsonResult RateByNumericalSparkassenformel(double betrag1, double zins1, double betrag2, double zins2, double endbetrag, int jahre)
        {
            double result = RateByNumericalSparkassenformel(
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
            double result = RateByNumericalSparkassenformel(
                new List<double> { betrag1, betrag2, betrag3 },
                new List<double> { zins1, zins2, zins3 },
                endbetrag,
                jahre
            );

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }

        internal static double NonRiskAssetsNeededInCaseOfRiskAssetCrash(double totalAmount, double stocksCrashFactor, double totalAmount_minNeededAfterCrash)
        {
            if (stocksCrashFactor > 1 || stocksCrashFactor < 0 || totalAmount_minNeededAfterCrash < 0 || totalAmount < 0)
            {
                throw new ArgumentException();
            }

            double lowRiskAmount = (totalAmount_minNeededAfterCrash - totalAmount + (totalAmount * stocksCrashFactor)) / stocksCrashFactor;
            double highRiskAmount = totalAmount - lowRiskAmount;

            if (highRiskAmount < 0)
            {
                throw new ArgumentException("Not solvable with parameters...");
            }

            if (lowRiskAmount < 0)
            {
                return 0;
            }

            return lowRiskAmount;
        }

        internal static double RateByNumericalSparkassenformel(List<double> betrag, List<double> zins, double endbetrag, int jahre)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            const int MaxIterations = 100;
            const double Precision = 0.001;

            int numIterations = 0;

            double gesamtBetrag = betrag.Sum();
            double angenommeneRate_min = 0;
            double angenommeneRate_max = gesamtBetrag;
            double restBetrag;
            double angenommeneRate;
            do
            {
                angenommeneRate = (angenommeneRate_min + angenommeneRate_max) / 2d;

                restBetrag = betrag.Sum();
                List<double> faktors = betrag.Select(betrag => betrag / gesamtBetrag).ToList();

                for (int i = 0; i < jahre; i++)
                {
                    // rate runter
                    restBetrag -= angenommeneRate;

                    // zinsen drauf
                    List<double> anteiligeBetraege = faktors.Select(faktor => faktor * restBetrag).ToList();

                    for (int j = 0; j < anteiligeBetraege.Count; j++)
                        anteiligeBetraege[j] *= zins[j];

                    restBetrag = anteiligeBetraege.Sum();
                }

                if (restBetrag >= endbetrag)
                {
                    angenommeneRate_min = angenommeneRate;
                }
                else
                {
                    angenommeneRate_max = angenommeneRate;
                }

                if (numIterations++ > MaxIterations)
                {
                    throw new Exception($"Too many iterations: {numIterations}");
                }

                //todo: log
                Console.WriteLine($"{nameof(angenommeneRate)}: {angenommeneRate}  //  {nameof(restBetrag)}: {restBetrag}");

            } while (Math.Abs(restBetrag - endbetrag) > Precision);

            //todo: log
            Console.WriteLine("Duration: " + sw.Elapsed);
            Console.WriteLine("NumIterations: " + numIterations);
            return angenommeneRate;
        }
    }
}