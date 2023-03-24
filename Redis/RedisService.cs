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
    private List<string?> _keys = new List<string?>();


    public RedisService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string game, string key, PlayerType playerId,
        CancellationToken cancellationToken = default)
    {
        var value = await _cache.GetStringAsync($"{game}_{playerId}_{key}", cancellationToken);
        if (value is null)
        {
            throw new NullReferenceException();
        } 
        return (JsonConvert.DeserializeObject(value) as JObject)!.ToObject<T>();
    }

    public async Task CreateAsync<T>(string game, string key, PlayerType playerId, T obj,
        CancellationToken cancellationToken = default) where T : class
    {
        _keys.Add($"{game}_{playerId}_{key}");
        await _cache.SetAsync($"{game}_{playerId}_{key}", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj)),
            cancellationToken);
    }

    public async Task RemoveAsync(string game, string key, PlayerType playerId,
        CancellationToken cancellationToken = default)
    {
        _keys.Remove($"{game}_{playerId}_{key}");
        await _cache.RemoveAsync($"{game}_{playerId}_{key}", cancellationToken);
    }

    public async Task<T?> GetAsync<T>(string game, string key, CancellationToken cancellationToken = default)
    {
        var value = await _cache.GetStringAsync($"{game}_{key}", cancellationToken);
        if (value is null)
        {
            throw new NullReferenceException();
        } 
        return ((JsonConvert.DeserializeObject(value) as JObject)!).ToObject<T>();
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
        _keys.Remove($"{game}_{key}");
        await _cache.RemoveAsync($"{game}_{key}", cancellationToken);
    }


    public async Task<object> GetAll()
    {
        List<object> objects = new();
        _keys.ForEach(async c => objects.Add( JsonConvert.DeserializeObject(await _cache.GetStringAsync(c))));
        return objects;
    }
}