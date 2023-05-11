using Domain;
using Microsoft.AspNetCore.Mvc;
using PhaseIntegratorService.Clients;
using PhaseIntegratorService.DTOs;
using System.Text.Json;

namespace PhaseIntegratorService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PhaseIntegratorController : ControllerBase
    {
        private readonly ILogger<PhaseIntegratorController> myLogger;
        private readonly IHttpClientFactory myHttpClientFactory;
        private readonly IServiceProvider myServiceProvider;

        public PhaseIntegratorController(IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory, ILogger<PhaseIntegratorController> logger)
        {
            myServiceProvider = serviceProvider;
            myHttpClientFactory = httpClientFactory;
            myLogger = logger;
        }

        [HttpPost("SimulateGoodCase")]
        [Produces("application/json")]
        public async Task<JsonResult> SimulateGoodCase(LifeAssumptions input)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            PhaseIntegratorServiceResultDTO result;
            try
            {
                result = await myServiceProvider.GetService<IPhaseIntegrator>().SimulateGoodCase(
                    input,
                    myServiceProvider.GetService<IFinanceMathClient>(),
                    myServiceProvider.GetService<ISavingPhaseClient>(),
                    myServiceProvider.GetService<IStopWorkPhaseClient>(),
                    myServiceProvider.GetService<IRentPhaseClient>()
                    );
            }
            catch (Exception ex)
            {
                result = new PhaseIntegratorServiceResultDTO();
                result.Result.Type = ResultDTO.ResultType.Failure;
                result.Result.Message = ex.Message;
            }

            return new JsonResult(result, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}