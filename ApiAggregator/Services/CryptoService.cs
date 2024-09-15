using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ApiAggregator.Models;
using Microsoft.Extensions.Logging;

namespace ApiAggregator.Services
{
    // Service responsible for fetching cryptocurrency data from external API (CoinGecko)
    public class CryptoService : ICryptoService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CryptoService> _logger;

        // Constructor with dependency injection for HttpClient and ILogger
        public CryptoService(HttpClient httpClient, ILogger<CryptoService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Fetches crypto data for the given coinId from CoinGecko API
        public async Task<CryptoApiResponse> GetCryptoDataAsync(string coinId)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");

                var response = await _httpClient.GetAsync($"https://api.coingecko.com/api/v3/coins/{coinId}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error fetching data from CoinGecko for coin: {coinId}. Status Code: {response.StatusCode}");
                    return new CryptoApiResponse(); // Return an empty response if the API call fails
                }

                var responseString = await response.Content.ReadAsStringAsync();

                // Parse the JSON string into a JsonDocument object
                var jsonDocument = JsonDocument.Parse(responseString);

                var cryptoResponse = new CryptoApiResponse
                {
                    Id = jsonDocument?.RootElement.GetProperty("id").GetString() ?? "Unknown",  // Default to "Unknown" if null
                    Symbol = jsonDocument?.RootElement.GetProperty("symbol").GetString() ?? "Unknown",  // Default to "Unknown"
                    Name = jsonDocument?.RootElement.GetProperty("name").GetString() ?? "Unknown",  // Default to "Unknown"
                    CurrentPriceUsd = jsonDocument?.RootElement
                        .GetProperty("market_data")
                        .GetProperty("current_price")
                        .GetProperty("usd")
                        .GetDouble() ?? 0.0,
                    MarketCapUsd = jsonDocument?.RootElement
                        .GetProperty("market_data")
                        .GetProperty("market_cap")
                        .GetProperty("usd")
                        .GetDouble() ?? 0.0,
                    TotalVolumeUsd = jsonDocument?.RootElement
                        .GetProperty("market_data")
                        .GetProperty("total_volume")
                        .GetProperty("usd")
                        .GetDouble() ?? 0.0,
                    PriceChangePercentage24H = jsonDocument?.RootElement
                        .GetProperty("market_data")
                        .GetProperty("price_change_percentage_24h")
                        .GetDouble() ?? 0.0
                };

                return cryptoResponse;
            }
            catch (HttpRequestException httpRequestEx)
            {
                _logger.LogError(httpRequestEx, $"Error in HTTP request for coin: {coinId}");
                return new CryptoApiResponse(); // Return empty response in case of HTTP request error
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, $"Error parsing JSON response for coin: {coinId}");
                return new CryptoApiResponse(); // Return empty response in case of JSON parsing error
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error occurred while fetching data for coin: {coinId}");
                return new CryptoApiResponse(); // Return empty response for any other unexpected error
            }
        }
    }
}