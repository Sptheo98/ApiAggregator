using System;
using System.Collections.Generic;
using ApiAggregator.Models;

namespace ApiAggregator.Models
{
    // Represents the structure of the aggregated JSON response returned by the API
    public class AggregatedResponse
    {
        public CryptoApiResponse? Crypto { get; set; }
        public MediastackApiResponse? News { get; set; }
        public WeatherApiResponse? Weather { get; set; }
    }
}
