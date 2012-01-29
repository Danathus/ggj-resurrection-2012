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

        static SpriteSheet BasicEnemySpriteSheet;
        static SpriteAnimation BasicEnemyAnimation;

        SpriteAnimationPlayer BasicEnemyPlayer;

        private double timeElapsed;
        private double mTimeSinceCall;
        private DIRECTION currentDirection;

        float snakeSpeed = .1f;

        Color tempColor = Color.White;

        private static SoundEffect mBatSnd;

        private static float mVolume;
             

        public Snake(World world, Vector2 initPos, Player player)
            : base(world, initPos, player)
        {
            
            mFixture = FixtureFactory.AttachRectangle(40f * Camera.kPixelsToUnits, 50f * Camera.kPixelsToUnits, .015f, new Vector2(-20.5f, 30f) * Camera.kPixelsToUnits, new Body(mPhysicsWorld));
            mFixture.Body.BodyType = BodyType.Dynamic;
            mFixture.CollisionCategories = Category.Cat3;
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
           mTimeSinceCall = 7500; //so that they call upon spawning

           mVolume = .3f;
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

            mTimeSinceCall += gameTime.ElapsedGameTime.TotalMilliseconds;

            //Make a call every 5 seconds
            if (mTimeSinceCall > 5000)
            {
                mTimeSinceCall = 0;
                mBatSnd.Play(mVolume, 0, 0);

            }

           // base.Update(gameTime);
        }
        
        new public static void LoadData(Game myGame)
        {
            //mTexture = myGame.Content.Load<Texture2D>("enemySprites/Sentinel");

            BasicEnemySpriteSheet = new SpriteSheet();
            BasicEnemySpriteSheet.SetTexture(myGame.Content.Load<Texture2D>("Enemies/BasicEnemy"));
            BasicEnemyAnimation = new SpriteAnimation();

            for (int i = 0; i < 2; i++)
            {
                BasicEnemySpriteSheet.AddSprite(new Vector2(i * 45, 0), new Vector2(45, 60));
                BasicEnemyAnimation.AddFrame(BasicEnemySpriteSheet, i, .1f);
            }

            mBatSnd = myGame.Content.Load<SoundEffect>("Audio/bat");

        }
    }
        
}
