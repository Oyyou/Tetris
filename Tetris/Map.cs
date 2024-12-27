using Microsoft.Xna.Framework;
using System;

namespace Tetris
{
  public interface IMappable
  {
    public Point MapPoint { get; }
  }

  public class Map
  {
    private readonly bool[,] _data;
    private Vector2 _position;

    public int Width => _data.GetLength(1);

    public int Height => _data.GetLength(0);

    public Map(int width, int height)
    {
      _data = new bool[height, width];

      WriteMap();
    }

    public void Clear()
    {
      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          _data[y, x] = false;
        }
      }
    }

    public bool GetPoint(int x, int y) => _data[y, x];

    public void WriteMap()
    {
      Console.Clear();
      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          Console.Write(_data[y, x] ? 1 : 0);
        }
        Console.WriteLine();
      }
    }

    public bool CanMove(IMappable obj)
    {
      if (!IsOnMap(obj))
        return false;

      if (ObjectCollides(obj))
        return false;

      return true;
    }

    public void AddObject(IMappable obj)
    {
      _data[obj.MapPoint.Y, obj.MapPoint.X] = true;
    }

    private bool ObjectCollides(IMappable obj)
    {
      return _data[obj.MapPoint.Y, obj.MapPoint.X];
    }

    private bool IsOnMap(IMappable obj)
    {
      if (obj.MapPoint.X < 0)
        return false;

      if (obj.MapPoint.Y < 0)
        return false;

      if (obj.MapPoint.X >= Width)
        return false;

      if (obj.MapPoint.Y >= Height)
        return false;

      return true;
    }
  }
}
