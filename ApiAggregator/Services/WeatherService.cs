using System;
using System.Net.Http;
using System.Threading.Tasks;
using ApiAggregator.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ApiAggregator.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(HttpClient httpClient, ILogger<WeatherService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Asynchronously retrieves weather data based on latitude and longitude
        // Accept an optional 'date' parameter for filtering
        public async Task<WeatherApiResponse> GetWeatherAsync(double latitude, double longitude, string? date = null)
        {
            try
            {
                // Construct the base API URL with latitude and longitude
                var apiUrl = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m,rain,visibility";

                // If a date is provided, add the 'start_date' and 'end_date' parameters
                if (!string.IsNullOrEmpty(date))
                {
                    apiUrl += $"&start_date={date}&end_date={date}";
                }

                
                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the response into the WeatherApiResponse model
                var weatherData = JsonConvert.DeserializeObject<WeatherApiResponse>(responseBody);
                if (weatherData == null)
                {
                    _logger.LogError("Failed to deserialize weather data. Returning fallback response.");
                    throw new ApplicationException("Failed to fetch weather data.");
                }

                return weatherData;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while fetching weather data from Open-Meteo API");
                throw new ApplicationException("Failed to fetch weather data. Please check the input coordinates or try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching weather data");
                throw;
            }
        }
    }
}
