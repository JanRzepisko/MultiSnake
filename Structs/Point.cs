
namespace MultiSnake.Structs;

public class Point
{
    public int X { get; }
    public int Y { get; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point MinusOnePosition()
    {
        return new Point(Y, X - 1);
    }
}