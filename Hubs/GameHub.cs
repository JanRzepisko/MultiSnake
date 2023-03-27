using Microsoft.AspNetCore.SignalR;
using MultiSnake.Enums;
using MultiSnake.GameService;
using MultiSnake.Redis;
using MultiSnake.Services.Interfaces;
using MultiSnake.Structs;

namespace MultiSnake.Hubs;

public class GameHub : Hub
{
    private readonly IGameService _game;

    public GameHub(IGameService game)
    {
        _game = game;
    }
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("Hej");
    }

    [HubMethodName("Move")]
    public async Task Move(string gameId, PlayerType playerId, Point step)
    {
        var response = _game.MovePlayer(gameId, playerId, step);
        await Clients.Caller.SendAsync("SEND", await response);
    }
    [HubMethodName("Eat")]
    public Task Eat(string gameId, PlayerType playerId) => _game.MoveFood(gameId, playerId);

    [HubMethodName("GameOver")]
    public async Task GameOver(string gameId, GameOver whoWon)
    {
       await Clients.All.SendAsync("GameOver", new { gameId, whoWon }); 
       await _game.RemoveRoom(gameId);
    }
}