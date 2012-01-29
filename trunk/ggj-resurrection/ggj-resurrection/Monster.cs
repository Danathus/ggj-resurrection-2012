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

        float mMaxSpeed = 0.05f;
        float mHealth;
        Color tempColor = Color.White;
        bool isHit = false;

        

        public enum DIRECTION { UP, DOWN, LEFT, RIGHT, NONE } //Enum for direction of the char
        private DIRECTION currentDirection;
        static Random mRand = new Random();
        protected Player mPlayer;
        protected Boolean mDeath;

        //Player hits monster
        static SoundEffectInstance mHitMonsterSEI;
        static SoundEffect mHitMonsterSnd;

        //Monsters hit each other
        static SoundEffect mMonMonCollSnd;

        private static float mMonMonCollVolume;
        protected static Texture2D mHackSmoke;



        public Monster(World world, Vector2 initPos, Player player)
            : base(world, initPos)
        {
            mRadius = 1;
           // setRandDirection();
            //Body = BodyFactory.CreateRectangle(mPhysicsWorld, 1f, 1f, .0125f);
            //mBody.BodyType = BodyType.Dynamic;


            //reference to the player
            mPlayer = player;

            // hit points
            //mHealth = 3;

            mMonMonCollVolume = .01f;
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
                //one.Body.LinearDamping = .01f;
                mFixture.Body.ApplyForce(forceOfHit * 3);
                //two.Body.ResetDynamics();
                mFixture.Body.Mass *= 0.8f;
                //mFixture.Body.
                isHit = true;
                mHealth -= 500;
                mHitMonsterSEI.Play();
            }
            else mMonMonCollSnd.Play(mMonMonCollVolume, -.5f, 0);

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
            
            //mFixture.Body.ResetDynamics();
            mFixture.Body.Rotation = 0f;

            Vector2 multiply = new Vector2(0, 0);
            


            //Vector2 multiply = new Vector2(0, 0);
         
            //mFixture.Body.LinearVelocity = (multiply * mMaxSpeed);


        }

        public static void LoadData(Game myGame)
        {
            mHitMonsterSnd = myGame.Content.Load<SoundEffect>("Audio/hitMonster");
            mHitMonsterSEI = mHitMonsterSnd.CreateInstance();
            mHitMonsterSEI.Volume = .35f;
            mHitMonsterSEI.Pitch = -.3f; //lower pitch of bat collision


            mHackSmoke = myGame.Content.Load<Texture2D>("Particles/SmokeParticleEffectSprite");
            mMonMonCollSnd = myGame.Content.Load<SoundEffect>("Audio/monsterMonsterColl");

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
