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
        try
        {
            var game = await _redis.GetAsync<Game>(gameId, Keys.GAME_KEY);
            Snake? snakePlayer = await _redis.GetAsync<Snake>(gameId, Keys.SNAKE_KEY, player);
            snakePlayer.Move(step);
            await _redis.RemoveAsync(gameId, Keys.SNAKE_KEY, player);
            await _redis.CreateAsync(gameId, Keys.SNAKE_KEY, player, snakePlayer);
            Snake? snakeOpponent;

            if (snakePlayer.PlayerId == PlayerType.Player2)
                snakeOpponent = await _redis.GetAsync<Snake>(gameId, Keys.SNAKE_KEY, PlayerType.Player1);
            else
                snakeOpponent = await _redis.GetAsync<Snake>(gameId, Keys.SNAKE_KEY, PlayerType.Player2);
            return new { opponent = snakeOpponent, game };
        }
        catch(NullReferenceException)
        {
            Console.WriteLine("Bad Game Id");
            return "BAD_GAME_ID";
        }
    }

    public async Task MoveFood(string gameId, PlayerType player)
    {
        try
        {
            var game = await _redis.GetAsync<Game>(gameId, Keys.GAME_KEY);
            game.Food.RandomPoint();

            await _redis.RemoveAsync(gameId, Keys.GAME_KEY);
            await _redis.CreateAsync(gameId, Keys.GAME_KEY, game);

            var snakePlayer = await _redis.GetAsync<Snake>(gameId, Keys.SNAKE_KEY, player);
            snakePlayer.AddElement();

            await _redis.RemoveAsync(gameId, Keys.SNAKE_KEY, player);
            await _redis.CreateAsync(gameId, Keys.SNAKE_KEY, player, snakePlayer);
        }
        catch (NullReferenceException)
        {
            Console.WriteLine("Bad Game Id");
        }
    }

    public async Task<string> CreateGame()
    {
        //Init Game
        var gameId = Game.RandomString(6);
        var game = new Game(gameId);
        game.InitGame();
        
        //Save game instance in Redis
        await _redis.CreateAsync(gameId, Keys.GAME_KEY, game);
        
        await _redis.CreateAsync(gameId, Keys.SNAKE_KEY, PlayerType.Player2, game.Player2);
        await _redis.CreateAsync(gameId, Keys.SNAKE_KEY, PlayerType.Player1, game.Player1);
        return gameId;
    }
    
    public async Task EndGame(string gameId, PlayerType winner)
    {
        var game = await _redis.GetAsync<Game>(gameId, Keys.GAME_KEY);
        var snakePlayer = await _redis.GetAsync<Snake>(gameId, Keys.SNAKE_KEY, winner);
        snakePlayer.Won = true;
        await _redis.RemoveAsync(gameId, Keys.SNAKE_KEY, winner);
        await _redis.CreateAsync(gameId, Keys.SNAKE_KEY, winner, snakePlayer);
    }
    public Task<Game> GetGameInstance(string gameId)
    {
        return _redis.GetAsync<Game>(gameId, Keys.GAME_KEY);
    }

    public async Task Check(string gameId, string name, string color, PlayerType player)
    {
        var game = await _redis.GetAsync<Game>(gameId, Keys.GAME_KEY);
        var snake = await _redis.GetAsync<Snake>(gameId, Keys.SNAKE_KEY, player);
        
        snake.Color = color;
        snake.Name = name;

        await _redis.RemoveAsync(gameId, Keys.SNAKE_KEY, player);
        await _redis.CreateAsync(gameId, Keys.SNAKE_KEY, player, snake);
    }

    public async Task<List<Game>> GetAllGames()
    {
        var game =await _redis.GetAllGames();
        var includeGames = new List<Game>();
        foreach (var g in game)
        {
            g.Player1 = await _redis.GetAsync<Snake>(g.GameId, Keys.SNAKE_KEY, PlayerType.Player1);
            g.Player2 = await _redis.GetAsync<Snake>(g.GameId, Keys.SNAKE_KEY, PlayerType.Player2);
            includeGames.Add(g);
        }

        return includeGames;
    }
}