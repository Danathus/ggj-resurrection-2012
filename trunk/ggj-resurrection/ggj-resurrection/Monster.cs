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
    class Monster : GameObject
    {
        public Monster(GraphicsDeviceManager gdm)
            : base(gdm)
        {
           //mSpriteBatch.Draw(mTexture, mPosition, Color.YellowGreen);
        }
        ~Monster()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
 
        }

        public override void LoadData(Game myGame)
        {

        }
    }
}
