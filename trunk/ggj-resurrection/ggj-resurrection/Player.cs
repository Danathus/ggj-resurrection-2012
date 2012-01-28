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
        static private Texture2D mTexture;

        KeyboardState mCurrKeyboardState, mPrevKeyboardState;

        public Player(GraphicsDeviceManager gdm, World world)
            : base(gdm, world)
        {
            mPrevKeyboardState = mCurrKeyboardState = Keyboard.GetState();
        }

        ~Player()
        {
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mPosition, Color.YellowGreen);
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

            if (mCurrKeyboardState.IsKeyDown(Keys.Z) && !mPrevKeyboardState.IsKeyDown(Keys.Z))
            {
                SwordSlash newSwordSlash = new SwordSlash(mGraphicsDeviceManager, mWorld);
                newSwordSlash.SetPosition(mPosition + 50 * mDirection);
                GetGameWorld().AddGameObject(newSwordSlash);
            }

            const float speed = 300.0f;
            mPosition += speed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public static void LoadData(Game myGame)
        {
            mTexture = myGame.Content.Load<Texture2D>("monster");
        }
       
    }
}
