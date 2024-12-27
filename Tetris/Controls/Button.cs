using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Tetris.Controls
{
  public class Button
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

    public Color Colour { get; set; } = Color.White;

    public Action OnClick;

    public Button(Texture2D texture, Vector2 position, string text)
    {
      _texture = texture;
      _position = position;

      var padding = 10;
      _label = new Label(text, new Rectangle((int)_position.X + padding, (int)_position.Y + padding, _texture.Width - (padding * 2), _texture.Height - (padding * 2)));
    }

    public void SetText(string newText, Label.HorizonalAlignments? newAlignment = null, Label.VerticalAlignments? newVAlignment = null)
    {
      _label.SetText(newText, newAlignment, newVAlignment);
    }

    public void Update(GameTime gameTime)
    {
      Colour = Color.White;
      if (Game1.GameMouse.Interests(Rectangle))
      {
        if (Game1.GameMouse.IsLeftClicked)
        {
          OnClick?.Invoke();
        }
        Colour = Color.DarkGray;

        if (Game1.GameMouse.IsLeftDown)
        {
          Colour = Color.Gray;
        }
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_texture, _position, Colour);
      _label.Draw(spriteBatch);
    }
  }
}
