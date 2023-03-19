using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using MultiSnake.Enums;
using MultiSnake.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MultiSnake.Services.implementations;

public class RedisService : IRedisService
{
    private readonly IDistributedCache _cache;
    public List<string> _keys = new List<string>();


    public RedisService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T> GetAsync<T>(string game, string key, PlayerType playerID,
        CancellationToken cancellationToken = default)
    {
        var value =  (JsonConvert.DeserializeObject(await _cache.GetStringAsync($"{game}_{playerID}_{key}", cancellationToken)) as JObject).ToObject<T>();
        if (value is null)
            throw new NullReferenceException();
        return value;
    }

    public async Task CreateAsync<T>(string game, string key, PlayerType playerID, T obj,
        CancellationToken cancellationToken = default) where T : class
    {
        _keys.Add($"{game}_{playerID}_{key}");
        await _cache.SetAsync($"{game}_{playerID}_{key}", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj)),
            cancellationToken);
    }

    public async Task RemoveAsync(string game, string key, PlayerType playerID,
        CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync($"{game}_{playerID}_{key}", cancellationToken);
        _keys.Remove($"{game}_{playerID}_{key}");
    }

    public async Task<T> GetAsync<T>(string game, string key, CancellationToken cancellationToken = default)
    {
        var value = (JsonConvert.DeserializeObject(await _cache.GetStringAsync($"{game}_{key}", cancellationToken)) as JObject).ToObject<T>();
        if (value is null)
            throw new NullReferenceException();
        return value;
    }

    public async Task CreateAsync<T>(string game, string key, T obj, CancellationToken cancellationToken = default)
        where T : class
    {
        _keys.Add($"{game}_{key}");
        await _cache.SetAsync($"{game}_{key}", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj)),
            cancellationToken);
    }

    public async Task RemoveAsync(string game, string key, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync($"{game}_{key}", cancellationToken);
        _keys.Remove($"{game}_{key}");
    }


    public async Task<object> GetAll()
    {
        List<object> objects = new();
        _keys.ForEach(async c => objects.Add( JsonConvert.DeserializeObject(await _cache.GetStringAsync(c))));
        return objects;
    }
}