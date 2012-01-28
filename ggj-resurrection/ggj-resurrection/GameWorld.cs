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
        List<GameObject> mAddList, mRemoveList;

        public GameWorld()
        {
            mGameObjects = new List<GameObject>();
            mAddList     = new List<GameObject>();
            mRemoveList  = new List<GameObject>();
        }

        public void AddGameObject(GameObject go)
        {
            mAddList.Add(go);
            go.SetGameWorld(this);
        }

        public void RemoveGameObject(GameObject go)
        {
            mRemoveList.Add(go);
            go.SetGameWorld(null);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (GameObject go in mGameObjects)
            {
                go.Update(gameTime);
            }

            // handle all add requests
            foreach (GameObject go in mAddList)
            {
                mGameObjects.Add(go);
            }
            mAddList.Clear();

            // handle all remove requests
            foreach (GameObject go in mRemoveList)
            {
                //remove in farseer?
                mGameObjects.Remove(go);
            }
            mRemoveList.Clear();
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
