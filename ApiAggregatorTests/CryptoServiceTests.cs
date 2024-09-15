using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ApiAggregator.Services;
using ApiAggregator.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace ApiAggregator.Tests
{
    public class CryptoServiceTests
    {
        private readonly CryptoService _cryptoService;
        private readonly Mock<ILogger<CryptoService>> _loggerMock;

        public CryptoServiceTests()
        {
            _loggerMock = new Mock<ILogger<CryptoService>>();
        }

        [Fact]
        public async Task GetCryptoDataAsync_ReturnsValidData_WhenApiCallIsSuccessful()
        {
            var coinId = "bitcoin";
            var mockApiResponse = @"
            {
                ""id"": ""bitcoin"",
                ""symbol"": ""btc"",
                ""name"": ""Bitcoin"",
                ""market_data"": {
                    ""current_price"": { ""usd"": 50000.0 },
                    ""market_cap"": { ""usd"": 1000000000.0 },
                    ""total_volume"": { ""usd"": 1000000.0 },
                    ""price_change_percentage_24h"": 5.0
                }
            }";

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(mockApiResponse)
            };

            var httpClient = new HttpClient(new MockHttpMessageHandler(responseMessage));
            var cryptoService = new CryptoService(httpClient, _loggerMock.Object);
            var result = await cryptoService.GetCryptoDataAsync(coinId);

            Assert.NotNull(result);
            Assert.Equal("bitcoin", result.Id);
            Assert.Equal("btc", result.Symbol);
            Assert.Equal("Bitcoin", result.Name);
            Assert.Equal(50000.0, result.CurrentPriceUsd);
            Assert.Equal(1000000000.0, result.MarketCapUsd);
            Assert.Equal(1000000.0, result.TotalVolumeUsd);
            Assert.Equal(5.0, result.PriceChangePercentage24H);
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