using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Tetris.Models;

namespace Tetris.Manager
{
  public class HighScoreManager
  {
    private const string _highScoresFile = "highscores.xml";
    private readonly XmlSerializer _serializer;

    public List<HighScore> HighScores { get; private set; } = new List<HighScore>();

    public HighScoreManager()
    {
      _serializer = new XmlSerializer(typeof(List<HighScore>));
      LoadHighScores();
    }

    private void LoadHighScores()
    {
      if (!File.Exists(_highScoresFile))
        return;

      using (var reader = new StreamReader(_highScoresFile))
      {
        HighScores = (List<HighScore>)_serializer.Deserialize(reader);
      }
    }

    public void AddHighScore(HighScore highScore)
    {
      HighScores.Add(highScore);
    }

    public void SaveHighScores()
    {
      using (var writer = new StreamWriter(_highScoresFile))
      {
        _serializer.Serialize(writer, HighScores);
      }
    }

    public int GetTopScore()
    {
      return GetTopHighScores(1).FirstOrDefault().Value;
    }

    public IEnumerable<HighScore> GetTopHighScores(int amount = 10)
    {
      return HighScores.OrderByDescending(c => c.Value).Take(amount);
    }
  }
}
