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
    class Particle : GameObject
    {
        private Texture2D mTexture;
        float mTimeout;
        float mRotation;

        public Particle(Texture2D texture, Vector2 initPos, float timeout)
            : base(null, initPos)
        {
            mTexture  = texture;
            mTimeout  = timeout;
            mRotation = 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mTimeout > 0)
            {
                int opacity = (int)(mTimeout / mTimeout * 255);
                spriteBatch.Draw(mTexture, mPosition, null, new Color(opacity, opacity, opacity, opacity),
                                mRotation, new Vector2(mTexture.Width / 2, mTexture.Height / 2), Camera.kPixelsToUnits, SpriteEffects.None, 0f);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (mTimeout > 0.0f)
            {
                mTimeout -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (mTimeout <= 0.0f)
                {
                    // remove thyself
                    GetGameWorld().RemoveGameObject(this);
                }
            }
        } // Update()
    } // class Particle
} // namespace ggj_resurrection
