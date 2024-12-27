using Microsoft.Xna.Framework.Input;

namespace Tetris
{
  public class GameKeyboard
  {
    private KeyboardState _previousKeyboard;
    private KeyboardState _currentKeyboard;

    public bool IsKeyPressed(Keys key)
    {
      return _previousKeyboard.IsKeyDown(key) && _currentKeyboard.IsKeyUp(key);
    }

    public bool IsKeyDown(Keys key)
    {
      return _currentKeyboard.IsKeyDown(key);
    }

    public Keys[] KeysPressed
    {
      get
      {
        return _currentKeyboard.GetPressedKeys();
      }
    }

    public void Update()
    {
      _previousKeyboard = _currentKeyboard;
      _currentKeyboard = Keyboard.GetState();
    }
  }
}
