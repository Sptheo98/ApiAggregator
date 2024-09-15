using System.Threading.Tasks;
using System.Collections.Generic;
using ApiAggregator.Models;

namespace ApiAggregator.Services;

public interface IAggregationService
{
    Task<AggregatedResponse> GetAggregatedData(double latitude, double longitude, string topic, string coinId);
}
