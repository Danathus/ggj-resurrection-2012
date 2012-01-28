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
    class SwordSlash : GameObject
    {
        static private Texture2D mTexture;

        float mSlashTimeout;
        const float mMaxSlashTimeout = 0.5f;

        public SwordSlash(GraphicsDeviceManager gdm, World world)
            : base(gdm, world)
        {
            mSlashTimeout = mMaxSlashTimeout;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mSlashTimeout > 0)
            {
                spriteBatch.Draw(mTexture, mPosition, new Color(255, 255, 255, mSlashTimeout / mMaxSlashTimeout * 255));
            }
        }

        public override void Update(GameTime gameTime)
        {
            mSlashTimeout -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (mSlashTimeout < 0.0f)
            {
                mSlashTimeout = 0.0f;
                // todo: kill self
            }
        }

        public static void LoadData(Game myGame)
        {
            mTexture = myGame.Content.Load<Texture2D>("monster");
        }
    }
}
