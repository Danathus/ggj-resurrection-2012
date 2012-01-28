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
        Vector2 maxSpeed = new Vector2(.01f,.01f);
        

        static private Texture2D mTexture;

        KeyboardState mCurrKeyboardState, mPrevKeyboardState;
        GamePadState mCurrControllerState, mPrevControllerState;

        public Player(World world)   //this is never called. We need it for physics object
            : base(world)
        {
            mBody = BodyFactory.CreateCircle(mPhysicsWorld, 50f / 64f, 1f, new Vector2(400f / 64f, 300f / 64f));
            mBody.BodyType = BodyType.Dynamic;
            
        }

        ~Player()
        {
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(mTexture, mPosition, Color.YellowGreen);
            spriteBatch.Draw(mTexture, mPosition, null, Color.YellowGreen, mBody.Rotation, new Vector2(mTexture.Width / 2, mTexture.Height / 2), 1f, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            mPrevKeyboardState = mCurrKeyboardState;
            mCurrKeyboardState = Keyboard.GetState();
            mPrevControllerState = mCurrControllerState;
            mCurrControllerState = GamePad.GetState(PlayerIndex.One);

            Vector2 direction = new Vector2(0, 0);
            Vector2 multiply = new Vector2(0,0);
            mBody.LinearVelocity = new Vector2(0, 0);
            Vector2 getStick;
            if (mCurrControllerState.IsConnected)
            {
                getStick.X = mCurrControllerState.ThumbSticks.Left.X;
                getStick.Y = mCurrControllerState.ThumbSticks.Left.Y;

                getStick.Y *= -1f;

                if (getStick.Length() > .065f )
                {
                    mBody.LinearVelocity = (getStick * maxSpeed);
                }

            }

            else
            {
                if (mCurrKeyboardState.IsKeyDown(Keys.Right))
                {
                    //Vector2 velocity = new Vector2(1, 0);
                    multiply.X = 1f;
                    mBody.LinearVelocity = (multiply * maxSpeed);
                }//direction needs to be LinearVelocity

                if (mCurrKeyboardState.IsKeyDown(Keys.Left))
                {
                    multiply.X = -1f;
                    mBody.LinearVelocity = (multiply * maxSpeed);
                }

                if (mCurrKeyboardState.IsKeyDown(Keys.Up))
                {
                    multiply.Y = -1f; ;
                    mBody.LinearVelocity = (multiply * maxSpeed);
                }

                if (mCurrKeyboardState.IsKeyDown(Keys.Down))
                {
                    multiply.Y = 1f;
                    mBody.LinearVelocity = (multiply * maxSpeed);
                }

            }

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
            //mPosition += speed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
            mPosition = mBody.Position * 64f;       //converts Body.Position (meters) into pixels
        }

        public static void LoadData(Game myGame)
        {
            mTexture = myGame.Content.Load<Texture2D>("monster");

            // fixture load to initial position;
         
            
        }
       
    }
}
