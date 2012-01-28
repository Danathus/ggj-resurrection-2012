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

        public Player(World world)   //this is never called. We need it for physics object
            : base(world)
        {
            mPrevKeyboardState = mCurrKeyboardState = Keyboard.GetState();

        
            
        }

        ~Player()
        {
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(mTexture, mPosition, Color.YellowGreen);
            spriteBatch.Draw(mTexture, mBody.Position * 64f, null, Color.YellowGreen, mBody.Rotation, new Vector2(mTexture.Width / 2, mTexture.Height / 2), 1f, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            mPrevKeyboardState = mCurrKeyboardState;
            mCurrKeyboardState = Keyboard.GetState();

            Vector2 direction = new Vector2(0, 0);
            if (mCurrKeyboardState.IsKeyDown(Keys.Right)) direction.X += 1.0f;      //direction needs to be LinearVelocity
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
                SwordSlash newSwordSlash = new SwordSlash(mPhysicsWorld);
                newSwordSlash.SetPosition(mPosition + 50 * mDirection);
                GetGameWorld().AddGameObject(newSwordSlash);
            }

            const float speed = 300.0f;
            mPosition += speed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void LoadData(Game myGame)
        {
            mTexture = myGame.Content.Load<Texture2D>("monster");

            // fixture load to initial position;
            mBody = BodyFactory.CreateCircle(mPhysicsWorld, 50f / 64f, 1f, new Vector2(400f / 64f, 300f / 64f));
            mBody.BodyType = BodyType.Dynamic;

            
        }
       
    }
}
