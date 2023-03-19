using MultiSnake.Enums;
using MultiSnake.Interfaces;

namespace MultiSnake.Structs;

public class Snake : IPlayer
{
    public List<Point> Positions { get; set; } = new();
    public string Name { get; set; }
    public string GameId { get; set; }
    public PlayerType PlayerID { get; set; }

    public void Move(Point position)
    {
        var newPositions = new List<Point> { position };
        int i = 0;
        newPositions.AddRange(Positions.TakeWhile(item => i <= Positions.Count - 2));

        Positions = newPositions;
    }

    public void AddElement()
    {
        Positions.Add(Positions.Last().MinusOnePosition());
    }
}