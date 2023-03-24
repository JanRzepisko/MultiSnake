using MultiSnake.Enums;
using MultiSnake.Structs;

namespace MultiSnake.GameService;
public interface IGameService
{
    public Task<object> MovePlayer(string gameId, PlayerType player, Point step);
    public Task MoveFood(string gameId, PlayerType player);
    public Task<string> CreateGame();
    public Task EndGame(string gameId);
    public Task<Game> GetGameInstance(string gameId);
}