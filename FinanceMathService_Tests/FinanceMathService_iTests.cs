using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace FinanceMathService_Tests
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/test-aspnet-core-services-web-apps
    /// https://learn.microsoft.com/de-de/aspnet/core/test/integration-tests?view=aspnetcore-6.0
    /// </summary>
    public class FinanceMathService_iTests
    {
        [TestCase("/FinanceMath/NonRiskAssets?totalAmount=500&stocksCrashFactor=0.5&totalAmount_minNeededAfterCrash=400")]
        [TestCase("/FinanceMath/RateByNumericalSparkassenformel?betrag_cash=10000&zins_cash=0&betrag_stocks=100000&zins_stocks=8&betrag_metals=10000&zins_metals=1&endbetrag=0&yearStart=60&yearEnd=70")]
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
    }
}