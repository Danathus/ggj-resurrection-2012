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
        Camera     mCamera;
        Player     mAlivePlayer;
        DeadPlayer mDeadPlayer;

        GraphicsDeviceManager mGraphics;
        SpriteBatch mSpriteBatch;
        SoundEffect mLifeTheme;
        SoundEffectInstance mLifeThemeSEI;
        
        //
        LifeWorld  mLifeWorld;
        DeathWorld mDeathWorld;
        GameWorld  mCurrentWorld; // this is to point to whichever one we're in

        public Game1()
        {
            mGraphics = new GraphicsDeviceManager(this);
            mGraphics.PreferredBackBufferWidth  = 800;
            mGraphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";

            mCamera = new Camera(
                null, new Vector2(0, 0),
                new Vector2(mGraphics.PreferredBackBufferWidth, mGraphics.PreferredBackBufferHeight));

            mLifeWorld  = new LifeWorld(mCamera, "Content/lifeworld.txt", this);
            mDeathWorld = new DeathWorld(mCamera);
            //
            mCurrentWorld = mLifeWorld;

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
            DeadPlayer.LoadData(this);
            MonsterSpawner.LoadData(this);
            Monster.LoadData(this);
            Snake.LoadData(this);
            EvilCow.LoadData(this);
            SwordSlash.LoadData(this);

            mLifeWorld.loadTiles(this);

            mLifeWorld.AddGameObject(mAlivePlayer = new Player(mLifeWorld.mPhysicsWorld, new Vector2(0, 0)));
            mDeathWorld.AddGameObject(mDeadPlayer = new DeadPlayer(mDeathWorld.mPhysicsWorld, new Vector2(0, 0)));
            mLifeWorld.AddGameObject(new MonsterSpawner(mLifeWorld.mPhysicsWorld, new Vector2(0, 0), mAlivePlayer));

            mLifeTheme = Content.Load<SoundEffect>("Audio/LifeTheme");
            //We'll need to load the Death song here!
            mLifeThemeSEI = mLifeTheme.CreateInstance();
            mLifeThemeSEI.IsLooped = true;
            mLifeThemeSEI.Volume = 1;
            mLifeThemeSEI.Play();

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

            KeyboardState keyState = Keyboard.GetState();
            //
            //mCamera.mSideViewOffset.X += ((keyState.IsKeyDown(Keys.U)?1:0) - (keyState.IsKeyDown(Keys.J)?1:0)) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //mCamera.mSideViewOffset.Y += ((keyState.IsKeyDown(Keys.I)?1:0) - (keyState.IsKeyDown(Keys.K)?1:0)) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //mCamera.mSideViewOffset.Z += ((keyState.IsKeyDown(Keys.O)?1:0) - (keyState.IsKeyDown(Keys.L)?1:0)) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed ||
                keyState.IsKeyDown(Keys.Q))
            {
                if (mCurrentWorld != mDeathWorld)
                {
                    // start transition to death world
                    mCamera.mTargetRot = new Vector3(90f - 15f, 0f, 0f);
                    mCurrentWorld = mDeathWorld;

                    mDeadPlayer.SetPosition(new Vector2(mAlivePlayer.GetPosition().X, 0));
                    mCamera.mSideViewOffset = new Vector3(0, 0,
                        -mAlivePlayer.GetPosition().Y +
                        //+50 * Camera.kPixelsToUnits // this is the "height" (incidently half of it, doubled) of the player sprite
                        +Player.kFrameSizeInPixels.Y/2 * Camera.kPixelsToUnits
                        );
                    mLifeThemeSEI.Stop();
                }
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed ||
                keyState.IsKeyDown(Keys.W))
            {
                if (mCurrentWorld != mLifeWorld)
                {
                    mCamera.mTargetRot = new Vector3(0f, 0f, 0f);
                    mCurrentWorld = mLifeWorld;
                    mLifeThemeSEI.Play();
                }
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

            // draw life world
            {
            }

            // draw death world
            {
            }

            /*
            // apply the camera view and projection matrices by passing a BasicEffect to the SpriteBatch
            mRenderingEffect.World      = Matrix.Identity;
            mRenderingEffect.View       = mCamera.mViewMatrix;
            mRenderingEffect.Projection = mCamera.mProjectionMatrix;
            //
            mRenderingEffect.TextureEnabled = true;
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
             * /*/

            mLifeWorld.Draw(mSpriteBatch);
            mDeathWorld.Draw(mSpriteBatch);

            /*
            mSpriteBatch.End();
             * //*/

            //mDebugView.RenderDebugData(ref mCamera.mProjectionMatrix, ref mCamera.mViewMatrix);

            base.Draw(gameTime);
        }

        private void DrawGameWorld()
        {
        }
    }
}
