using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tetris.Controls;
using Tetris.Sprites;

namespace Tetris.States
{
  public class HighScoresState : State
  {
    private Game1 _game;

    private Sprite _background;

    private Label _titleLabel;

    private List<Label> _labels = new List<Label>();

    private Button _backButton;

    public HighScoresState(Game1 game)
    {
      _game = game;
    }

    public override void LoadContent(ContentManager content)
    {
      var width = 236;

      _background = new Sprite(content.Load<Texture2D>("Background2"), new Vector2(0, 0));

      _titleLabel = new Label("High Scores", new Rectangle(Game1.CentreX - (width / 2), 180, width, 30));

      _labels = _game.HighScoreManager.GetTopHighScores(10).Select((score, i) =>
        new Label($"#{(i + 1):00}:{score.Name}-{score.Value}", new Rectangle(Game1.CentreX - (width / 2), (_titleLabel.Box.Bottom + 30) + (34 * i), 216, 30), 1f, Label.HorizonalAlignments.Left)
      ).ToList();

      var buttonTexture = content.Load<Texture2D>("Button");
      _backButton = new Button(buttonTexture, new Vector2(Game1.CentreX - (buttonTexture.Width / 2), 600), "Back")
      {
        OnClick = () => _game.ChangeState(new MenuState(_game)),
      };
    }

    public override void Update(GameTime gameTime)
    {
      _backButton.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {

      spriteBatch.Begin();

      _background.Draw(spriteBatch);

      _backButton.Draw(spriteBatch);

      _titleLabel.Draw(spriteBatch);

      foreach (var label in _labels)
      {
        label.Draw(spriteBatch);
      }

      spriteBatch.End();
    }
  }
}
