using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tetris.Sprites;

namespace Tetris.Manager
{
  public class TetrominoManager
  {
    public class TetrominoObject
    {
      public readonly Func<Tetromino> GetPiece;

      public int Appearances { get; set; } = 0;

      public TetrominoObject(Func<Tetromino> getPiece)
      {
        GetPiece = getPiece;
      }

      public override string ToString()
      {
        return Appearances.ToString();
      }
    }

    private ContentManager _content;

    private Map _map;

    private string _lastPiece = "";

    public Dictionary<string, TetrominoObject> Stats { get; private set; } = new Dictionary<string, TetrominoObject>();

    public TetrominoManager(ContentManager content, Map map)
    {
      _content = content;
      _map = map;

      Stats = new Dictionary<string, TetrominoObject>()
      {
        { "OrangeRicky", new TetrominoObject(GetOrangeRicky) },
        { "Hero", new TetrominoObject(GetHero) },
        { "BlueRicky", new TetrominoObject(GetBlueRicky) },
        { "Teewee", new TetrominoObject(GetTeewee) },
        { "ClevelandZ", new TetrominoObject(GetClevelandZ) },
        { "RhodeIslandZ", new TetrominoObject(GetRhodeIslandZ) },
        { "Smashboy", new TetrominoObject(GetSmashboy) }
      };
    }

    public Tetromino GetRandomBlock()
    {
      string result = null;

    Start:
      var total = Stats.Sum(c => c.Value.Appearances + 10);

      var random = Game1.Random.Next(0, total);

      var chance = 0;
      foreach (var piece in Stats)
      {
        var amount = piece.Value.Appearances + 10;
        chance += amount;
        if (random < chance)
        {

          result = piece.Key;
          break;
        }
      }

      // Re-roll once if we get the same piece twice in a row
      if (_lastPiece == result)
      {
        _lastPiece = "";
        goto Start;
      }

      Stats[result].Appearances++;

      _lastPiece = result;
      return Stats[result].GetPiece();
    }

    #region Tetrominoes

    private Tetromino GetOrangeRicky()
    {
      return new Tetromino(_content.Load<Texture2D>("Block"), _map, new int[,] {
        { 0, 0, 1 },
        { 1, 1, 1 },
        { 0, 0, 0 },
      })
      {
        Colour = new Color(254, 170, 0),// Color.Orange,
      };
    }

    private Tetromino GetHero()
    {
      return new Tetromino(_content.Load<Texture2D>("Block"), _map, new int[,] {
        { 0, 0, 0, 0 },
        { 1, 1, 1, 1 },
        { 0, 0, 0, 0 },
        { 0, 0, 0, 0 },
      })
      {
        Colour = new Color(1, 255, 255), //Color.LightBlue,
      };
    }

    private Tetromino GetBlueRicky()
    {
      return new Tetromino(_content.Load<Texture2D>("Block"), _map, new int[,] {
        { 1, 0, 0 },
        { 1, 1, 1 },
        { 0, 0, 0 },
      })
      {
        Colour = new Color(0, 0, 255), //Color.Blue,
      };
    }

    private Tetromino GetTeewee()
    {
      return new Tetromino(_content.Load<Texture2D>("Block"), _map, new int[,] {
        { 0, 1, 0 },
        { 1, 1, 1 },
        { 0, 0, 0 },
      })
      {
        Colour = new Color(151, 1, 254), //Color.Purple,
      };
    }

    private Tetromino GetClevelandZ()
    {
      return new Tetromino(_content.Load<Texture2D>("Block"), _map, new int[,] {
        { 1, 1, 0 },
        { 0, 1, 1 },
        { 0, 0, 0 },
      })
      {
        Colour = new Color(255, 1, 0), //Color.Red,
      };
    }

    private Tetromino GetRhodeIslandZ()
    {
      return new Tetromino(_content.Load<Texture2D>("Block"), _map, new int[,] {
        { 0, 1, 1 },
        { 1, 1, 0 },
        { 0, 0, 0 },
      })
      {
        Colour = new Color(0, 255, 1), //Color.Green,
      };
    }

    private Tetromino GetSmashboy()
    {
      return new Tetromino(_content.Load<Texture2D>("Block"), _map, new int[,] {
        { 1, 1, },
        { 1, 1, },
      })
      {
        Colour = new Color(255, 255, 0), //Color.Yellow,
      };
    }

    #endregion
  }
}
