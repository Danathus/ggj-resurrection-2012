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

using FarseerPhysics.DebugViews;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;

namespace ggj_resurrection
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Camera mCamera;

        Player mPlayer;

        GraphicsDeviceManager mGraphics;
        SpriteBatch mSpriteBatch;
        //
        LifeWorld mLifeWorld;
        DeathWorld mDeathWorld;
        GameWorld mCurrentWorld; // this is to point to whichever one we're in

        BasicEffect mRenderingEffect;

        public Game1()
        {
            mGraphics = new GraphicsDeviceManager(this);
            mGraphics.PreferredBackBufferWidth  = 800;
            mGraphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
            
            mLifeWorld    = new LifeWorld("Content/lifeworld.txt", this);
            mDeathWorld   = new DeathWorld();

            mCurrentWorld = mLifeWorld;

            mCamera = new Camera(
                null, new Vector2(0, 0),
                new Vector2(mGraphics.PreferredBackBufferWidth, mGraphics.PreferredBackBufferHeight));

            //FarseerPhysics.Settings.ContinuousPhysics = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);

            mLifeWorld.LoadContent(mGraphics.GraphicsDevice, Content);
            mDeathWorld.LoadContent(mGraphics.GraphicsDevice, Content);

            Player.LoadData(this);
            MonsterSpawner.LoadData(this);
            Monster.LoadData(this);
            Snake.LoadData(this);
            EvilCow.LoadData(this);
            SwordSlash.LoadData(this);

            mRenderingEffect = new BasicEffect(GraphicsDevice);

            mLifeWorld.loadTiles(this);

            mPlayer = new Player(mLifeWorld.mPhysicsWorld, new Vector2(0, 0));
            mLifeWorld.AddGameObject(mPlayer);
            mLifeWorld.AddGameObject(new MonsterSpawner(mLifeWorld.mPhysicsWorld, new Vector2(0, 0), mPlayer));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            mLifeWorld.Update(gameTime);
            mDeathWorld.Update(gameTime);
            mCamera.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
            {
                mCamera.mTargetRot = new Vector3(90f-15f, 0f, 0f);
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
            {
                mCamera.mTargetRot = new Vector3(0f, 0f, 0f);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // apply the camera view and projection matrices by passing a BasicEffect to the SpriteBatch
            mRenderingEffect.World      = Matrix.Identity;
            mRenderingEffect.View       = mCamera.mViewMatrix;
            mRenderingEffect.Projection = mCamera.mProjectionMatrix;
            //
            mRenderingEffect.TextureEnabled     = true;
            mRenderingEffect.VertexColorEnabled = true;

            // custom drawing code here
            mSpriteBatch.Begin(
                SpriteSortMode.Immediate,   // sprite sort mode (which is better, immediate or deffered?)
                BlendState.AlphaBlend,      // blend state
                SamplerState.LinearClamp,   // sampler state
                DepthStencilState.None,     // depth stencil state
                RasterizerState.CullNone,   // rasterizer state
                mRenderingEffect,           // effect (formerly null)
                Matrix.Identity);           // transform matrix

            mLifeWorld.Draw(mSpriteBatch);
            mDeathWorld.Draw(mSpriteBatch);

            mSpriteBatch.End();

           // mDebugView.RenderDebugData(ref mCamera.mProjectionMatrix, ref mCamera.mViewMatrix);

            base.Draw(gameTime);
        }
    }
}
