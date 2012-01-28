using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ggj_resurrection
{
    public abstract class GameWorld
    {
        List<GameObject> mGameObjects;

        public GameWorld()
        {
            mGameObjects = new List<GameObject>();
        }

        public void AddGameObject(GameObject go)
        {
            mGameObjects.Add(go);
        }

        public void RemoveGameObject(GameObject go)
        {
            mGameObjects.Remove(go);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (GameObject go in mGameObjects)
            {
                go.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject go in mGameObjects)
            {
                go.Draw(spriteBatch);
            }
        }
    }
}
