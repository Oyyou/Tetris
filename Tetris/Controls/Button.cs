using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Tetris.Controls
{
  public class Button
  {
    private Texture2D _texture;
    private Vector2 _position;

    private Label _label;

    public Rectangle Rectangle
    {
      get
      {
        return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
      }
    }

    public Color Colour { get; set; } = Color.White;

    public Action OnClick;

    public Button(Texture2D texture, Vector2 position, string text, float textScale = 1f)
    {
      _texture = texture;
      _position = position;

      _label = new Label(text, new Rectangle((int)position.X, (int)position.Y, _texture.Width, _texture.Height), textScale);
    }

    public void SetText(string newText, float? scale = null, Label.HorizonalAlignments? newAlignment = null, Label.VerticalAlignments? newVAlignment = null)
    {
      _label.SetText(newText, scale, newAlignment, newVAlignment);
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
