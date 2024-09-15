using ApiAggregator.Models;
using ApiAggregator.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ApiAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregateController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly IMediastackNewsService _newsService;
        private readonly ICryptoService _cryptoService;

        public AggregateController(IWeatherService weatherService, IMediastackNewsService newsService, ICryptoService cryptoService)
        {
            // Constructor injecting the required services for weather, news, and crypto data
            _weatherService = weatherService;
            _newsService = newsService;
            _cryptoService = cryptoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAggregatedData(double latitude, double longitude, string coinId, string topic, string? date = null)
        {
             // Main method to aggregate data from multiple external services based on input parameters
            if (latitude == 0 || longitude == 0 || string.IsNullOrEmpty(topic) || string.IsNullOrEmpty(coinId))
            {
                return BadRequest("Invalid input parameters.");
            }

            try
            {
                // Run tasks in parallel for better performance
                var weatherTask = _weatherService.GetWeatherAsync(latitude, longitude, date);
                var newsTask = _newsService.GetNewsAsync(topic, date);

                // Crypto data does not depend on the date , it only works with real time data
                var cryptoTask = _cryptoService.GetCryptoDataAsync(coinId);

                // Wait for all the tasks to complete before proceeding
                await Task.WhenAll(weatherTask, newsTask, cryptoTask);

                // Aggregate the data from all services
                var aggregatedResponse = new AggregatedResponse
                {
                    Weather = weatherTask.Result,
                    News = newsTask.Result,
                    Crypto = cryptoTask.Result
                };

                return Ok(aggregatedResponse);
            }
            catch (Exception)
            {
                // Handle errors, returning 500 if something fails
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the aggregated data.");
            }
        }
    }
}
