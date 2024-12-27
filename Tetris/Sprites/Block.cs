using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris.Sprites
{
  public class Block : IMappable
  {
    private readonly Tetromino _parent;

    private readonly Texture2D _texture;

    public Point Point { get; set; }
    public Point MapPoint => _parent.MapPoint + Point;
    public Vector2 Position => _parent.Position + new Vector2(Point.X * _texture.Width, Point.Y * _texture.Height);

    public Block(Tetromino parent, Texture2D texture, Point point)
    {
      _parent = parent;
      _texture = texture;
      Point = point;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_texture, Position, _parent.Colour * _parent.Opacity);
    }
  }
}
