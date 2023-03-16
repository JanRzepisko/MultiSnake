using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using MultiSnake.Services.Interfaces;
using Newtonsoft.Json;

namespace MultiSnake.Services.implementations;

public class RedisService : IRedisService
{
    private IDistributedCache _cache;

    public RedisService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<string> GetAsync<T>(string game, string key, CancellationToken cancellationToken = default) where T : class
    {
        string? value = await _cache.GetStringAsync($"{game}_{key}", cancellationToken);
        if (value is null)
            throw new NullReferenceException();
        return value;
    }
    public async Task CreateAsync<T>(string game, string key, T obj, CancellationToken cancellationToken = default) where T : class
    {
         await _cache.SetAsync($"{game}_{key}", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj)),cancellationToken);
    }
    public async Task RemoveAsync(string game, string key, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync($"{game}_{key}", cancellationToken);
    }
}