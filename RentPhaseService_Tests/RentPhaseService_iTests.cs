using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RentPhaseService.Clients;
using RentPhaseService.DTOs;
using System.Text.Json;

namespace RentPhaseService_Tests
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/test-aspnet-core-services-web-apps
    /// https://learn.microsoft.com/de-de/aspnet/core/test/integration-tests?view=aspnetcore-6.0
    /// https://rehansaeed.com/asp-net-core-integration-testing-mocking-using-moq/
    /// https://gunnarpeipman.com/aspnet-core-integration-test-startup/
    /// https://www.infoworld.com/article/3646098/demystifying-the-program-and-startup-classes-in-aspnet-core.html
    /// </summary>
    public class RentPhaseService_iTests
    {
        [TestCase("/RentPhase/Simulate?ageStart=65&ageEnd=80&totalRateNeeded_perYear=40000&capitalCash=200000&growthRateCash=3&capitalStocks=200000&growthRateStocks=3&capitalMetals=200000&growthRateMetals=3")]
        public async Task ClientTest_Simulate_ReturnsSuccessfulStatusCode(string url)
        {
            var response = await SendToClient(url);

            Assert.That(() => response.EnsureSuccessStatusCode(), Throws.Nothing); // Status Code 200-299
            Assert.That("application/json; charset=utf-8", Is.EqualTo(response.Content.Headers.ContentType.ToString()));
        }

        [TestCase("/RentPhase/Simulate?ageStart=65&ageEnd=80&totalRateNeeded_perYear=40000&capitalCash=200000&growthRateCash=3&capitalStocks=200000&growthRateStocks=3&capitalMetals=200000&growthRateMetals=3")]
        public async Task ClientTest_Simulate_ReturnsValidJson(string url)
        {
            var response = await SendToClient(url);

            var rentPhaseResultString = await response.Content.ReadAsStringAsync();

            Assert.That(() => JsonDocument.Parse(rentPhaseResultString), Throws.Nothing, "WebApi response must be valid Json.");
        }

        private async Task<HttpResponseMessage?> SendToClient(string url)
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    // ... Configure test services
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped<IFinanceMathClient>(x => MockedFinanceMathClient.Object);
                    });
                });

            var client = application.CreateClient();
            var response = await client.GetAsync(url);

            return response;
        }

        private Mock<IFinanceMathClient> MockedFinanceMathClient
        {
            get {
                var mockedFinanceMathClient = new Mock<IFinanceMathClient>();
                mockedFinanceMathClient
                    .Setup(x => x.StartCapitalByNumericalSparkassenformel(
                        It.IsAny<decimal>(), 
                        It.IsAny<decimal>(), 
                        It.IsAny<decimal>(), 
                        It.IsAny<decimal>(), 
                        It.IsAny<decimal>(), 
                        It.IsAny<decimal>(), 
                        It.IsAny<decimal>(), 
                        It.IsAny<decimal>(), 
                        It.IsAny<int>(), 
                        It.IsAny<int>()))
                    .ReturnsAsync(ValidFinancialMathServiceResponse);

                return mockedFinanceMathClient;
            }
        }

        private RentPhaseServiceResultDTO ValidFinancialMathServiceResponse
        {
            get
            {
                string resultJson = """
                            {
                            "Entries": [
                            {
                            "Age": 60,
                            "Deposits": {
                                "Cash": 8000.0,
                                "Stocks": 20000.0,
                                "Metals": 12000.0
                            },
                            "Interests": {
                                "Cash": 1967.13209509849548339843750,
                                "Stocks": 3278.55349183082580566406250,
                                "Metals": 983.56604754924774169921875
                            }
                            }
                            ]
                            }
                            """;
                return JsonSerializer.Deserialize<RentPhaseServiceResultDTO>(resultJson);
            }
        }
    }
}
