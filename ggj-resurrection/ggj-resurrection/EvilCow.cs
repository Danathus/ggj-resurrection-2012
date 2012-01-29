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
    class EvilCow : Monster
    {
        //static private Texture2D mTexture;

        

        static SpriteSheet lightningCowSpriteSheet;
        static SpriteAnimation lightningCowAnimation;

        SpriteAnimationPlayer lightningPlayer;

        Color tempColor = Color.White;

        private DIRECTION cowDirection;
        private float cowMaxSpeed = 3f;
        private double timeElapsed;
        //private double mTimeSinceCall = 0;
        static Random cowRand = new Random();

        private static SoundEffect mMooSnd;
        private static SoundEffect mThunderSnd;
        private bool justSpawned;
        private static float mMooVolume;
        private static float mThunderVolume;
        //private static int mCallFrequency;
        static Texture2D mHackSmoke;

        public EvilCow(World world, Vector2 initPos, Player player)
            : base(world, initPos, player)
        {
            mHealth = 200;
            mPlayer = player;
            lightningPlayer = new SpriteAnimationPlayer();
            lightningPlayer.SetAnimationToPlay(lightningCowAnimation);

            mFixture = FixtureFactory.AttachRectangle(90f * Camera.kPixelsToUnits, 90f * Camera.kPixelsToUnits, 1f, new Vector2(0, 0), new Body(mPhysicsWorld));
            mFixture.Body.BodyType = BodyType.Dynamic;
            mFixture.CollisionCategories = Category.Cat4;
            mFixture.CollidesWith = Category.All & ~Category.Cat3;
            mFixture.Body.OnCollision += monsterOnCollision;

            //Correct for meters vs pixels
            mFixture.Body.Position = new Vector2(mPosition.X, mPosition.Y);
            //mFixture.Body.UserData = "Monster";
            mFixture.Body.UserData = "Monster";
            mFixture.UserData = "Monster";

            //mTimeSinceCall = 10000; //so that they call upon spawning
            //mCallFrequency = 10000; //how often cow moos, in ms
            justSpawned = true;
            mMooVolume = .2f;
            mThunderVolume = .1f;

            //Init direction
            getNextDirection(mPlayer);

            /*
            //mBody = BodyFactory.CreateRectangle(mPhysicsWorld, 3f, 3f, .0125f);
            
            mFixture = FixtureFactory.AttachRectangle(3f, 3f, .0125f, new Vector2(0,0), new Body(mPhysicsWorld));
            mFixture.Body.BodyType = BodyType.Dynamic;
            mFixture.UserData = "EvilCow";
            mFixture.Body.UserData = "EvilCow";
            mFixture.CollisionCategories = Category.Cat3;
            mFixture.Body.OnCollision += monsterOnCollision;*/

        }

        ~EvilCow()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 spriteOffset = -new Vector2(50, -50) * Camera.kPixelsToUnits;
            lightningPlayer.Draw(spriteBatch, new SpriteSheet.SpriteRenderingParameters(mFixture.Body.Position + spriteOffset, 0f, Color.White, 1* new Vector2(Camera.kPixelsToUnits, -Camera.kPixelsToUnits)));
            //float proximity = Vector2.Distance(mBody.Position, mPlayer.GetPosition());
           // spriteBatch.Draw(mTexture, mFixture.Body.Position, null, tempColor, 0f, new Vector2(mTexture.Width / 2, mTexture.Height / 2), Camera.kPixelsToUnits, SpriteEffects.None, 0f);
        }


        private void getNextDirection(Player mPlayer)
        {
            int randomnum = cowRand.Next(1, 100);

            if (randomnum <= 20)
            {
                cowDirection = DIRECTION.NONE;
            }

            else
            {
                Vector2 playerPos = mPlayer.GetPosition();
                float xDifference = Math.Abs((playerPos.X - mFixture.Body.Position.X));
                float yDifference = Math.Abs((playerPos.Y - mFixture.Body.Position.Y));

                if (randomnum > 20 && randomnum <= 90)
                {
                    if (xDifference >= yDifference)
                    {
                        if (playerPos.X < mFixture.Body.Position.X)
                        {
                            cowDirection = DIRECTION.LEFT;
                        }

                        else
                        {
                            cowDirection = DIRECTION.RIGHT;
                        }

                    }

                    else
                    {

                        if (playerPos.Y < mFixture.Body.Position.Y)
                        {
                            cowDirection = DIRECTION.DOWN;
                        }

                        else
                        {
                            cowDirection = DIRECTION.UP;
                        }

                    }

                }

                else
                {

                    if (xDifference >= yDifference)
                    {

                        if (playerPos.X > mFixture.Body.Position.X)
                        {
                            cowDirection = DIRECTION.RIGHT;
                        }

                        else
                        {
                            cowDirection = DIRECTION.LEFT;
                        }

                    }

                    if (yDifference > xDifference)
                    {

                        if (playerPos.Y > mFixture.Body.Position.Y)
                        {
                            cowDirection = DIRECTION.UP;
                        }

                        else
                        {
                            cowDirection = DIRECTION.DOWN;
                        }

                    }

                }

            }


        }

        public override void Update(GameTime gameTime)
        {
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

           // mFixture.Body.LinearDamping = .01f;
            mFixture.Body.Rotation = 0f;
            timeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeElapsed > 1000)
            {
                timeElapsed = 0;
                getNextDirection(mPlayer);

            }

            Vector2 multiply = new Vector2(0, 0);

            switch (cowDirection)
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


                default:
                    break;
            }

            mFixture.Body.ApplyLinearImpulse(multiply * cowMaxSpeed * .03f);

            if (!lightningPlayer.IsPlaying())
            {
                lightningPlayer.Play();
            }

            lightningPlayer.Update(gameTime);

           base.Update(gameTime);

            if (justSpawned)
                {
                    if (cowRand.Next() % 2 == 0) mThunderSnd.Play(mThunderVolume, 0f, 0f);
                    else mMooSnd.Play(mMooVolume, 0, 0);
                    justSpawned = false;
                }

            /*Could be used for intermittent monster calls
             * mTimeSinceCall += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (mTimeSinceCall > mCallFrequency)
            {
                mTimeSinceCall = 0;

                
                else if (cowRand.Next() % 19 == 0)
                {
                    mMooSnd.Play(mMooVolume, 0f, 0f);
                }

            }*/

            float speedParticle = 10f;
            if (mFixture.Body.LinearVelocity.Length() > speedParticle)
            {
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

            //base.Update(gameTime);

        }

        new public static void LoadData(Game myGame)
        {
            lightningCowSpriteSheet = new SpriteSheet();
            lightningCowSpriteSheet.SetTexture(myGame.Content.Load<Texture2D>("Enemies/CowRun"));//("Enemies/LightningCow5fps"));

            lightningCowAnimation = new SpriteAnimation();

            for (int i = 0; i < 4; i++)
            {
                lightningCowSpriteSheet.AddSprite(new Vector2(i * 100, 0), new Vector2(100, 100));
                lightningCowAnimation.AddFrame(lightningCowSpriteSheet, i, .1f);
            }

            mHackSmoke = myGame.Content.Load<Texture2D>("Particles/SmokeParticleEffectSprite");
            mMooSnd = myGame.Content.Load<SoundEffect>("Audio/moo");
            mThunderSnd = myGame.Content.Load<SoundEffect>("Audio/thunder");
            
            //mTexture = myGame.Content.Load<Texture2D>("enemySprites/evilCow");
        }
    }

}
