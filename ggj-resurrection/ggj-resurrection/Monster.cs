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
        float mHealth;
        Color tempColor = Color.White;
        bool isHit = false;

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
            
            //Body = BodyFactory.CreateRectangle(mPhysicsWorld, 1f, 1f, .0125f);
            //mBody.BodyType = BodyType.Dynamic;
            mFixture = FixtureFactory.AttachRectangle(1f, 1f, .0125f, new Vector2(0,0), new Body(mPhysicsWorld));
            mFixture.Body.BodyType = BodyType.Dynamic;
            mFixture.CollisionCategories = Category.Cat3;
            mFixture.Body.OnCollision += monsterOnCollision;
           
            //Correct for meters vs pixels
            mFixture.Body.Position = new Vector2(mPosition.X, mPosition.Y);
            //mFixture.Body.UserData = "Monster";
            mFixture.Body.UserData = "Monster";
            mFixture.UserData = "Monster";
            setRandDirection();

            //reference to the player
            mPlayer = player;

            // hit points
            mHealth = 3;
        }

        Vector2 getKnockBack(Fixture a, Fixture b)
        {
            Vector2 apos, bpos;
            apos = a.Body.Position;
            apos.Y *= -1;
            bpos = b.Body.Position;
            bpos.Y *= -1;

            Vector2 difference = bpos - apos;
            difference.Normalize();
            difference *= 5f;

            return difference;

        }

        public virtual bool monsterOnCollision(Fixture one, Fixture two, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            tempColor = Color.Red;

            if ((String)two.Body.UserData == "Sword")
            {
               // --mHealth;
                //mFixture.Body.ApplyLinearImpulse(new Vector2(5f, 5f));
                Vector2 forceOfHit = getKnockBack(one, two);
                one.Body.LinearDamping = .01f;
                mFixture.Body.ApplyLinearImpulse(forceOfHit * 500);
               two.Body.ResetDynamics();

                isHit = true;

            }

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
            
            mFixture.Body.ResetDynamics();
            mFixture.Body.Rotation = 0f;
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

            mFixture.Body.ApplyLinearImpulse(multiply * mMaxSpeed * .03f);
            //mFixture.Body.LinearVelocity = (multiply * mMaxSpeed);

            if (mHealth <= 0)
            {
                GetGameWorld().RemoveGameObject(this);
            }
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
