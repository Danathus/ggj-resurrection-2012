using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class Grave : GameObject
    {
        static Texture2D mTexture;
        public static Vector2 kFrameSizeInPixels = new Vector2(50, 60); // C# won't let me make this const, but please don't change!

        public Grave(World world, Vector2 initPos)   //this is never called. We need it for physics object
            : base(world, initPos)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 spriteOffset =
                new Vector2(kFrameSizeInPixels.X / 2, kFrameSizeInPixels.Y / 2)
                * Camera.kPixelsToUnits;
            spriteBatch.Draw(mTexture, spriteOffset, null, new Color(1f, 1f, 1f, 1f),
                0f, new Vector2(mTexture.Width / 2, mTexture.Height / 2), new Vector2(1f, -1f) * Camera.kPixelsToUnits, SpriteEffects.None, 0f);
        }
        public override void Update(GameTime gameTime)
        {
            //
        }

        public static void LoadData(Game myGame)
        {
            mTexture = myGame.Content.Load<Texture2D>("Grave");
        }
    }
}
