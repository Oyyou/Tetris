using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Tetris.Manager;
using Tetris.Models;
using Tetris.Sprites;
using Tetris.States;

namespace Tetris
{
  public class Game1 : Game
  {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Sprite _background;

    private State _state
    {
      get
      {
        return _states.Peek();
      }
    }

    /// <summary>
    /// When popping, this is the state we're popping to
    /// </summary>
    private Type _prevStateType = null;

    /// <summary>
    /// The next state we're changing to (on next frame)
    /// </summary>
    private State _nextState = null;

    /// <summary>
    /// Go back a state
    /// </summary>
    private bool _pop = false;

    private Stack<State> _states = new();

    private Song _backgroundMusic;

    private SoundEffect _rotateSound;
    private SoundEffect _lineClearSound;

    public static Random Random;

    public static GameMouse GameMouse;
    public static GameKeyboard GameKeyboard;

    public static int ScreenWidth = 1280;
    public static int ScreenHeight = 720;

    public static Dictionary<string, Texture2D> Characters;

    public static int CentreX
    {
      get
      {
        return ScreenWidth / 2;
      }
    }

    public HighScoreManager HighScoreManager;

    private Settings _settings;
    public Settings Settings
    {
      get { return _settings; }
      set
      {
        _settings = value;
        OnSettingsChanged();
      }
    }

    private void OnSettingsChanged()
    {
      if (_graphics.IsFullScreen != Settings.IsFullScreen)
      {
        _graphics.ToggleFullScreen();
        _graphics.ApplyChanges();
      }
    }

    public Game1()
    {
      _graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    protected override void Initialize()
    {
      Random = new Random();
      GameMouse = new GameMouse();
      GameKeyboard = new GameKeyboard();

      Settings = LoadSettings();

      _graphics.PreferredBackBufferWidth = ScreenWidth;
      _graphics.PreferredBackBufferHeight = ScreenHeight;
      _graphics.ApplyChanges();

      base.Initialize();
    }

    public static Settings LoadSettings()
    {
      var settingsFiles = "settings.xml";

      if (!File.Exists(settingsFiles))
        return new Settings();

      XmlSerializer serializer = new XmlSerializer(typeof(Settings));

      using var stream = new StreamReader("settings.xml");

      return (Settings)serializer.Deserialize(stream);
    }

    public void SaveSettings(Settings settings)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(Settings));

      using var stream = new StreamWriter("settings.xml");

      serializer.Serialize(stream, settings);

      Settings = settings;

      MediaPlayer.Volume = Settings.MusicVolume;
      MediaPlayer.IsMuted = !Settings.HasMusic;
    }

    protected override void LoadContent()
    {
      _spriteBatch = new SpriteBatch(GraphicsDevice);

      Characters = Directory.GetFiles("Content/Fonts/Characters")
        .Select(c => Path.GetFileNameWithoutExtension(c))
        .ToDictionary(c => c, v => Content.Load<Texture2D>($"Fonts/Characters/{v}"));

      HighScoreManager = new HighScoreManager();

      _backgroundMusic = Content.Load<Song>("Music/Background");
      MediaPlayer.IsRepeating = true;
      MediaPlayer.Volume = Settings.MusicVolume;
      MediaPlayer.IsMuted = !Settings.HasMusic;
      MediaPlayer.Play(_backgroundMusic);

      _rotateSound = Content.Load<SoundEffect>($"Sounds/Rotate");
      _lineClearSound = Content.Load<SoundEffect>("Sounds/LineClear");
      _background = new Sprite(Content.Load<Texture2D>("Background"), new Vector2(0, 0));

      SetState(new MenuState(this));
    }

    private void SetState(State newState)
    {
      _states.Push(newState);

      if (!_state.Loaded)
      {
        _state.Loaded = true;
        _state.LoadContent(Content);
      }
    }

    public void ChangeState(State newState)
    {
      _nextState = newState;
    }

    public void GoBackState()
    {
      _pop = true;
    }

    public void GoBackTo(Type type)
    {
      _pop = true;
      _prevStateType = type;
    }

    public void PlayRotateSoundEffect()
    {
      if (!Settings.HasFx)
        return;

      _rotateSound.Play(Settings.FXVolume, 0f, 0f);
    }

    public void PlayLineClearedSoundEffect()
    {
      if (!Settings.HasFx)
        return;

      _lineClearSound.Play(Settings.FXVolume, 0f, 0f);
    }

    protected override void Update(GameTime gameTime)
    {
      while (_pop)
      {
        _states.Pop();

        if (_prevStateType == null || _state.GetType() == _prevStateType)
        {
          _prevStateType = null;
          _pop = false;
        }
      }

      GameMouse.Update();
      GameKeyboard.Update();

      if (_nextState != null)
      {
        SetState(_nextState);

        _nextState = null;
      }

      _state.Update(gameTime);

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Black);

      _spriteBatch.Begin();
      _background.Draw(_spriteBatch);
      _spriteBatch.End();

      _state.Draw(_spriteBatch);

      base.Draw(gameTime);
    }
  }
}
