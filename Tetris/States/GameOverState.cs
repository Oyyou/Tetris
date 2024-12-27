using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Tetris.Controls;
using Tetris.Models;

namespace Tetris.States
{
  public class GameOverState : State
  {
    private readonly Game1 _game;
    private readonly HighScore _score;

    private Button _continueButton;
    private Label _nameLabel;
    private List<Label> _labels = [];
    private string _name = "";
    private Keys? _pressedKey = null;

    public GameOverState(Game1 game, HighScore score)
    {
      _game = game;
      _score = score;
    }

    public override void LoadContent(ContentManager content)
    {
      var buttonTexture = content.Load<Texture2D>("Button");

      _continueButton = new Button(buttonTexture, new Vector2(Game1.CentreX - (buttonTexture.Width / 2), 600), "Continue")
      {
        OnClick = Continue
      };


      var width = 236;
      var x = Game1.CentreX - (width / 2);

      _nameLabel = new Label("---", new Rectangle(x, 350, width, 30));

      _labels =
      [
        new Label("Score:" + _score.Value, new Rectangle(x, 180, width, 30)),
        new Label("Please enter", new Rectangle(x, 300, width, 30)),
        new Label("your name:", new Rectangle(x, 320, width, 30)),
        _nameLabel
      ];
    }

    private void Continue()
    {
      _score.Name = _name;
      _game.HighScoreManager.AddHighScore(_score);
      _game.HighScoreManager.SaveHighScores();
      _game.ChangeState(new MenuState(_game));
    }

    public override void Update(GameTime gameTime)
    {
      _continueButton.Update(gameTime);

      if (Game1.GameKeyboard.KeysPressed.Length > 0)
      {
        _pressedKey = Game1.GameKeyboard.KeysPressed[0];
      }

      if (Game1.GameKeyboard.IsKeyPressed(Keys.Back))
      {
        if (_name.Length > 0)
        {
          _name = _name[0..^1]; // Ouch :(
        }
      }
      else if (Game1.GameKeyboard.IsKeyPressed(Keys.Enter))
      {
        Continue();
      }
      else // We do else otherwise we'll draw 'back' as the name ():))():) 
      {
        if (_pressedKey != null)
        {
          if (Game1.GameKeyboard.IsKeyPressed(_pressedKey.Value))
          {
            _name += _pressedKey.Value;
            if (_name.Length >= 3)
              _name = _name.Substring(0, 3);

            _pressedKey = null;
          }
        }
      }

      _nameLabel.SetText(_name.PadRight(3, '-'));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin(samplerState: SamplerState.PointClamp);

      _continueButton.Draw(spriteBatch);

      foreach (var label in _labels)
        label.Draw(spriteBatch);

      spriteBatch.End();
    }
  }
}
