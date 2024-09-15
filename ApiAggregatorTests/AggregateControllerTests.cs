using Moq;
using System.Threading.Tasks;
using Xunit;
using ApiAggregator.Controllers;
using ApiAggregator.Services;
using ApiAggregator.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ApiAggregator.Tests
{
    public class AggregateControllerTests
    {
        private readonly Mock<IWeatherService> _weatherServiceMock;
        private readonly Mock<IMediastackNewsService> _newsServiceMock;
        private readonly Mock<ICryptoService> _cryptoServiceMock;
        private readonly AggregateController _controller;

        public AggregateControllerTests()
        {
            _weatherServiceMock = new Mock<IWeatherService>();
            _newsServiceMock = new Mock<IMediastackNewsService>();
            _cryptoServiceMock = new Mock<ICryptoService>();

            // Initialize the controller with mocked services
            _controller = new AggregateController(_weatherServiceMock.Object, _newsServiceMock.Object, _cryptoServiceMock.Object);
        }

        [Fact]
        public async Task GetAggregatedData_ReturnsOk_WhenValidParametersAreProvided()
        {
            var latitude = 33.0;
            var longitude = 33.0;
            var coinId = "bitcoin";
            var topic = "sports";
            var date = "2024-09-14";

            var weatherMockResponse = new WeatherApiResponse(); // Mock weather response
            var newsMockResponse = new MediastackApiResponse(); // Mock news response
            var cryptoMockResponse = new CryptoApiResponse(); // Mock crypto response

            // Set up mocked services to return the expected responses
            _weatherServiceMock.Setup(ws => ws.GetWeatherAsync(latitude, longitude, date))
                .ReturnsAsync(weatherMockResponse);
            _newsServiceMock.Setup(ns => ns.GetNewsAsync(topic, date))
                .ReturnsAsync(newsMockResponse);
            _cryptoServiceMock.Setup(cs => cs.GetCryptoDataAsync(coinId))
                .ReturnsAsync(cryptoMockResponse);

            var result = await _controller.GetAggregatedData(latitude, longitude, coinId, topic, date);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var aggregatedResponse = Assert.IsType<AggregatedResponse>(okResult.Value);

            Assert.Equal(weatherMockResponse, aggregatedResponse.Weather);
            Assert.Equal(newsMockResponse, aggregatedResponse.News);
            Assert.Equal(cryptoMockResponse, aggregatedResponse.Crypto);
        }

        [Fact]
        public async Task GetAggregatedData_ReturnsBadRequest_WhenInvalidParametersProvided()
        {
            // Arrange
            var latitude = 0.0;
            var longitude = 0.0;
            var coinId = "";
            var topic = "";

            var result = await _controller.GetAggregatedData(latitude, longitude, coinId, topic);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetAggregatedData_ReturnsServerError_WhenExceptionThrown()
        {
            var latitude = 33.0;
            var longitude = 33.0;
            var coinId = "bitcoin";
            var topic = "sports";
            var date = "2024-09-14";

            _weatherServiceMock.Setup(ws => ws.GetWeatherAsync(latitude, longitude, date))
                .ThrowsAsync(new Exception("Test exception"));

            var result = await _controller.GetAggregatedData(latitude, longitude, coinId, topic, date);
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}