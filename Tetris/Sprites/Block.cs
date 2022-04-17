using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Sprites
{
  public class Block: IMappable
  {
    private readonly Tetromino _parent;

    private readonly Texture2D _texture;

    public Vector2 Position
    {
      get
      {
        return _parent.Position + new Vector2(_point.X * _texture.Width, _point.Y * _texture.Height);
      }
    }

    public Point _point;
    public Point MapPoint { get { return _parent.MapPoint + _point; } }

    public Block(Tetromino parent, Texture2D texture, Point point)
    {
      _parent = parent;
      _texture = texture;
      _point = point;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_texture, Position, _parent.Colour * _parent.Opacity);
    }
  }
}
