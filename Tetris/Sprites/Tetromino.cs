using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris.Sprites
{
  public class Tetromino : IMappable
  {
    private Texture2D _texture;
    private Map _map;
    private int[,] _shape;
    private Rectangle _mapRectangle;
    private float _timer = 0f;

    public List<Block> Blocks { get; private set; } = new List<Block>();

    public Vector2 Position
    {
      get
      {
        return _map.Position + new Vector2(MapPoint.X * _texture.Width, MapPoint.Y * _texture.Height) + PositionOffset;
      }
    }

    public Rectangle Rectangle
    {
      get
      {
        return new Rectangle((_mapRectangle.X * 24) + (int)Position.X, (_mapRectangle.Y * 24) + (int)Position.Y, _mapRectangle.Width * 24, _mapRectangle.Height * 24);
      }
    }

    /// <summary>
    /// Used for the 'next' piece
    /// </summary>
    public Vector2 PositionOffset { get; set; }

    public Color Colour { get; set; } = Color.White;

    public float Opacity { get; private set; } = 1f;

    public bool HasLanded { get; set; } = false;

    public Point MapPoint { get; set; }

    public Tetromino(Texture2D texture, Map map, int[,] shape)
    {
      _texture = texture;
      _map = map;
      _shape = shape;

      Blocks = GetBlocks(_shape);
    }

    private List<Block> GetBlocks(int[,] matrix)
    {
      var sprites = new List<Block>();

      var xAmount = matrix.GetLength(1);
      var yAmount = matrix.GetLength(0);

      var rectangle = new Rectangle();

      for (int y = 0; y < yAmount; y++)
      {
        for (int x = 0; x < xAmount; x++)
        {
          var value = matrix[y, x];
          if (value != 1)
            continue;

          sprites.Add(new Block(this, _texture, new Point(x, y)));

          var currRec = new Rectangle(x, y, 1, 1);

          if (rectangle == Rectangle.Empty)
          {
            rectangle = currRec;
          }
          else
          {
            if (currRec.X < rectangle.X)
              rectangle.X = currRec.X;

            if (currRec.Y < rectangle.Y)
              rectangle.Y = currRec.Y;

            if (currRec.Right > rectangle.Right)
              rectangle.Width = currRec.Right;

            if (currRec.Bottom > rectangle.Bottom)
              rectangle.Height = currRec.Bottom;
          }
        }
      }

      _mapRectangle = rectangle;

      return sprites;
    }

    public void RotateRight()
    {
      var oldShape = _shape;
      _shape = RotateMatrix(_shape);

      if (WillCollideOnRotate(_shape))
      {
        _shape = oldShape;
        return;
      }

      Blocks = GetBlocks(_shape);
    }

    public void RotateLeft()
    {
      var oldShape = _shape;
      for (int i = 0; i < 3; i++)
      {
        _shape = RotateMatrix(_shape);
      }

      if (WillCollideOnRotate(_shape))
      {
        _shape = oldShape;
        return;
      }

      Blocks = GetBlocks(_shape);
    }

    public void MoveX(int direction)
    {
      var oldPoint = MapPoint;
      MapPoint = new Point(MapPoint.X + direction, MapPoint.Y);

      var isValid = true;
      foreach (var block in Blocks)
      {
        if (!_map.CanMove(block))
          isValid = false;
      }

      if (!isValid)
      {
        MapPoint = oldPoint;
      }
    }

    public void MoveY()
    {
      var oldPoint = MapPoint;
      MapPoint = new Point(MapPoint.X, MapPoint.Y + 1);

      bool isValid = true;
      foreach (var block in Blocks)
      {
        if (!_map.CanMove(block))
          isValid = false;
      }

      if (!isValid)
      {
        HasLanded = true;
        MapPoint = oldPoint;
      }
    }

    public void Flash(GameTime gameTime)
    {
      _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
      if (_timer > 0.1f)
      {
        _timer = 0f;
        Opacity = Opacity == 0f ? 1f : 0f;
      }
    }

    public void Fall(int y)
    {
      foreach (var block in Blocks)
      {
        if (block.MapPoint.Y <= y)
        {
          var point = new Point(0, 1);
          block.Point += point;

          if (!_map.CanMove(block))
            block.Point -= point;
        }
      }
    }

    public bool WillCollideOnRotate(int[,] newShape)
    {
      var newBlocks = GetBlocks(newShape);

      foreach (var block in newBlocks)
      {
        if (!_map.CanMove(block))
          return true;
      }

      return false;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (var sprite in Blocks)
        sprite.Draw(spriteBatch);
    }

    // https://stackoverflow.com/questions/42519/how-do-you-rotate-a-two-dimensional-array
    static int[,] RotateMatrix(int[,] matrix)
    {
      var m = matrix.GetLength(1);
      var n = matrix.GetLength(0);

      int[,] rot = new int[m, n];

      for (int i = 0; i < n; i++)
        for (int j = 0; j < m; j++)
          rot[j, n - i - 1] = matrix[i, j];


      return rot;
    }
  }
}
