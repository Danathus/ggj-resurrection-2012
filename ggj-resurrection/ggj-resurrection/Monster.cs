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
        float mMaxSpeed = 5;
        Color tempColor = Color.White;

        public enum DIRECTION { UP, DOWN, LEFT, RIGHT } //Enum for direction of the char
        private DIRECTION currentDirection;
        private double timeElapsed;
        static Random mRand = new Random();
        protected Player mPlayer;

        public Monster(World world, Vector2 initPos, Player player)
            : base(world, initPos)
        {
            timeElapsed = 0;
            mRadius = 1;
            
            mBody = BodyFactory.CreateCircle(mPhysicsWorld, 1f, 1f);
            mBody.BodyType = BodyType.Dynamic;
            mFixture = FixtureFactory.AttachCircle(mRadius, 1f, mBody);
            mFixture.CollisionCategories = Category.Cat3;
           mBody.OnCollision += monsterOnCollision;
           
            //Correct for meters vs pixels
            mBody.Position = mPosition;
            mBody.UserData = "Monster";
            setRandDirection();

            //reference to the player
            mPlayer = player;
        }

        
        public virtual bool monsterOnCollision(Fixture one, Fixture two, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            tempColor = Color.Red;
            return true;
        }

        ~Monster()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {   
        }

        public override void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeElapsed > 1000)
            {
                timeElapsed = 0;
                setRandDirection();
            }

            Vector2 multiply = new Vector2(0, 0);
            
            switch (currentDirection)
            {

                case DIRECTION.UP:
                    multiply.Y = 1f; 
                    break;

                
                case DIRECTION.RIGHT:
                    multiply.X = 1f;
                    break;

                
                case DIRECTION.DOWN:
                    multiply.Y = -1f;
                    break;

                
                case DIRECTION.LEFT:
                    multiply.X = -1f;
                    break;
            }

            //mFixture.Body.ApplyLinearImpulse(multiply * mMaxSpeed);
            mFixture.Body.LinearVelocity = (multiply * mMaxSpeed);
        }

        public static void LoadData(Game myGame)
        {
        }

        private void setRandDirection() {
                
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
    }
}
