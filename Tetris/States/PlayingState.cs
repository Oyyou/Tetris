using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tetris.Controls;
using Tetris.Manager;
using Tetris.Sprites;

namespace Tetris.States
{
  public class PlayingState : State
  {
    private enum States
    {
      Playing,
      LineBreaking,
      GameOver,
    }

    private const int _tileSize = 24;

    private Game1 _game;

    private ContentManager _content;

    private Map _map;

    private Sprite _background;

    private Tetromino _current;
    private Tetromino _next;

    private TetrominoManager _tetrominoManager;

    private List<Tetromino> _placedPieces = new List<Tetromino>();

    private Dictionary<string, TextBox> _textBoxes;

    private Dictionary<string, Label> _statLabels = new Dictionary<string, Label>();

    private List<Tetromino> _statPieces = new List<Tetromino>();

    private Score _score;

    private int _linesCleared;

    private int _level
    {
      get
      {
        return (int)Math.Floor(_linesCleared / 10d) + 1;
      }
    }

    private float _timer = 0f;
    private float _moveTimer = 0.3f;
    private bool _hardFall = false;

    private bool _lost = false;

    private float _normalDropSpeed
    {
      get
      {
        return 0.3f - (_level / 100f);
      }
    }

    private float _softDropSpeed
    {
      get
      {
        return _normalDropSpeed / 2f;
      }
    }

    private float _hardDropSpeed
    {
      get
      {
        return _softDropSpeed / 4f;
      }
    }

    public PlayingState(Game1 game)
    {
      _game = game;
    }

    public override void LoadContent(ContentManager content)
    {
      var mapX = Game1.CentreX - 120;
      var mapY = _tileSize * 6;
      var mapWidth = 10 * _tileSize;
      var mapHeight = 22 * _tileSize;

      _content = content;
      _map = new Map(10, 22)
      {
        Position = new Vector2(mapX, mapY),
      };

      _score = new Score(_game);

      _tetrominoManager = new TetrominoManager(_content, _map);

      _background = new Sprite(content.Load<Texture2D>("Background2"), new Vector2(0, 0));

      var levelTextBoxTexture = content.Load<Texture2D>("TextBoxes/Level");
      var scoreTextBoxTexture = content.Load<Texture2D>("TextBoxes/Score");
      var nextTextBoxTexture = content.Load<Texture2D>("TextBoxes/Next");
      var statsTextBoxTexture = content.Load<Texture2D>("TextBoxes/Stats");

      _textBoxes = new Dictionary<string, TextBox>()
      {
        { "Level", new TextBox(levelTextBoxTexture, new Vector2(Game1.CentreX - (levelTextBoxTexture.Width / 2), _tileSize * 3), "Lines-0")},
        { "Score", new TextBox(scoreTextBoxTexture, new Vector2(mapX + mapWidth + (_tileSize * 1), _tileSize * 1), "")},
        { "Next", new TextBox(nextTextBoxTexture, new Vector2((mapX + mapWidth) + _tileSize, mapY + (_tileSize * 5)), "Next", vAlignment: Label.VerticalAlignments.Top)},
        { "Stats", new TextBox(statsTextBoxTexture, new Vector2((mapX - statsTextBoxTexture.Width) - _tileSize, _tileSize * 6), "Stats", vAlignment: Label.VerticalAlignments.Top)},
      };

      var y = 36;

      foreach (var stat in _tetrominoManager.Stats)
      {
        var piece = stat.Value.GetPiece();
        piece.PositionOffset = new Vector2(-(_tileSize * 8), y);
        _statPieces.Add(piece);

        _statLabels.Add(stat.Key, new Label("0", new Rectangle((int)piece.Position.X + (_tileSize * 5), (int)piece.Position.Y + (_tileSize / 2), 48, 20), 1f, Label.HorizonalAlignments.Left));

        y += 72;
      }

      _current = _tetrominoManager.GetRandomBlock();
      _current.MapPoint = new Point(3, 0);

      SetNext();
    }

    private void SetNext()
    {
      _next = _tetrominoManager.GetRandomBlock();
      var panelWidth = 96;
      var x = (panelWidth / 2) - (_next.Rectangle.Width / 2);
      _next.PositionOffset = new Vector2(276 + x, 156);
    }

    public override void Update(GameTime gameTime)
    {
      _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (_lost)
      {
        foreach (var piece in _placedPieces)
        {
          piece.Flash(gameTime);
        }
        if (_timer >= 1.5f)
        {
          _game.ChangeState(new GameOverState(_game, new Models.HighScore() { Value = _score.Value }));
        }
        return;
      }

      if (_current.IsDone)
      {
        _current = _next;
        _current.PositionOffset = new Vector2(0, 0);
        _current.MapPoint = new Point(3, 0);

        SetNext();

        _hardFall = false;
        _moveTimer = _normalDropSpeed;
      }

      Input();

      if (_timer >= _moveTimer)
      {
        _timer = 0f;
        _current.MoveY();
        foreach (var block in _current.Blocks)
        {
          if (!_map.CanMove(block))
          {
            _lost = true;
            _timer = 0f;
          }
        }
      }

      UpdateMap();
    }

    private void UpdateMap()
    {
      if (!_current.IsDone)
        return;

      _placedPieces.Add(_current);
      foreach (var block in _current.Blocks)
      {
        _map.AddObject(block);
      }

      int linesCleared = 0;
      for (int y = _map.Height - 1; y >= 0; y--)
      {
        bool isLineFull = true;
        for (int x = 0; x < _map.Width; x++)
        {
          if (_map.GetPoint(x, y) == 0)
            isLineFull = false;
        }

        if (isLineFull)
        {
          linesCleared++;
          _map.Clear();
          for (int i = 0; i < _placedPieces.Count; i++)
          {
            var piece = _placedPieces[i];
            for (int j = 0; j < piece.Blocks.Count; j++)
            {
              var block = piece.Blocks[j];
              if (block.MapPoint.Y == y)
              {
                piece.Blocks.RemoveAt(j);
                j--;
              }
            }

            if (piece.Blocks.Count == 0)
            {
              _placedPieces.RemoveAt(i);
              i--;
            }
          }

          foreach (var piece in _placedPieces)
          {
            piece.Fall(y);

            foreach (var block in piece.Blocks)
              _map.AddObject(block);
          }
          y++;
        }
      }
      _linesCleared += linesCleared;
      if (linesCleared > 0)
      {
        _game.PlayLineClearedSoundEffect();
      }
      _textBoxes["Level"].SetText("Lines-" + _linesCleared);
      _score.Increment(_level, linesCleared);
      foreach (var label in _statLabels)
        label.Value.SetText(_tetrominoManager.Stats[label.Key].Appearances.ToString());
      _map.WriteMap();
    }

    private void Input()
    {
      if (Game1.GameKeyboard.IsKeyPressed(Keys.P) || Game1.GameKeyboard.IsKeyPressed(Keys.Escape))
      {
        _game.ChangeState(new PauseState(_game));
      }

      if (Game1.GameKeyboard.IsKeyPressed(Keys.Space))
      {
        _hardFall = true;
        _moveTimer = _hardDropSpeed;
      }

      // Don't allow anymore input if hard falling
      if (_hardFall)
        return;

      if (Game1.GameKeyboard.IsKeyPressed(Keys.E))
      {
        _current.RotateRight();
        _game.PlayRotateSoundEffect();
      }
      else if (Game1.GameKeyboard.IsKeyPressed(Keys.Q))
      {
        _current.RotateLeft();
        _game.PlayRotateSoundEffect();
      }

      if (Game1.GameKeyboard.IsKeyDown(Keys.S))
      {
        _moveTimer = _softDropSpeed;
      }
      else
      {
        _moveTimer = _normalDropSpeed;
      }

      if (Game1.GameKeyboard.IsKeyPressed(Keys.D))
      {
        _current.MoveX(1);
      }
      else if (Game1.GameKeyboard.IsKeyPressed(Keys.A))
      {
        _current.MoveX(-1);
      }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();

      _current.Draw(spriteBatch);

      foreach (var piece in _placedPieces)
        piece.Draw(spriteBatch);

      _background.Draw(spriteBatch);

      foreach (var textbox in _textBoxes)
        textbox.Value.Draw(spriteBatch);

      foreach (var label in _statLabels)
        label.Value.Draw(spriteBatch);

      foreach (var piece in _statPieces)
        piece.Draw(spriteBatch);

      _next.Draw(spriteBatch);
      _score.Draw(spriteBatch);

      spriteBatch.End();
    }
  }
}
