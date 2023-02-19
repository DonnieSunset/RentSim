using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using SavingPhaseService.DTOs;
using System.Net.Http.Json;

namespace SavingPhaseService_Tests
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/test-aspnet-core-services-web-apps
    /// https://learn.microsoft.com/de-de/aspnet/core/test/integration-tests?view=aspnetcore-6.0
    /// </summary>
    public class SavingPhaseService_iTests
    {
        /// <summary>
        /// Both work without parameters becasue calculation is even possible with default (0) values.
        /// </summary>
        [TestCase("/SavingPhase/Simulate")]
        public async Task ReturnSuccessStatusCode(string url)
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    // ... Configure test services
                });
            var client = application.CreateClient();

            var response = await client.PostAsJsonAsync(url, new SavingPhaseServiceInputDTO());

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.That("application/json; charset=utf-8", Is.EqualTo(
                response.Content.Headers.ContentType.ToString()));
        }
    }
}
