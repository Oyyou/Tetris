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

    public Settings Settings;

    public SettingsManager()
    {
      Settings = new Settings();
      _serializer = new XmlSerializer(typeof(Settings));
      Load();
    }

    private void Load()
    {
      if (!File.Exists(_path))
        return;

      using (var reader = new StreamReader(_path))
      {
        Settings = (Settings)_serializer.Deserialize(reader);
      }
    }

    public void Save()
    {
      using (var writer = new StreamWriter(_path))
      {
        _serializer.Serialize(writer, Settings);
      }
    }
  }
}
