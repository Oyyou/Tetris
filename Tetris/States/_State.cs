using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.States
{
  public abstract class State
  {
    public bool Loaded { get; set; } = false;

    public abstract void LoadContent(ContentManager content);

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(SpriteBatch spriteBatch);
  }
}
