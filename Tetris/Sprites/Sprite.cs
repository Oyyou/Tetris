using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris.Sprites
{
  public class Sprite
  {
    private readonly Texture2D _texture;
    private readonly Vector2 _position;

    public float Scale { get; set; } = 1f;

    public Sprite(Texture2D texture, Vector2 position)
    {
      _texture = texture;
      _position = position;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_texture, _position, null, Color.White, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 0f);
    }
  }
}
