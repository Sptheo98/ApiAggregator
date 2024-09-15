using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ApiAggregator.Services;
using ApiAggregator.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace ApiAggregator.Tests
{
    public class WeatherServiceTests
    {
        private readonly WeatherService _weatherService;
        private readonly Mock<HttpClient> _httpClientMock;
        private readonly Mock<ILogger<WeatherService>> _loggerMock;

        public WeatherServiceTests()
        {
            // Mocking the HttpClient and Logger
            _httpClientMock = new Mock<HttpClient>();
            _loggerMock = new Mock<ILogger<WeatherService>>();
        }

        [Fact]
        public async Task GetWeatherAsync_ReturnsWeatherData_WhenCalledWithValidParameters()
        {
            var latitude = 33.0;
            var longitude = 33.0;
            var date = "2024-09-14";

            // Mock response data
            var mockResponse = new WeatherApiResponse
            {
                Hourly = new HourlyWeather
                {
                    Time = new List<DateTime> { DateTime.Now },
                    Temperature_2M = new List<double> { 25.0 }
                }
            };

            // Convert the mock response to a JSON string
            var responseContent = new StringContent(JsonConvert.SerializeObject(mockResponse));

            // Create a fake HttpResponseMessage to simulate a successful response
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = responseContent
            };

            // Use a custom handler to mock the HttpClient behavior
            var httpClient = new HttpClient(new MockHttpMessageHandler(responseMessage));

            // Create a real instance of WeatherService with mocked HttpClient and Logger
            var weatherService = new WeatherService(httpClient, _loggerMock.Object);
            var result = await weatherService.GetWeatherAsync(latitude, longitude, date);

            Assert.NotNull(result);
            Assert.Single(result.Hourly.Time);
            Assert.Equal(25.0, result.Hourly.Temperature_2M[0]);
        }

        // Mocking HttpClient for testing purposes
        private class MockHttpMessageHandler : HttpMessageHandler
        {
            private readonly HttpResponseMessage _responseMessage;

            public MockHttpMessageHandler(HttpResponseMessage responseMessage)
            {
                _responseMessage = responseMessage;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                return await Task.FromResult(_responseMessage);
            }
        }
    }
}