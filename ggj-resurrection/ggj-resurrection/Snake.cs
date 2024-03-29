﻿using System;
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
    class Snake : Monster
    {
        //static private Texture2D mTexture;

        static SpriteSheet BasicEnemySpriteSheet, mBasicEnemyPainSpriteSheet;
        static SpriteAnimation BasicEnemyAnimation, mBasicEnemyPainAnimation, mBasicEnemyDeathAnimation;

        SpriteAnimationPlayer BasicEnemyPlayer;

        private double timeElapsed;
        //private double mTimeSinceCall;
        private bool justSpawned;
        private DIRECTION currentDirection;

        float snakeSpeed = .04f;

        Color tempColor = Color.White;

        static Random mSnakeRand = new Random();

        private static SoundEffect mBatSnd;

        //private static int mCallFrequency;

        private static float mVolume;
             

        public Snake(World world, Vector2 initPos, Player player)
            : base(world, initPos, player)
        {
            //mHealth = 300;
            this.setHealth(300);
            mFixture = FixtureFactory.AttachRectangle(40f * Camera.kPixelsToUnits, 50f * Camera.kPixelsToUnits, .03f, new Vector2(-20.5f, 30f) * Camera.kPixelsToUnits, new Body(mPhysicsWorld));
            mFixture.Body.BodyType = BodyType.Dynamic;
            mFixture.CollisionCategories = Category.Cat3;
            mFixture.CollidesWith = Category.All & ~Category.Cat4;
            mFixture.Body.OnCollision += monsterOnCollision;

            //Correct for meters vs pixels
            mFixture.Body.Position = new Vector2(mPosition.X, mPosition.Y);
            //mFixture.Body.UserData = "Monster";
            mFixture.Body.UserData = "Monster";
            mFixture.UserData = "Monster";
            //setRandDirection();

            //Init current direction
            getNextDirection(mPlayer);

           mPlayer = player;
           BasicEnemyPlayer = new SpriteAnimationPlayer();
           BasicEnemyPlayer.SetAnimationToPlay(BasicEnemyAnimation);

            justSpawned = true;
            //mTimeSinceCall = 10000; //so that they call upon spawning
            //mCallFrequency = 10000; //time between calls in ms
            mVolume = .1f; //call volume
        }

        ~Snake()
        {
        }

        public override void Draw(SpriteBatch spriteBatch) {
            //float proximity = Vector2.Distance(mBody.Position, mPlayer.GetPosition());
            Vector2 spriteOffset = -new Vector2(45, -60) * Camera.kPixelsToUnits;
            BasicEnemyPlayer.Draw(spriteBatch, new SpriteSheet.SpriteRenderingParameters(mFixture.Body.Position + spriteOffset, 0f, Color.White, 1 * new Vector2(Camera.kPixelsToUnits, -Camera.kPixelsToUnits)));
           
        }

        private void getNextDirection(Player mPlayer)
        {
            Vector2 playerPos = mPlayer.GetPosition();
            float xDifference = Math.Abs((playerPos.X - mFixture.Body.Position.X));
            float yDifference = Math.Abs((playerPos.Y - mFixture.Body.Position.Y));

            if (xDifference >= yDifference)
            {

                if (playerPos.X > mFixture.Body.Position.X)
                {
                    currentDirection = DIRECTION.RIGHT;
                }

                else
                {
                    currentDirection = DIRECTION.LEFT;
                }

            }

            if (yDifference > xDifference)
            {

                if (playerPos.Y > mFixture.Body.Position.Y)
                {
                    currentDirection = DIRECTION.UP;
                }

                else
                {
                    currentDirection = DIRECTION.DOWN;
                }

            }

        }

        public override void Update(GameTime gameTime)
        {
            HandleStun(gameTime, BasicEnemyPlayer, BasicEnemyAnimation, mBasicEnemyPainAnimation);

            if (mHealth <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Particle smoke = new Particle(mHackSmoke, mFixture.Body.Position, 1.0f);
                    smoke.mScale = new Vector2(1f);
                    float precision = 100f;
                    float maxSmokeSpeed = 1f;
                    float maxRotSpeed = 1.0f;
                    float maxScaleSpeed = 1.0f;
                    smoke.mVelocity = new Vector2(
                        Particle.Random(-maxSmokeSpeed / 2, +maxSmokeSpeed / 2),
                        Particle.Random(-maxSmokeSpeed / 2, +maxSmokeSpeed / 2));
                    smoke.mRotVel = Particle.Random(-maxRotSpeed / 2, +maxRotSpeed / 2);
                    smoke.mScaleVel = -new Vector2(
                        Particle.Random(-maxScaleSpeed / 2, +maxScaleSpeed / 2),
                        Particle.Random(-maxScaleSpeed / 2, +maxScaleSpeed / 2));
                    GetGameWorld().AddGameObject(smoke);
                }
            }
           // mFixture.Body.ResetDynamics();
            mFixture.Body.LinearDamping = 1f;
            mFixture.Body.Rotation = 0f;
            timeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeElapsed > 650)
            {
                timeElapsed = 0;
                getNextDirection(mPlayer);

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

            mFixture.Body.ApplyLinearImpulse(multiply * snakeSpeed * .03f);
            //mFixture.Body.LinearVelocity = (multiply * mMaxSpeed);

            /*if (mHealth <= 0)
            {
                GetGameWorld().RemoveGameObject(this);
            }*/

            if (!BasicEnemyPlayer.IsPlaying())
            {
                BasicEnemyPlayer.Play();
            }

            BasicEnemyPlayer.Update(gameTime);

            

            if (justSpawned)
            {
                justSpawned = false;
                mBatSnd.Play(mVolume, 0, 0);

            }

            /*Could be used for intermittent monster calls
             * mTimeSinceCall += gameTime.ElapsedGameTime.TotalMilliseconds;
             * if (mTimeSinceCall > mCallFrequency)
            {
                mTimeSinceCall = 0;

                if (mSnakeRand.Next() % 19 == 0)
                {
                    mBatSnd.Play(mVolume, 0, 0);
                }
            }*/

            // base.Update(gameTime);
            float speedParticle = 15;
            if (mFixture.Body.LinearVelocity.Length() > speedParticle)
            {
                mHealth -= 30;
                Particle smoke = new Particle(mHackSmoke, mFixture.Body.Position, 1.0f);
                smoke.mScale = new Vector2(0.5f);
                float precision = 100f;
                float maxSmokeSpeed = 2.0f;
                float maxRotSpeed = 1.0f;
                float maxScaleSpeed = 2.0f;
                smoke.mVelocity = new Vector2(
                    Particle.Random(-maxSmokeSpeed / 2, +maxSmokeSpeed / 2),
                    Particle.Random(-maxSmokeSpeed / 2, +maxSmokeSpeed / 2));
                smoke.mRotVel = Particle.Random(-maxRotSpeed / 2, +maxRotSpeed / 2);
                smoke.mScaleVel = -new Vector2(
                    Particle.Random(-maxScaleSpeed / 2, +maxScaleSpeed / 2),
                    Particle.Random(-maxScaleSpeed / 2, +maxScaleSpeed / 2));
                GetGameWorld().AddGameObject(smoke);
                mHealth -= 30;
            }

        }
        
        new public static void LoadData(Game myGame)
        {
            //mTexture = myGame.Content.Load<Texture2D>("enemySprites/Sentinel");
            mHackSmoke = myGame.Content.Load<Texture2D>("Particles/SmokeParticleEffectSprite");
            BasicEnemySpriteSheet = new SpriteSheet();
            BasicEnemySpriteSheet.SetTexture(myGame.Content.Load<Texture2D>("Enemies/BasicEnemy"));
            BasicEnemyAnimation = new SpriteAnimation();
            //
            mBasicEnemyPainSpriteSheet = new SpriteSheet();
            mBasicEnemyPainSpriteSheet.SetTexture(myGame.Content.Load<Texture2D>("enemySprites/basicEnemyHit"));
            mBasicEnemyPainAnimation = new SpriteAnimation();
            mBasicEnemyDeathAnimation = new SpriteAnimation();

            for (int i = 0; i < 2; i++)
            {
                BasicEnemySpriteSheet.AddSprite(new Vector2(i * 45, 0), new Vector2(45, 60));
                mBasicEnemyPainSpriteSheet.AddSprite(new Vector2(i * 45, 0), new Vector2(45, 60));
                BasicEnemyAnimation.AddFrame(BasicEnemySpriteSheet, i, .1f);
            }
            mBasicEnemyPainAnimation.AddFrame(mBasicEnemyPainSpriteSheet, 0, 2f);
            mBasicEnemyDeathAnimation.AddFrame(mBasicEnemyPainSpriteSheet, 1, 2f);

            mBatSnd = myGame.Content.Load<SoundEffect>("Audio/bat");
        }
    }
        
}
