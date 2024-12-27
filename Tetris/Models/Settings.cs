namespace Tetris.Models
{
  public class Settings
  {
    public bool IsFullScreen { get; set; } = false;

    public bool HasMusic { get; set; } = true;

    public bool HasFx { get; set; } = true;

    public float FXVolume { get; set; } = 0.1f;

    public float MusicVolume { get; set; } = 0.1f;
  }
}
