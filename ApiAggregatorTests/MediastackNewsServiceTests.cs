using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ApiAggregator.Services;
using ApiAggregator.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;

namespace ApiAggregator.Tests
{
    public class MediastackNewsServiceTests
    {
        private readonly MediastackNewsService _mediastackNewsService;
        private readonly Mock<ILogger<MediastackNewsService>> _loggerMock;

        public MediastackNewsServiceTests()
        {
            _loggerMock = new Mock<ILogger<MediastackNewsService>>();
        }

        [Fact]
        public async Task GetNewsAsync_ReturnsValidData_WhenApiCallIsSuccessful()
        {
            var topic = "sports";
            var date = "2024-09-14";

            var mockApiResponse = new MediastackApiResponse
            {
                Pagination = new Pagination { Total = 1 },
                Data = new List<MediastackArticle>
                {
                    new MediastackArticle
                    {
                        Title = "Sample News",
                        Url = "http://example.com",
                        Source = "Example Source",
                        published_at = "2024-09-14"
                    }
                }
            };

            var responseContent = new StringContent(JsonConvert.SerializeObject(mockApiResponse));
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = responseContent
            };

            var httpClient = new HttpClient(new MockHttpMessageHandler(responseMessage));
            var mediastackNewsService = new MediastackNewsService(httpClient, _loggerMock.Object);
            var result = await mediastackNewsService.GetNewsAsync(topic, date);

            Assert.NotNull(result);
            Assert.Single(result.Data);
            Assert.Equal("Sample News", result.Data[0].Title);
            Assert.Equal("Example Source", result.Data[0].Source);
            Assert.Equal("2024-09-14", result.Data[0].published_at);
        }

        [Fact]
        public async Task GetNewsAsync_ReturnsFallbackData_WhenApiCallFails()
        {
            var topic = "sports";
            var date = "2024-09-14";

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            var httpClient = new HttpClient(new MockHttpMessageHandler(responseMessage));
            var mediastackNewsService = new MediastackNewsService(httpClient, _loggerMock.Object);
            var result = await mediastackNewsService.GetNewsAsync(topic, date);

            Assert.NotNull(result);
            Assert.Single(result.Data);
            Assert.Equal("Fallback news data", result.Data[0].Title);
            Assert.Equal("Fallback Source", result.Data[0].Source);
        }

        [Fact]
        public async Task GetNewsAsync_ReturnsFallbackData_WhenInvalidResponseReceived()
        {
            var topic = "sports";
            var date = "2024-09-14";

            // Simulating invalid JSON
            var invalidJsonResponse = @"{""invalid"":""json""}";

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(invalidJsonResponse)
            };

            var httpClient = new HttpClient(new MockHttpMessageHandler(responseMessage));
            var mediastackNewsService = new MediastackNewsService(httpClient, _loggerMock.Object);
            var result = await mediastackNewsService.GetNewsAsync(topic, date);

            Assert.NotNull(result);
            Assert.Single(result.Data);
            Assert.Equal("Fallback news data", result.Data[0].Title);
            Assert.Equal("Fallback Source", result.Data[0].Source);
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