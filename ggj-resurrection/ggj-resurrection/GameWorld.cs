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

using FarseerPhysics.DebugViews;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;

namespace ggj_resurrection
{
    public abstract class GameWorld
    {
        public Camera    mCamera;
        protected List<GameObject> mGameObjects;
        List<GameObject> mAddList, mRemoveList;
        public World     mPhysicsWorld;
        public DebugViewXNA     mDebugView;
        protected BasicEffect mRenderingEffect;

        bool mAwake;

        Texture2D mHackSmoke;

        public GameWorld(Camera camera)
        {
            mCamera      = camera;

            mGameObjects = new List<GameObject>();
            mAddList     = new List<GameObject>();
            mRemoveList  = new List<GameObject>();

            mPhysicsWorld = new World(new Vector2(0, 0));
            mDebugView    = new DebugViewXNA(mPhysicsWorld);

            mAwake = false;
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
            mGameObjects.Sort();
            if (mAwake)
            {
                foreach (GameObject go in mGameObjects)
                {
                    if (go.IsEnabled())
                    {
                        go.Update(gameTime);
                    }

                    if (go.mHealth < 0)
                    {
                        this.RemoveGameObject(go);
                    }
                }

                mPhysicsWorld.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

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
                    go.fixtureDestory();
                    mGameObjects.Remove(go);
                }
                mRemoveList.Clear();
            }
        }

        public abstract void DrawCustomWorldDetails(SpriteBatch spriteBatch);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // apply the camera view and projection matrices by passing a BasicEffect to the SpriteBatch
            mRenderingEffect.World      = Matrix.Identity;
            mRenderingEffect.Projection = mCamera.mProjectionMatrix;
            //
            mRenderingEffect.TextureEnabled     = true;
            mRenderingEffect.VertexColorEnabled = true;

            mRenderingEffect.DiffuseColor = mAwake
                ? new Vector3(1f, 1f, 1f)
                : new Vector3(0.5f, 0.5f, 0.5f);

            // custom drawing code here
            spriteBatch.Begin(
                SpriteSortMode.Immediate,   // sprite sort mode (which is better, immediate or deffered?)
                BlendState.AlphaBlend,      // blend state
                SamplerState.LinearClamp,   // sampler state
                DepthStencilState.None,     // depth stencil state
                RasterizerState.CullNone,   // rasterizer state
                mRenderingEffect,           // effect (formerly null)
                Matrix.Identity);           // transform matrix

            DrawCustomWorldDetails(spriteBatch);

            foreach (GameObject go in mGameObjects)
            {
                if (go.IsEnabled())
                {
                    go.Draw(spriteBatch);
                }
            }

            spriteBatch.End();
        }

        public void LoadContent(GraphicsDevice device, ContentManager content)
        {

            mDebugView.LoadContent(device, content);
            mRenderingEffect = new BasicEffect(device);
             mHackSmoke = content.Load<Texture2D>("Particles/SmokeParticleEffectSprite");
        }

        public virtual void GoToSleep()
        {
            mAwake = false;
            // todo: on-sleep processing
        }

        public virtual void WakeUp()
        {
            mAwake = true;
            // todo: on-wake processing
        }
    }
}
