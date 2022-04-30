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
    private bool[,] _data;

    public Vector2 Position;

    public int Width
    {
      get
      {
        return _data.GetLength(1);
      }
    }

    public int Height
    {
      get
      {
        return _data.GetLength(0);
      }
    }

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

    public void ClearRow(int y)
    {
      for (int x = 0; x < Width; x++)
      {
        _data[y, x] = false;
      }
    }

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

    public bool IsOnMap(IMappable obj)
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

    public bool ObjectCollides(IMappable obj)
    {
      return _data[obj.MapPoint.Y, obj.MapPoint.X];
    }

    public void AddObject(IMappable obj)
    {
      _data[obj.MapPoint.Y, obj.MapPoint.X] = true;
    }

    public void RemoveObject(IMappable obj)
    {
      _data[obj.MapPoint.Y, obj.MapPoint.X] = false;
    }
  }
}
