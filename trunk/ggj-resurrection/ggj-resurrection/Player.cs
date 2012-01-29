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
        float mMaxSpeed = .03f;
        float mMaxSpeed = 10;
        Color tempColor = Color.YellowGreen;
        List<SwordSlash> bats = new List<SwordSlash>();

        static private Texture2D mTexture;

        KeyboardState mCurrKeyboardState, mPrevKeyboardState;
        GamePadState mCurrControllerState, mPrevControllerState;

        public Player(World world, Vector2 initPos)   //this is never called. We need it for physics object
            : base(world, initPos)
        {
            mRadius = 1f;


            mBody = BodyFactory.CreateRectangle(mPhysicsWorld, 64f / 64f, 64f / 64f, 1f);
            mBody.BodyType = BodyType.Dynamic;

            mFixture = FixtureFactory.AttachRectangle(64f / 64f, 64f / 64f, 1f, new Vector2(.5f, .5f), mBody);
            mFixture.Body.CollisionCategories = Category.Cat1;
            mFixture.CollisionCategories = Category.Cat1;
            mFixture.CollidesWith = Category.All & ~Category.Cat1;
            mBody.OnCollision += playerOnCollision;
            mFixture.UserData = "Player";
            mFixture.Body.UserData = "Player";
        }

        ~Player()
        {
        }

        public bool playerOnCollision(Fixture one, Fixture two, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (two.Body.UserData.ToString() == "Sword")
            {
                tempColor = Color.Red;
                return false;
            }
            
            tempColor = Color.Red;
            return true;
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(mTexture, mPosition, Color.YellowGreen);
            spriteBatch.Draw(mTexture, mBody.Position, null, tempColor, mBody.Rotation,
                new Vector2(mTexture.Width / 2, mTexture.Height / 2),
                Camera.kPixelsToUnits, SpriteEffects.None, 0f);
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

                getStick.Y *= 1f;

                multiply = getStick;

                if (getStick.Length() > .065f )
                {
                    //mFixture.Body.ApplyLinearImpulse(multiply * mMaxSpeed);
                    mFixture.Body.LinearVelocity = (multiply * mMaxSpeed);
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
                    multiply.Y =  1f;
                }

                if (mCurrKeyboardState.IsKeyDown(Keys.Down))
                {
                    multiply.Y = -1f;
                }
                //mFixture.Body.ApplyLinearImpulse(multiply * mMaxSpeed);
                mFixture.Body.LinearVelocity = (multiply * mMaxSpeed);
            //}

            if (multiply.Length() > 0)
            {
                multiply.Normalize();
                mDirection = multiply;
            }

            Vector2 rightStick = mCurrControllerState.ThumbSticks.Right;
            rightStick.Y *= -1f;

            if (mCurrControllerState.IsConnected && rightStick.Length() > .25)
            {
                SwordSlash newSwordSlash = new SwordSlash(mPhysicsWorld, mPosition);
          
                newSwordSlash.SetPosition(mPosition + (100 * mDirection));
                newSwordSlash.SetVelocity(mBody.LinearVelocity);
                GetGameWorld().AddGameObject(newSwordSlash);
                if (bats.Count <= 4)
                {
                    Vector2 offset = mPosition + (50 * rightStick);
                    SwordSlash newSwordSlash = new SwordSlash(mPhysicsWorld, offset);
                    newSwordSlash.setRotation(rightStick);
                    newSwordSlash.SetPosition(mPosition + (100 * mDirection));
                    newSwordSlash.SetVelocity(mBody.LinearVelocity);
                    bats.Add(newSwordSlash);
                    GetGameWorld().AddGameObject(newSwordSlash);
                }

                else
                {
                    SwordSlash apply = bats.ElementAt(0);
                    apply.setAngularVelocity(bats.ElementAt(2), bats.ElementAt(1));
                    bats.RemoveAt(0);
                   /* bats.ForEach(delegate(SwordSlash curr)
                    {
                        if (curr.isTimedOut())
                        {
                            bats.Remove(curr);
                        }

                    });*/
                    //bats.RemoveAt(0);
                }

                

            }

            const float speed = 300.0f;
            //mPosition += speed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
            mPosition = mBody.Position * 64f;       //converts Body.Position (meters) into pixels
        }

        public static void LoadData(Game myGame)
        {
            mTexture = myGame.Content.Load<Texture2D>("enemySprites/monster");
      
            // fixture load to initial position;
        }
       
    }
}
