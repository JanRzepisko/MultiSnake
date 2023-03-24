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

    [HttpPost("GameOver")]
    public Task GameOver(string gameId) => _game.EndGame(gameId);
}