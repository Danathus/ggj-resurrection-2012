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
    class Snake : Monster
    {
        static private Texture2D mTexture;
        Color tempColor = Color.White;
             

        public Snake(World world, Vector2 initPos, Player player)
            : base(world, initPos, player)
        {
           mPlayer = player;
        }

        ~Snake()
        {
        }

        public override void Draw(SpriteBatch spriteBatch) {
            //float proximity = Vector2.Distance(mBody.Position, mPlayer.GetPosition());
           
            spriteBatch.Draw(mTexture, mFixture.Body.Position, null, tempColor, 0f, new Vector2(mTexture.Width / 2, mTexture.Height / 2), Camera.kPixelsToUnits, SpriteEffects.None, 0f);
          
        }
        
        new public static void LoadData(Game myGame)
        {
            mTexture = myGame.Content.Load<Texture2D>("enemySprites/Sentinel");
        }
    }
        
}
