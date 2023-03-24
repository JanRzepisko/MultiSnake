using System.Runtime.InteropServices;
using MultiSnake.Enums;
using MultiSnake.Redis;
using MultiSnake.Services.Interfaces;
using MultiSnake.Structs;

namespace MultiSnake.GameService;

public class GameService : IGameService
{
    private readonly IRedisService _redis;

    public GameService(IRedisService redis)
    {
        _redis = redis;
    }
    public async Task<object> MovePlayer(string gameId, PlayerType player, Point step)
    {
        var game = await _redis.GetAsync<Game>(gameId, Keys.GAME_KEY);
        var snakePlayer = await _redis.GetAsync<Snake>(gameId, Keys.SNAKE_KEY, player);
        snakePlayer.Move(step);
        await _redis.RemoveAsync(gameId, Keys.SNAKE_KEY, player);
        await _redis.CreateAsync(gameId, Keys.SNAKE_KEY, player, snakePlayer);
        Snake snakeOpponent;

        if (snakePlayer.PlayerID == PlayerType.Blue)
            snakeOpponent = await _redis.GetAsync<Snake>(gameId, Keys.SNAKE_KEY, PlayerType.Red);
        else
            snakeOpponent = await _redis.GetAsync<Snake>(gameId, Keys.SNAKE_KEY, PlayerType.Blue);

        return new { opponent=snakeOpponent, game };    
    }

    public async Task MoveFood(string gameId, PlayerType player)
    {
        await _redis.GetAsync<Game>(gameId, Keys.GAME_KEY);
        var game = await _redis.GetAsync<Game>(gameId, Keys.GAME_KEY);
        game.Food.RandomPoint();

        await _redis.RemoveAsync(gameId, Keys.GAME_KEY);
        await _redis.CreateAsync(gameId, Keys.GAME_KEY, game);

        var snakePlayer = await _redis.GetAsync<Snake>(gameId, Keys.SNAKE_KEY, player);
        snakePlayer.AddElement();
        
        await _redis.RemoveAsync(gameId, Keys.SNAKE_KEY, player);
        await _redis.CreateAsync(gameId, Keys.SNAKE_KEY, player, snakePlayer);
    }

    public async Task<string> CreateGame()
    {
        //Init Game
        var gameId = Game.RandomString(6);
        var game = new Game(gameId);
        game.InitGame();
        
        //Save game instance in Redis
        await _redis.CreateAsync(gameId, Keys.GAME_KEY, game);
        await _redis.CreateAsync(gameId, Keys.SNAKE_KEY, PlayerType.Blue, game.SnakeBlue);
        await _redis.CreateAsync(gameId, Keys.SNAKE_KEY, PlayerType.Red, game.SnakeRed);
        return gameId;
    }
    public Task EndGame(string gameId)
    {
        throw new NotImplementedException();
    }
    public Task<Game> GetGameInstance(string gameId)
    {
        return _redis.GetAsync<Game>(gameId, Keys.GAME_KEY);
    }
}