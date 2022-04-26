using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tetris.Sprites;

namespace Tetris.Controls
{
  public class Label
  {
    public enum HorizonalAlignments
    {
      Center,
      Left,
      Right,
    }

    public enum VerticalAlignments
    {
      Top,
      Middle,
      Bottom,
    }

    private string _text;
    public readonly Rectangle Box;
    private float _scale;
    private HorizonalAlignments _hAlignment;
    private VerticalAlignments _vAlignment;

    private List<Sprite> _sprites = new List<Sprite>();

    public Label(string text, Rectangle box, HorizonalAlignments alignment = HorizonalAlignments.Center, VerticalAlignments vAlignment = VerticalAlignments.Middle)
    {
      Box = box;
      _scale = 1f;
      _hAlignment = alignment;
      _vAlignment = vAlignment;

      SetText(text, _hAlignment, _vAlignment);
    }

    public void SetText(string newText, HorizonalAlignments? newAlignment = null, VerticalAlignments? newVAlignment = null)
    {
      if (_text == newText)
        return;

      _sprites = new List<Sprite>();

      _text = newText;

      if (newAlignment != null)
        _hAlignment = newAlignment.Value;

      if (newVAlignment != null)
        _vAlignment = newVAlignment.Value;


      float charWidth;
      float charHeight;
      float incrementAmount;
      float width;
      float height;

      do
      {
        charWidth = Game1.Characters.FirstOrDefault().Value.Width * _scale; // The width should be the same for everything
        charHeight = Game1.Characters.FirstOrDefault().Value.Height * _scale;
        var spaceBetween = (4f * _scale);
        incrementAmount = charWidth + spaceBetween;

        width = (charWidth * newText.Length) + (spaceBetween * (newText.Length - 1));
        height = charHeight;

        _scale -= 0.01f;

      } while (width > Box.Width || height > Box.Height);

      var x = 0f;
      var y = 0f;

      switch (_vAlignment)
      {
        case VerticalAlignments.Top:
          y = Box.Y;
          break;
        case VerticalAlignments.Middle:
          y = (Box.Y + (Box.Height / 2)) - (charHeight / 2);
          break;
        case VerticalAlignments.Bottom:
          y = Box.Bottom - (charHeight);
          break;
        default:
          break;
      }

      switch (_hAlignment)
      {
        case HorizonalAlignments.Left:
          x = Box.X;
          break;
        case HorizonalAlignments.Center:
          x = (Box.X + (Box.Width / 2)) - ((_text.Length * incrementAmount) / 2) + 2;
          break;
        case HorizonalAlignments.Right:
          x = Box.Right - (_text.Length * incrementAmount);
          break;
        default:
          break;
      }

      var position = new Vector2(x, y);

      foreach (var character in _text)
      {
        var key = GetChar(character.ToString());

        if (key == " ")
        {
          position.X += incrementAmount;
          continue;
        }

        if (Game1.Characters.ContainsKey(key))
        {
          var texture = Game1.Characters[key];
          _sprites.Add(new Sprite(texture, position) { Scale = _scale });

          position.X += incrementAmount;
        }
      }
    }

    private string GetChar(string value)
    {
      var result = value.ToUpper();

      switch (result)
      {
        case ":":
          return "Colon";

        case "#":
          return "Hash";

        case "-":
          return "Dash";

        default:
          return result;
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (var sprite in _sprites)
      {
        sprite.Draw(spriteBatch);
      }
    }
  }
}
