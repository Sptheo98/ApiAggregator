using System;
using System.Collections.Generic;

namespace ApiAggregator.Models
{
    public class WeatherApiResponse
    {
        public HourlyWeather Hourly { get; set; } = new HourlyWeather();
    }

    // represents the JSON response structure about the info for a location given its latitude-longitude
    public class HourlyWeather
    {
        public List<DateTime> Time { get; set; } = new List<DateTime>();
        public List<double> Temperature_2M { get; set; } = new List<double>();
        public List<double> Rain { get; set; } = new List<double>();
        public List<double> Visibility { get; set; } = new List<double>();
    }
}
