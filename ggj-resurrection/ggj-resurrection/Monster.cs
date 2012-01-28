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
    class Monster : GameObject
    {
        static private Texture2D mTexture;

        Vector2 maxSpeed = new Vector2(.02f, .02f);
        Color tempColor = Color.White;

        public enum DIRECTION { UP, DOWN, LEFT, RIGHT } //Enum for direction of the char
        private DIRECTION currentDirection;
        private double timeElapsed;
        static Random mRand = new Random();

        public Monster(World world, Vector2 initPos)
            : base(world, initPos)
        {
            timeElapsed = 0;
            currentDirection = DIRECTION.RIGHT;
            mBody = BodyFactory.CreateCircle(mPhysicsWorld, 50f / 64f, 1f);
            mBody.BodyType = BodyType.Dynamic;
            mFixture = FixtureFactory.AttachCircle(50f / 64f, 1f, mBody);
            mFixture.CollisionCategories = Category.Cat3;
            mBody.OnCollision += monsterOnCollision;
            mPosition /= 64f;
            mBody.Position = mPosition;
            mBody.UserData = "Monster";
        }

        public bool monsterOnCollision(Fixture one, Fixture two, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            tempColor = Color.Red;
            return true;
        }

        ~Monster()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mFixture.Body.Position * 64f, null, tempColor, 0f, new Vector2(mTexture.Width / 2, mTexture.Height / 2), 1f, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeElapsed > 1000)
            {
                timeElapsed = 0;
                int randomNumber = mRand.Next(1, 5);
                switch (randomNumber)
                {
                    //up
                    case 1:
                        currentDirection = DIRECTION.UP;
                        break;
                        
                    //right
                    case 2:
                        currentDirection = DIRECTION.RIGHT;
                        break;

                    //down
                    case 3:
                        currentDirection = DIRECTION.DOWN;
                        break;

                    //left
                    case 4:
                        currentDirection = DIRECTION.LEFT;
                        break;
                }
            }

            Vector2 multiply = new Vector2(0, 0);
            
            switch (currentDirection)
            {

                case DIRECTION.UP:
                    if (mFixture.Body.Position.Y <= 500 / 64f) 
                    {
                        multiply.Y = 1f;
                      //  mPosition.Y += 3; 
                    }
                    break;

                
                case DIRECTION.RIGHT:
                    if (mFixture.Body.Position.X <= 700 / 64f)
                    {
                        multiply.X = 1f;
                    }
                    break;

                
                case DIRECTION.DOWN:
                    if (mFixture.Body.Position.Y >= 100 / 64f)
                    {
                        multiply.Y = -1f;
                    }
                    break;

                
                case DIRECTION.LEFT:
                    if (mFixture.Body.Position.X >= 100 / 64f)
                    {
                        multiply.X = -1f;
                    }
                    break;
            }

            mFixture.Body.ApplyLinearImpulse(multiply * maxSpeed);

        }

        public static void LoadData(Game myGame)    //i don't htink this should be static because every monster has a different "physics" body.
        {
            mTexture = myGame.Content.Load<Texture2D>("monster");
        }
    }
}
