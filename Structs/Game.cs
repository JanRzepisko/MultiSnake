using MultiSnake.Enums;

namespace MultiSnake.Structs;

public class Game
{
    public Game(string gameId) => GameId = gameId;

    public string GameId { get; }
    public Point Food { get; set; } = null!;
    public Snake Player1 { get; internal set; } = null!;
    public Snake Player2 { get; internal set; } = null!;

    private static readonly Random Random = new();

    public void InitGame()
    {
        Food = new Point(14, 14);
        
        Player2 = new Snake()
        {
            Name = "",
            PlayerId = PlayerType.Player2,
            GameId = this.GameId
        };

        Player2.Positions.Add(new Point(7, 15));
        Player2.Positions.Add(new Point(6, 15));
        Player2.Positions.Add(new Point(5, 15));
        Player2.Positions.Add(new Point(4, 15));
        Player2.Positions.Add(new Point(3, 15));
        Player2.Positions.Add(new Point(2, 15));

        Player1 = new Snake
        {
            Name = "",
            PlayerId = PlayerType.Player1,
            GameId = this.GameId
        };
        Player1.Positions.Add(new Point(29, 15));
        Player1.Positions.Add(new Point(28, 15));
        Player1.Positions.Add(new Point(27, 15));
        Player1.Positions.Add(new Point(26, 15));
        Player1.Positions.Add(new Point(25, 15));
        Player1.Positions.Add(new Point(24, 15));
    }
    
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}