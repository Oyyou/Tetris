using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Sprites
{
  public class Sprite
  {
    private readonly Texture2D _texture;

    public Vector2 Position;

    public float Scale { get; set; } = 1f;

    public Sprite(Texture2D texture, Vector2 position)
    {
      _texture = texture;
      Position = position;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_texture, Position, null, Color.White, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 0f);
    }
  }
}
