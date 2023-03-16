using MultiSnake.Interfaces;

namespace MultiSnake.Structs;

public class Snake : IPlayer
{
    public string Name;
    List<Point> Positions = new List<Point>();

    public void Move(Point position)
    {
        List<Point> newPositions = new List<Point>();
        newPositions.Add(position);
        foreach (var item in Positions)
        {
            newPositions.Add(item);   
        }

        Positions = newPositions;
    }

    public void AddElement()
    {
        Positions.Add(Positions.Last().MinusOnePosition());
    }
}