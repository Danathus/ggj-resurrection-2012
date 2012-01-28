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

using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;

namespace ggj_resurrection
{
    //Snake class is currently untested
    class Snake : GameObject
    {
        private Monster snake;

        public Snake(World world, Vector2 initPos)
            : base(world, initPos)
        {

        }

        ~Snake()
        {
        }

        public override void Draw(SpriteBatch spriteBatch) { snake.Draw(spriteBatch); }
        public override void Update(GameTime gameTime) { snake.Update(gameTime);  }
    }
        
}
