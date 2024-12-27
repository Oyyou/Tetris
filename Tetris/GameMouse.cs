using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
  public class GameMouse
  {
    private MouseState _previousMouse;
    private MouseState _currentMouse;

    public Rectangle Rectangle
    {
      get
      {
        return new Rectangle(_currentMouse.Position, new Point(1, 1));
      }
    }

    public bool IsLeftClicked
    {
      get
      {
        return _previousMouse.LeftButton == ButtonState.Pressed &&
          _currentMouse.LeftButton == ButtonState.Released;
      }
    }

    public bool IsLeftDown
    {
      get
      {
        return _currentMouse.LeftButton == ButtonState.Pressed;
      }
    }

    public bool IsRightClicked
    {
      get
      {
        return _previousMouse.RightButton == ButtonState.Pressed &&
          _currentMouse.RightButton == ButtonState.Released;
      }
    }

    public bool IsRightDown
    {
      get
      {
        return _currentMouse.RightButton == ButtonState.Pressed;
      }
    }

    public void Update()
    {
      _previousMouse = _currentMouse;
      _currentMouse = Mouse.GetState();
    }

    public bool Interests(Rectangle rectangle)
    {
      return this.Rectangle.Intersects(rectangle);
    }
  }
}
