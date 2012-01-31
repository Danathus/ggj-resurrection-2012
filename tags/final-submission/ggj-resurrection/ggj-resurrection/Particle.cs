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
        private static Random mRand = new Random(); // a friendly hack, for convenience!

        private Texture2D mTexture;
        float mMaxTimeout, mTimeout;

        // these are variables you can freely edit after construction
        public Vector2 mVelocity;
        public float mRotation, mRotVel;
        public Vector2 mScale, mScaleVel;

        // helper for generating randomness!
        public static float Random(float minValue, float maxValue, float precision = 100)
        {
            return (float)mRand.Next(0, (int)((maxValue - minValue) * precision)) / precision + minValue;
        }

        public Particle(Texture2D texture, Vector2 initPos, float timeout)
            : base(null, initPos)
        {
            mTexture  = texture;
            mTimeout = mMaxTimeout = timeout;

            mVelocity = new Vector2(0, 0);
            mRotation = mRotVel = 0;
            mScale    = new Vector2(1, 1);
            mScaleVel = new Vector2(0, 0);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mTimeout > 0)
            {
                int opacity = (int)(mTimeout / mMaxTimeout * 255);
                spriteBatch.Draw(mTexture, mPosition, null, new Color(opacity, opacity, opacity, opacity),
                                mRotation, new Vector2(mTexture.Width / 2, mTexture.Height / 2), mScale * Camera.kPixelsToUnits, SpriteEffects.None, 0f);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (mTimeout > 0.0f)
            {
                mTimeout -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                SetPosition(GetPosition() + (float)gameTime.ElapsedGameTime.TotalSeconds * mVelocity);
                mRotation += (float)gameTime.ElapsedGameTime.TotalSeconds * mRotVel;
                mScale    += (float)gameTime.ElapsedGameTime.TotalSeconds * mScaleVel;

                if (mTimeout <= 0.0f)
                {
                    // remove thyself
                    GetGameWorld().RemoveGameObject(this);
                }
            }
        } // Update()
    } // class Particle
} // namespace ggj_resurrection
