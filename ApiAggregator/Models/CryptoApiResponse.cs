using System;
using System.Collections.Generic;

namespace ApiAggregator.Models
{
    // represents the JSON response structure about the field cryptoId from external API
    public class CryptoApiResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public double CurrentPriceUsd { get; set; }
        public double MarketCapUsd { get; set; }
        public double TotalVolumeUsd { get; set; }
        public double PriceChangePercentage24H { get; set; }
    }
}
