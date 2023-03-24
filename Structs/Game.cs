using MultiSnake.Enums;

namespace MultiSnake.Structs;

public class Game
{
    public Game(string gameId) => GameId = gameId;

    public string GameId { get; }
    public Point Food { get; set; } = null!;

    public Snake SnakeRed { get; private set; } = null!;
    public Snake SnakeBlue { get; private set; } = null!;

    private static readonly Random Random = new();

    public void InitGame()
    {
        Food = new Point(14, 14);
        
        SnakeBlue = new Snake()
        {
            Name = "Blue",
            PlayerID = PlayerType.Blue,
            GameId = this.GameId
        };

        SnakeBlue.Positions.Add(new Point(7, 15));
        SnakeBlue.Positions.Add(new Point(6, 15));
        SnakeBlue.Positions.Add(new Point(5, 15));
        SnakeBlue.Positions.Add(new Point(4, 15));
        SnakeBlue.Positions.Add(new Point(3, 15));
        SnakeBlue.Positions.Add(new Point(2, 15));

        SnakeRed = new Snake
        {
            Name = "Red",
            PlayerID = PlayerType.Red,
            GameId = this.GameId
        };
        SnakeRed.Positions.Add(new Point(7, 15));
        SnakeRed.Positions.Add(new Point(6, 15));
        SnakeRed.Positions.Add(new Point(5, 15));
        SnakeRed.Positions.Add(new Point(4, 15));
        SnakeRed.Positions.Add(new Point(3, 15));
        SnakeRed.Positions.Add(new Point(2, 15));
    }
    
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}