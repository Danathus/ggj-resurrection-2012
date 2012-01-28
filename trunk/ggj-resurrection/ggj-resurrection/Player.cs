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
        Vector2 maxSpeed = new Vector2(.03f,.03f);
        Color tempColor = Color.YellowGreen;
        

        static private Texture2D mTexture;

        KeyboardState mCurrKeyboardState, mPrevKeyboardState;
        GamePadState mCurrControllerState, mPrevControllerState;

        public Player(World world)   //this is never called. We need it for physics object
            : base(world)
        {
            mRadius = 50f;
            

            mBody = BodyFactory.CreateCircle(mPhysicsWorld, mRadius / 64f, 1f, new Vector2(mPosition.X / 64f, mPosition.Y / 64f));
            mBody.BodyType = BodyType.Dynamic;
            
            mFixture = FixtureFactory.AttachCircle(mRadius / 64f, 1f, mBody);

            mFixture.CollisionCategories = Category.Cat1;
            mFixture.CollidesWith = Category.All & ~Category.Cat2;
            mBody.OnCollision += playerOnCollision;
        }

        ~Player()
        {
        }

        public bool playerOnCollision(Fixture one, Fixture two, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            /*if (one.Body.LinearVelocity.Length() == Vector2.Zero.Length())
            {
                tempColor = Color.Red;
                return false;
            }
            */
            tempColor = Color.Red;
            return true;
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(mTexture, mPosition, Color.YellowGreen);
            spriteBatch.Draw(mTexture, mPosition, null, tempColor, mBody.Rotation, new Vector2(mTexture.Width / 2, mTexture.Height / 2), 1f, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            tempColor = Color.YellowGreen;
            mPrevKeyboardState = mCurrKeyboardState;
            mCurrKeyboardState = Keyboard.GetState();
            mPrevControllerState = mCurrControllerState;
            mCurrControllerState = GamePad.GetState(PlayerIndex.One);

            Vector2 direction = new Vector2(0, 0);
            Vector2 multiply = new Vector2(0,0);
            mFixture.Body.LinearVelocity = new Vector2(0, 0);
            mFixture.Body.Rotation = 0;
            Vector2 getStick;


            if (mCurrControllerState.IsConnected)
            {
                getStick.X = mCurrControllerState.ThumbSticks.Left.X;
                getStick.Y = mCurrControllerState.ThumbSticks.Left.Y;

                getStick.Y *= -1f;

                multiply = getStick;

                if (getStick.Length() > .065f )
                {
                    mFixture.Body.ApplyLinearImpulse(multiply * maxSpeed);
                    //mFixture.Body.LinearVelocity = (multiply * maxSpeed);
                }

            }

            //else
            //{
                if (mCurrKeyboardState.IsKeyDown(Keys.Right))
                {
                    //Vector2 velocity = new Vector2(1, 0);
                    multiply.X = 1f;
                    
                }//direction needs to be LinearVelocity

                if (mCurrKeyboardState.IsKeyDown(Keys.Left))
                {
                    multiply.X = -1f;
                
                }

                if (mCurrKeyboardState.IsKeyDown(Keys.Up))
                {
                    multiply.Y = -1f; ;
                
                }

                if (mCurrKeyboardState.IsKeyDown(Keys.Down))
                {
                    multiply.Y = 1f;
                
                }
                mFixture.Body.ApplyLinearImpulse(multiply * maxSpeed);

            //}

            if (multiply.Length() > 0)
            {
                multiply.Normalize();
                mDirection = multiply;
            }

            if (mCurrKeyboardState.IsKeyDown(Keys.Z) && !mPrevKeyboardState.IsKeyDown(Keys.Z))
            {
                SwordSlash newSwordSlash = new SwordSlash(mPhysicsWorld);
          
                newSwordSlash.SetPosition(mPosition + (100 * mDirection));
                newSwordSlash.SetVelocity(mBody.LinearVelocity);
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
