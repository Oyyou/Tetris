using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tetris.Controls;
using Tetris.Sprites;

namespace Tetris.States
{
  public class MenuState : State
  {
    private Game1 _game;

    private List<Button> _buttons = new List<Button>();

    public MenuState(Game1 game)
    {
      _game = game;
    }

    public override void LoadContent(ContentManager content)
    {
      var buttonTexture = content.Load<Texture2D>("Button");
      var buttonStart = new Vector2(Game1.CentreX - (buttonTexture.Width / 2), 300);

      _buttons.Add(new Button(buttonTexture, buttonStart, "Start") { OnClick = () => _game.ChangeState(new PlayingState(_game)) });
      _buttons.Add(new Button(buttonTexture, buttonStart + new Vector2(0, (buttonTexture.Height + 10) * 1), "High Scores", 0.5f) { OnClick = () => _game.ChangeState(new HighScoresState(_game)) });
      _buttons.Add(new Button(buttonTexture, buttonStart + new Vector2(0, (buttonTexture.Height + 10) * 2), "Settings", 0.65f) { OnClick = () => _game.ChangeState(new SettingsState(_game)) });
      _buttons.Add(new Button(buttonTexture, buttonStart + new Vector2(0, (buttonTexture.Height + 10) * 3), "Quit") { OnClick = () => _game.Exit() });
    }

    public override void Update(GameTime gameTime)
    {
      foreach (var button in _buttons)
        button.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();

      foreach (var button in _buttons)
        button.Draw(spriteBatch);

      spriteBatch.End();
    }
  }
}
