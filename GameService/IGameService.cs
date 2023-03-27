using MultiSnake.Enums;
using MultiSnake.Structs;

namespace MultiSnake.GameService;
public interface IGameService
{
    public Task<object> MovePlayer(string gameId, PlayerType player, Point step);
    public Task MoveFood(string gameId, PlayerType player);
    public Task<string> CreateGame();
    public Task RemoveRoom(string gameId);
    public Task<Game> GetGameInstance(string gameId);
    public Task<Snake> Check(string gameId, string name, string color, PlayerType player);
    public Task<List<Game>> GetAllGames();
}