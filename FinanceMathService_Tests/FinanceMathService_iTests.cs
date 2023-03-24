using FinanceMathService.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net.Http.Json;

namespace FinanceMathService_Tests
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/test-aspnet-core-services-web-apps
    /// https://learn.microsoft.com/de-de/aspnet/core/test/integration-tests?view=aspnetcore-6.0
    /// </summary>
    public class FinanceMathService_iTests
    {
        [TestCase("/FinanceMath/NonRiskAssets?totalAmount=500&stocksCrashFactor=0.5&totalAmount_minNeededAfterCrash=400")]
        public async Task ReturnHelloWorld(string url)
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    // ... Configure test services
                });
            var client = application.CreateClient();

            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.That("application/json; charset=utf-8", Is.EqualTo(response.Content.Headers.ContentType.ToString()));
        }

        [TestCase("/FinanceMath/RateByNumericalSparkassenformel")]
        public async Task ReturnHelloWorld2(string url)
        {
            RateByNumericalSparkassenformelInputDTO input = new RateByNumericalSparkassenformelInputDTO()
            {
                AgeFrom = 60,
                AgeTo = 70,

                GrowthRateCash = 0,
                GrowthRateStocks = 8,
                GrowthRateMetals = 1,

                StartCapitalCash = new CAmount() { FromDeposits = 10000 },
                StartCapitalStocks = new CAmount() { FromDeposits = 100000 },
                StartCapitalMetals = new CAmount() { FromDeposits = 10000 },

                EndCapitalTotal = 0
            };

            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    // ... Configure test services
                });
            var client = application.CreateClient();

            var response = await client.PostAsJsonAsync(url, input);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.That("application/json; charset=utf-8", Is.EqualTo(response.Content.Headers.ContentType.ToString()));
        }
    }
}