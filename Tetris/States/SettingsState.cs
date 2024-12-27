using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tetris.Controls;
using Tetris.Models;

namespace Tetris.States
{
  public class SettingsState : State
  {
    private readonly Game1 _game;
    private readonly Settings _settings;

    private Dictionary<string, Button> _buttons = [];


    public SettingsState(Game1 game)
    {
      _game = game;
      _settings = _game.Settings;
    }

    public override void LoadContent(ContentManager content)
    {
      var buttonTexture = content.Load<Texture2D>("Button");

      var buttonStart = new Vector2(Game1.CentreX - (buttonTexture.Width / 2), 300);

      _buttons.Add("Screen", new Button(buttonTexture, buttonStart + new Vector2(0, (buttonTexture.Height + 10) * 0), GetFullScreenTitle()) { OnClick = OnScreenClicked });
      _buttons.Add("Music", new Button(buttonTexture, buttonStart + new Vector2(0, (buttonTexture.Height + 10) * 1), GetMusicTitle()) { OnClick = OnMusicClicked });
      _buttons.Add("FX", new Button(buttonTexture, buttonStart + new Vector2(0, (buttonTexture.Height + 10) * 2), GetFXTitle()) { OnClick = OnFxClicked });

      var lastButtonStart = new Vector2(Game1.CentreX - (buttonTexture.Width / 2), 540);
      _buttons.Add("Apply", new Button(buttonTexture, new Vector2(lastButtonStart.X, lastButtonStart.Y), "Apply") { OnClick = () => _game.SaveSettings(_settings) });
      _buttons.Add("Back", new Button(buttonTexture, new Vector2(lastButtonStart.X, lastButtonStart.Y + (buttonTexture.Height + 10)), "Back") { OnClick = () => _game.ChangeState(new MenuState(_game)) });
    }

    private string GetFullScreenTitle()
    {
      return _settings.IsFullScreen ? "Fullscreen" : "Windowed";
    }

    private string GetMusicTitle()
    {
      return $"Music:{(_game.Settings.HasMusic ? "on" : "off")}";
    }

    private string GetFXTitle()
    {
      return $"FX:{(_game.Settings.HasFx ? "on" : "off")}";
    }

    private void OnScreenClicked()
    {
      _settings.IsFullScreen = !_settings.IsFullScreen;

      _buttons["Screen"].SetText(GetFullScreenTitle());
    }

    private void OnMusicClicked()
    {
      _settings.HasMusic = !_settings.HasMusic;

      _buttons["Music"].SetText(GetMusicTitle());
    }

    private void OnFxClicked()
    {
      _settings.HasFx = !_settings.HasFx;

      _buttons["FX"].SetText(GetFXTitle());
    }

    public override void Update(GameTime gameTime)
    {
      foreach (var button in _buttons)
        button.Value.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();

      foreach (var button in _buttons)
        button.Value.Draw(spriteBatch);

      spriteBatch.End();
    }
  }
}
