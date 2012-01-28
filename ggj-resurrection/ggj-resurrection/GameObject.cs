using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ggj_resurrection
{
    public abstract class GameObject
    {
        public abstract void Draw();
        public abstract void Update(GameTime gameTime);
    };
}
