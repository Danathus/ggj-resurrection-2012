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
        Player mPlayer;

        public Monster(World world, Vector2 initPos, Player player)
            : base(world, initPos)
        {
            timeElapsed = 0;
            
            mBody = BodyFactory.CreateCircle(mPhysicsWorld, 50f / 64f, 1f);
            mBody.BodyType = BodyType.Dynamic;
            mFixture = FixtureFactory.AttachCircle(50f / 64f, 1f, mBody);
            mFixture.CollisionCategories = Category.Cat3;
            mBody.OnCollision += monsterOnCollision;
           
            //Correct for meters vs pixels
            mPosition /= 64f;
            mBody.Position = mPosition;
            mBody.UserData = "Monster";
            setRandDirection();

            //reference to the player
            mPlayer = player;
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
            
            float proximity = Vector2.Distance(mBody.Position, mPlayer.GetPosition());

            if (proximity < 1000)
            {
            spriteBatch.Draw(mTexture, mFixture.Body.Position * 64f, null, tempColor, 0f, new Vector2(mTexture.Width / 2, mTexture.Height / 2), 1f, SpriteEffects.None, 0f);
            }
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

            mFixture.Body.ApplyLinearImpulse(multiply * maxSpeed);

        }

        public static void LoadData(Game myGame)
        {
            mTexture = myGame.Content.Load<Texture2D>("monster");
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
