using MultiSnake.Structs;

namespace MultiSnake.Interfaces;

public interface IPlayer
{
    public void Move(Point position);
    public void AddElement();
}