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
    class MonsterSpawner : GameObject
    {
        List<Monster> mMonsters;
        private double timeBasicElapsed;
        private double timeCowElapsed;
        private static int mWidth;
        private static int mHeight;
        private int nextBasic;
        private int nextCow;
        static Random mRand = new Random();

        private static Player mPlayer;

        public MonsterSpawner(World world, Vector2 initPos, Player player)
            : base(world, initPos)
        {
            mPhysicsWorld = world;
            mMonsters = new List<Monster>();

            mPlayer = player;
            nextBasic = mRand.Next(1, 3);
            nextCow = mRand.Next(3, 10);
        }

        ~MonsterSpawner()
        {
        }

               
        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }
               
        public override void Update(GameTime gameTime)
        {
            timeBasicElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;
            timeCowElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

           if (timeBasicElapsed >= (nextBasic * 1000))
           {
               timeBasicElapsed = 0;
               nextBasic = mRand.Next(1, 3);
               SpawnBasic();
           }

           if (timeCowElapsed >= (nextCow * 1000))
           {
               timeCowElapsed = 0;
               nextCow = mRand.Next(3, 7);
               SpawnCow();
           }
        }

        public static void LoadData(Game myGame)
        {
            mHeight = 10;// myGame.Window.ClientBounds.Height;
            mWidth  = 10;// myGame.Window.ClientBounds.Width;
        }


        private void SpawnCow()
        {

            while (true)
            {
                Vector2 playerPos = mPlayer.GetPosition();
                Vector2 randomPos = new Vector2(mRand.Next(0, mWidth), mRand.Next(0, mHeight));

                Vector2 difference = playerPos - randomPos;

                if (difference.Length() > (50 * Camera.kPixelsToUnits))
                {

                    Monster newMonsterCow = new EvilCow(mPhysicsWorld, new Vector2(mRand.Next(-mHeight / 2, mHeight / 2), mRand.Next(-mHeight / 2, mHeight / 2)), mPlayer);
                    mMonsters.Add(newMonsterCow);
                    mGameWorld.AddGameObject(newMonsterCow);

                    break;
                }

            }

        }

        private void SpawnBasic()
        {
            //Spawn monster in a random location -- hopefully in bounds
            while (true)
            {
                Vector2 playerPos = mPlayer.GetPosition();
                Vector2 randomPos = new Vector2(mRand.Next(0, mWidth), mRand.Next(0, mHeight));

                Vector2 difference = playerPos - randomPos;

                if (difference.Length() > (20 * Camera.kPixelsToUnits))
                {

                    Monster newMonster = new Snake(mPhysicsWorld, new Vector2(mRand.Next(0, mWidth), mRand.Next(0, mHeight)), mPlayer);
                    mMonsters.Add(newMonster);
                    mGameWorld.AddGameObject(newMonster);

                    break;
                }

            }

            //mGameWorld.AddGameObject(new MonsterCow);

        }
    }
}
