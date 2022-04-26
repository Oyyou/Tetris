using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Tetris.Models;

namespace Tetris.Manager
{
  public class SettingsManager
  {
    private const string _path = "settings.xml";
    private readonly XmlSerializer _serializer;

    public Settings Settings { get; private set; }

    public SettingsManager()
    {
      _serializer = new XmlSerializer(typeof(Settings));
      Settings = Load();
    }

    private Settings Load()
    {
      Settings result = new Settings();

      if (!File.Exists(_path))
        return result;

      using var reader = new StreamReader(_path);

      result = (Settings)_serializer.Deserialize(reader);

      return result;
    }

    public void SaveSettings(Settings settings)
    {
      using var stream = new StreamWriter("settings.xml");

      _serializer.Serialize(stream, settings);

      Settings = settings;

      MediaPlayer.Volume = Settings.MusicVolume;
      MediaPlayer.IsMuted = !Settings.HasMusic;
    }
  }
}
