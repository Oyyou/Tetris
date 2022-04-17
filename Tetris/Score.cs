using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tetris.Controls;

namespace Tetris
{
  public class Score
  {
    private readonly Label _topTextLabel;
    private readonly Label _topValueLabel;

    private readonly Label _scoreTextLabel;
    private readonly Label _scoreValueLabel;

    public int Value { get; private set; }

    public Vector2 Position { get; private set; }

    public Score(Game1 game)
    {
      _topTextLabel = new Label("Top", new Rectangle(800, 40, 92, 20), 0.85f, Label.HorizonalAlignments.Left);
      _topValueLabel = new Label(game.HighScoreManager.GetTopScore().ToString().PadLeft(6, '0'), new Rectangle(800, 60, 92, 20), 0.85f, Label.HorizonalAlignments.Right);

      _scoreTextLabel = new Label("Score", new Rectangle(800, 90, 92, 20), 0.85f, Label.HorizonalAlignments.Left);
      _scoreValueLabel = new Label("000000", new Rectangle(800, 110, 92, 20), 0.85f, Label.HorizonalAlignments.Right);
    }

    public void Increment(int level, int linesCleared)
    {
      if (linesCleared == 0)
        return;

      int amount;
      switch (linesCleared)
      {
        case 1:
          amount = 40;
          break;
        case 2:
          amount = 100;
          break;
        case 3:
          amount = 300;
          break;
        case 4:
          amount = 1200;
          break;
        default:
          throw new ApplicationException($"We have somehow cleared '{linesCleared}' lines!?");
      }

      Value += (amount * level);

      _scoreValueLabel.SetText(Value.ToString().PadLeft(6, '0'));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      _topTextLabel.Draw(spriteBatch);
      _topValueLabel.Draw(spriteBatch);

      _scoreTextLabel.Draw(spriteBatch);
      _scoreValueLabel.Draw(spriteBatch);
    }
  }
}
