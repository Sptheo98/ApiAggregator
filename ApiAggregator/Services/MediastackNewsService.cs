using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ApiAggregator.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ApiAggregator.Services
{
    public class MediastackNewsService : IMediastackNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MediastackNewsService> _logger;

        public MediastackNewsService(HttpClient httpClient, ILogger<MediastackNewsService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        

        // Asynchronously retrieves news data (articles) for a given topic
        // Accept an optional 'date' parameter for filtering
        public async Task<MediastackApiResponse> GetNewsAsync(string topic, string? date = null)
        {
            try
            {
                // Mediastack API key and endpoint
                var apiKey = "b263607d1908c88ef4c6d5de6ecae1a9";  
                var apiUrl = $"http://api.mediastack.com/v1/news?access_key={apiKey}&keywords={topic}&languages=en";

                if (!string.IsNullOrEmpty(date))
                {
                    apiUrl += $"&date={date}";
                }

                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the response directly into MediastackApiResponse
                var mediastackApiResponse = JsonConvert.DeserializeObject<MediastackApiResponse>(responseBody);

                // Check if the API returned valid data
                if (mediastackApiResponse == null || mediastackApiResponse.Data == null || mediastackApiResponse.Data.Count == 0)
                {
                    _logger.LogError("Mediastack API returned a null or invalid response.");
                    return GetFallbackNewsApiResponse();
                }

                return mediastackApiResponse;  // Return the response with articles and pagination
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Error fetching news data from Mediastack API");
                return GetFallbackNewsApiResponse();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error occurred while fetching news");
                throw;
            }
        }

        // Fallback response in case of errors
        private MediastackApiResponse GetFallbackNewsApiResponse()
        {
            return new MediastackApiResponse
            {
                Pagination = new Pagination { Total = 1 },
                Data = new List<MediastackArticle>
                {
                    new MediastackArticle
                    {
                        Title = "Fallback news data",
                        Url = "",
                        Source = "Fallback Source",
                        published_at = ""
                    }
                }
            };
        }
    }
}
