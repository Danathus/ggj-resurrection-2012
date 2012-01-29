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
        public Player    mPlayer;
        public Camera    mCamera;
        protected List<GameObject> mGameObjects;
        List<GameObject> mAddList, mRemoveList;
        public World     mPhysicsWorld;
        public DebugViewXNA     mDebugView;
        protected BasicEffect mRenderingEffect;
        

        protected bool mAwake;
        float mFadeCountdown, mMaxFadeCountdown;

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
            mMaxFadeCountdown = 3f;
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
            }
            else // if (mAwake)
            {
                mFadeCountdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (mFadeCountdown <= 0)
                {
                    // clear all objects (except the player!)
                    foreach (GameObject go in mGameObjects)
                    {
                        if (!(go is Player))
                        {
                            Console.WriteLine("killing object...");
                            RemoveGameObject(go);
                        }
                    }
                }
            } // if (mAwake) else

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
        } // Update()

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
            mRenderingEffect.Alpha = 1f;
            mRenderingEffect.World = Matrix.Identity;

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

            spriteBatch.End();

            mRenderingEffect.DiffuseColor = mAwake
                ? new Vector3(1f, 1f, 1f)
                : new Vector3(mFadeCountdown / mMaxFadeCountdown, mFadeCountdown / mMaxFadeCountdown, mFadeCountdown / mMaxFadeCountdown);
            mRenderingEffect.Alpha = mAwake
                ? 1f
                : mFadeCountdown / mMaxFadeCountdown;

            if (!mAwake)
            {
                mRenderingEffect.World = Matrix.CreateScale(
                    2f - mFadeCountdown / mMaxFadeCountdown,
                    2f - mFadeCountdown / mMaxFadeCountdown,
                    1f);
            }

            spriteBatch.Begin(
                SpriteSortMode.Immediate,   // sprite sort mode (which is better, immediate or deffered?)
                BlendState.AlphaBlend,      // blend state
                SamplerState.LinearClamp,   // sampler state
                DepthStencilState.None,     // depth stencil state
                RasterizerState.CullNone,   // rasterizer state
                mRenderingEffect,           // effect (formerly null)
                Matrix.Identity);           // transform matrix

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

        public virtual bool ReadyToTransition()
        {
            return mAwake || mFadeCountdown <= 0;
        }

        public virtual void GoToSleep()
        {
            mAwake = false;

            // on-sleep processing
            mPlayer.GoToSleep();
            mPlayer.Disable();
            mFadeCountdown = mMaxFadeCountdown;
        }

        public virtual void WakeUp()
        {
            mAwake = true;
            // on-wake processing
            mPlayer.WakeUp();
            mPlayer.Enable();
        }
    }
}
