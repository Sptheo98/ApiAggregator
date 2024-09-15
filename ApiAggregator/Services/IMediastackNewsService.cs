using System.Collections.Generic;
using System.Threading.Tasks;
using ApiAggregator.Models;

namespace ApiAggregator.Services
{
    public interface IMediastackNewsService
    {
        Task<MediastackApiResponse> GetNewsAsync(string topic, string? date = null);
    }
}

