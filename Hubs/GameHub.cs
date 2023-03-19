using Microsoft.AspNetCore.SignalR;
using MultiSnake.Enums;
using MultiSnake.Redis;
using MultiSnake.Services.Interfaces;
using MultiSnake.Structs;

namespace MultiSnake.Hubs;

public class GameHub : Hub
{
    private readonly IRedisService _redis;

    public GameHub(IRedisService redis)
    {
        _redis = redis;
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("GET", "Hello");
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine("Pa pa");
        return Task.CompletedTask;
    }
    
    [HubMethodName("Move")]
    public async Task<object> Move(string GameId, PlayerType playerID, Point Step)
    {
        var game = await _redis.GetAsync<Game>(GameId, Keys.GAME_KEY);
        var snakePlayer = await _redis.GetAsync<Snake>(GameId, Keys.SNAKE_KEY, playerID);
        snakePlayer.Move(Step);
        await _redis.RemoveAsync(GameId, Keys.SNAKE_KEY, playerID);
        await _redis.CreateAsync(GameId, Keys.SNAKE_KEY, playerID, snakePlayer);
        var snakeOpponent = new Snake();

        if (snakePlayer.PlayerID == PlayerType.Blue)
            snakeOpponent = await _redis.GetAsync<Snake>(GameId, Keys.SNAKE_KEY, PlayerType.Red);
        else
            snakeOpponent = await _redis.GetAsync<Snake>(GameId, Keys.SNAKE_KEY, PlayerType.Blue);

        return new { opponent=snakeOpponent, game };
    }
    [HubMethodName("Consume")]
    public async Task Consume(string GameId, PlayerType playerID)
    {
        await _redis.GetAsync<Game>(GameId, Keys.GAME_KEY);
        var snakePlayer = await _redis.GetAsync<Snake>(GameId, Keys.SNAKE_KEY, playerID);
        snakePlayer.AddElement();
        await _redis.RemoveAsync(GameId, Keys.SNAKE_KEY, playerID);
        await _redis.CreateAsync(GameId, Keys.SNAKE_KEY, playerID, snakePlayer);
    }
}