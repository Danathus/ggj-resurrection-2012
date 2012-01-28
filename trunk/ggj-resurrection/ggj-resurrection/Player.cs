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
    public class Player : GameObject
    {
        KeyboardState mCurrKeyboardState, mPrevKeyboardState;
        float mSlashTimeout;
        const float mMaxSlashTimeout = 0.5f;

        public Player(GraphicsDeviceManager gdm, World world)
            : base(gdm, world)
        {
            mPrevKeyboardState = mCurrKeyboardState = Keyboard.GetState();
            mSlashTimeout = 0.0f;
        }

        ~Player()
        {
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mPosition, Color.YellowGreen);
            // draw sword slash
            if (mSlashTimeout > 0)
            {
                spriteBatch.Draw(mTexture, mPosition + 50 * mDirection, new Color(255, 255, 255, mSlashTimeout / mMaxSlashTimeout * 255));
            }
        }

        public override void Update(GameTime gameTime)
        {
            mPrevKeyboardState = mCurrKeyboardState;
            mCurrKeyboardState = Keyboard.GetState();

            Vector2 direction = new Vector2(0, 0);
            if (mCurrKeyboardState.IsKeyDown(Keys.Right)) direction.X += 1.0f;
            if (mCurrKeyboardState.IsKeyDown(Keys.Left))  direction.X -= 1.0f;
            if (mCurrKeyboardState.IsKeyDown(Keys.Up))    direction.Y -= 1.0f;
            if (mCurrKeyboardState.IsKeyDown(Keys.Down))  direction.Y += 1.0f;
            if (direction.Length() > 0)
            {
                direction.Normalize();
                mDirection = direction;
            }

            if (mCurrKeyboardState.IsKeyDown(Keys.Z) && mPrevKeyboardState.IsKeyDown(Keys.Z))
            {
                mSlashTimeout = mMaxSlashTimeout;
            }

            mSlashTimeout -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (mSlashTimeout < 0.0f) mSlashTimeout = 0.0f;

            const float speed = 300.0f;
            mPosition += speed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void LoadData(Game myGame)
        {
            mTexture = myGame.Content.Load<Texture2D>("monster");
        }
       
    }
}
