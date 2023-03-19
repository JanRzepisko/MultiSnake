namespace MultiSnake.Structs;

public class Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }
    public int Y { get; }

    public Point MinusOnePosition()
    {
        return new Point(X -1, Y);
    }
}