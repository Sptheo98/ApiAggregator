using ApiAggregator.Models;
using System.Threading.Tasks;

namespace ApiAggregator.Services
{
    public interface IWeatherService
    {
        Task<WeatherApiResponse> GetWeatherAsync(double latitude, double longitude, string? date = null);
    }
}
