﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris.Controls
{
  public class TextBox
  {
    private readonly Texture2D _texture;
    private readonly Vector2 _position;

    private readonly Label _label;

    public Rectangle Rectangle
    {
      get
      {
        return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
      }
    }

    public TextBox(Texture2D texture, Vector2 position, string text, Label.HorizonalAlignments hAlignment = Label.HorizonalAlignments.Center, Label.VerticalAlignments vAlignment = Label.VerticalAlignments.Middle)
    {
      var padding = 14;

      _texture = texture;
      _position = position;
      _label = new Label(text,
        new Rectangle(Rectangle.X + padding, Rectangle.Y + padding, Rectangle.Width - (padding * 2), Rectangle.Height - (padding * 2)),
        hAlignment,
        vAlignment
      );
    }

    public void SetText(string newText, Label.HorizonalAlignments? newAlignment = null, Label.VerticalAlignments? newVAlignment = null)
    {
      _label.SetText(newText, newAlignment, newVAlignment);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_texture, _position, Color.White);
      _label.Draw(spriteBatch);
    }
  }
}
