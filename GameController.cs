using Microsoft.AspNetCore.Mvc;
using MultiSnake.Enums;
using MultiSnake.GameService;
using MultiSnake.Redis;
using MultiSnake.Services.Interfaces;
using MultiSnake.Structs;

namespace MultiSnake;

[ApiController]
[Route("Snake")]
public class GameController : Controller
{
    private readonly IGameService _game;

    public GameController(IGameService game)
    {
        _game = game;
    }

    [HttpPost("Start")]
    public Task<string> Start() => _game.CreateGame();

    [HttpPost("Check")]
    public async Task<Snake> Check(string gameId, string name, string color, PlayerType player)
    {
        try
        {
            return await _game.Check(gameId, name, color, player);
        }
        catch (Exception e)
        {
            BadRequest("BAD_GAME_ID");
        }

        return null;
    }
    
    [HttpGet("Rooms")]
    public Task<List<Game>> GetAllGames() => _game.GetAllGames();
}