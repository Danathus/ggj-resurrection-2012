using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ggj_resurrection
{
    public abstract class GameObject
    {
        public abstract void Draw(SpriteBatch SpriteBatch);
        public abstract void Update(GameTime gameTime);
        public abstract void LoadData(Game myGame);
    };
}
