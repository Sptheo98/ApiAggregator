using ApiAggregator.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ApiAggregator.Services
{
    public interface ICryptoService
    {
        Task<CryptoApiResponse> GetCryptoDataAsync(string coinId);
    }
}