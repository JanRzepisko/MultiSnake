using MultiSnake.Enums;
using MultiSnake.Interfaces;

namespace MultiSnake.Structs;

public class Snake : IPlayer
{
    public List<Point> Positions { get; set; } = new();
    public string Name { get; set; }
    public string Color { get; set; }
    public string GameId { get; set; }
    public PlayerType PlayerId { get; set; }

    public void Move(Point position)
    {
        var newPositions = new List<Point> { position };
        int i = 0;

        foreach (var p in Positions)
        {
           newPositions.Add(p);
           i++;
           if(i > Positions.Count - 2)
               break;
        }

        Positions = newPositions;
    }

    public void AddElement()
    {
        Positions.Add(Positions.Last().MinusOnePosition());
    }
}