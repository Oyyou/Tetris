﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tetris.Controls;

namespace Tetris.States
{
  public class PauseState : State
  {
    private Game1 _game;

    private List<Button> _buttons = new List<Button>();

    public PauseState(Game1 game)
    {
      _game = game;
    }

    public override void LoadContent(ContentManager content)
    {
      var buttonTexture = content.Load<Texture2D>("Button");
      var buttonStart = new Vector2(Game1.CentreX - (buttonTexture.Width / 2), 300);

      _buttons.Add(new Button(buttonTexture, buttonStart + new Vector2(0, (buttonTexture.Height + 10) * 0), "Continue") { OnClick = () => _game.GoBackState() });
      _buttons.Add(new Button(buttonTexture, buttonStart + new Vector2(0, (buttonTexture.Height + 10) * 1), "Quit to Menu") { OnClick = () => _game.GoBackTo(typeof(MenuState)) });
      _buttons.Add(new Button(buttonTexture, buttonStart + new Vector2(0, (buttonTexture.Height + 10) * 2), "Quit to Desktop") { OnClick = () => _game.Exit() });
    }

    public override void Update(GameTime gameTime)
    {
      foreach (var button in _buttons)
        button.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin(samplerState: SamplerState.PointClamp);

      foreach (var button in _buttons)
        button.Draw(spriteBatch);

      spriteBatch.End();
    }
  }
}
