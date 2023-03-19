using Microsoft.AspNetCore.Mvc;
using MultiSnake.Enums;
using MultiSnake.Redis;
using MultiSnake.Services.Interfaces;
using MultiSnake.Structs;

namespace MultiSnake;

[ApiController]
[Route("Snake")]
public class GameController : Controller
{
    private readonly IRedisService _redis;

    public GameController(IRedisService redis)
    {
        _redis = redis;
    }

    [HttpPost("Start")]
    public async Task<string> Start()
    {
        var gameID = Game.RandomString(6);
        var snakeBlue = new Snake
        {
            Name = "Blue",
            PlayerID = PlayerType.Blue,
            GameId = gameID,
            Positions = { new Point(-10, 0) }
        };
        var snakeRed = new Snake
        {
            Name = "Red",
            PlayerID = PlayerType.Red,
            GameId = gameID,
            Positions = { new Point(10, 0) }
        };

        var game = new Game
        {
            GameId = gameID,
            Food = new Point(0, 0)
        };
        await _redis.CreateAsync(gameID, Keys.GAME_KEY, game);
        await _redis.CreateAsync(gameID, Keys.SNAKE_KEY, PlayerType.Blue, snakeBlue);
        await _redis.CreateAsync(gameID, Keys.SNAKE_KEY, PlayerType.Red, snakeRed);
        return gameID;
    }

    [HttpPost("Move")]
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
    [HttpPost("Consume")]
    public async Task Consume(string GameId, PlayerType playerID)
    {
        await _redis.GetAsync<Game>(GameId, Keys.GAME_KEY);
        var snakePlayer = await _redis.GetAsync<Snake>(GameId, Keys.SNAKE_KEY, playerID);
        snakePlayer.AddElement();
        await _redis.RemoveAsync(GameId, Keys.SNAKE_KEY, playerID);
        await _redis.CreateAsync(GameId, Keys.SNAKE_KEY, playerID, snakePlayer);
    }

    
}