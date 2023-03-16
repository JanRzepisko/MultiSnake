namespace MultiSnake.Services.Interfaces;

public interface IRedisService
{
    public Task<string> GetAsync<T>(string game, string key, CancellationToken cancellationToken = default) where T : class;
    public Task CreateAsync<T>(string game, string key, T obj, CancellationToken cancellationToken = default) where T : class;
    public Task RemoveAsync(string game, string key, CancellationToken cancellationToken = default);
}