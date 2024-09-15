using ApiAggregator.Models;
using ApiAggregator.Services;
using System.Threading.Tasks;

namespace ApiAggregator.Services
{
    // Service responsible for aggregating data from weather, news, and cryptocurrency services
    public class AggregationService : IAggregationService
    {
        private readonly IWeatherService _weatherService;
        private readonly IMediastackNewsService _newsService;
        private readonly ICryptoService _cryptoService;

        // Constructor injects the required services (weather, news, and crypto)
        public AggregationService(IWeatherService weatherService, IMediastackNewsService newsService, ICryptoService cryptoService)
        {
            _weatherService = weatherService;
            _newsService = newsService;
            _cryptoService = cryptoService;
        }

        // Asynchronously fetches and aggregates data from all external services
        public async Task<AggregatedResponse> GetAggregatedData(double latitude, double longitude, string topic, string coinId)
        {
            // Start all tasks concurrently
            var weatherTask = _weatherService.GetWeatherAsync(latitude, longitude);
            var newsTask = _newsService.GetNewsAsync(topic);
            var cryptoTask = _cryptoService.GetCryptoDataAsync(coinId);

            // Wait for all tasks to complete
            await Task.WhenAll(weatherTask, newsTask, cryptoTask);

            // Combine the results of all services into a single aggregated response
            return new AggregatedResponse
            {
                Weather = weatherTask.Result,
                News = newsTask.Result,
                Crypto = cryptoTask.Result
            };
        }
    }
}
