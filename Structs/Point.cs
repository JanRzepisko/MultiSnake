namespace MultiSnake.Structs;

public class Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    private int X { get; set; }
    private int Y { get; set; }

    public Point MinusOnePosition()
    {
        return new Point(X -1, Y);
    }
    
    
    public void RandomPoint()
    {
        Random r = new Random();
        X = r.Next(1, 29);
        Y = r.Next(1, 29);
    }
}