using MultiSnake.Enums;

namespace MultiSnake.Services.Interfaces;

public interface IRedisService
{
    Task<T?> GetAsync<T>(string game, string key, PlayerType playerID, CancellationToken cancellationToken = default);

    Task CreateAsync<T>(string game, string key, PlayerType playerID, T obj,
        CancellationToken cancellationToken = default) where T : class;

    Task RemoveAsync(string game, string key, PlayerType playerID, CancellationToken cancellationToken = default);
    Task<T?> GetAsync<T>(string game, string key, CancellationToken cancellationToken = default);
    Task CreateAsync<T>(string game, string key, T obj, CancellationToken cancellationToken = default) where T : class;
    Task RemoveAsync(string game, string key, CancellationToken cancellationToken = default);
    Task<object> GetAll();
}