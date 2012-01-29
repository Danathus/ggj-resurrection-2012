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
        private double timeElapsed;
        private static int mWidth;
        private static int mHeight;
        static Random mRand = new Random();

        private static Player mPlayer;

        public MonsterSpawner(World world, Vector2 initPos, Player player)
            : base(world, initPos)
        {
            mPhysicsWorld = world;
            mMonsters = new List<Monster>();

            mPlayer = player;
        }

        ~MonsterSpawner()
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
                Spawn();
            }
        }

        public static void LoadData(Game myGame)
        {
            mHeight = 10;// myGame.Window.ClientBounds.Height;
            mWidth  = 10;// myGame.Window.ClientBounds.Width;
        }


        private void Spawn()
        {
            //Spawn monster in a random location -- hopefully in bounds
            Monster newMonster = new Snake(mPhysicsWorld, new Vector2(mRand.Next(0, mWidth), mRand.Next(0, mHeight)), mPlayer);
            mMonsters.Add(newMonster);
            mGameWorld.AddGameObject(newMonster);

            Monster newMonsterCow = new EvilCow(mPhysicsWorld, new Vector2(mRand.Next(0, mWidth), mRand.Next(0, mHeight)), mPlayer);
            mMonsters.Add(newMonsterCow);
            mGameWorld.AddGameObject(newMonsterCow);
        }
    }
}
