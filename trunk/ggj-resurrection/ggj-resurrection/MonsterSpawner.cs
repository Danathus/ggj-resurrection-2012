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

        public MonsterSpawner(World world)
            : base(world)
        {
            mPhysicsWorld = world;
            mMonsters = new List<Monster>();
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

            if (timeElapsed > 5000)
            {
                timeElapsed = 0;
                Spawn();
            }
        }

        private void Spawn()
        {
            Monster newMonster = new Monster(mPhysicsWorld);
            mMonsters.Add(newMonster);
            mGameWorld.AddGameObject(new Monster(mPhysicsWorld));
        }
    }
}
