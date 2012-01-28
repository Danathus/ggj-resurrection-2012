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
        GraphicsDeviceManager mGraphics;
        SpriteBatch           mSpriteBatch;
        World                 mPhysicsWorld;
        //
        LifeWorld  mLifeWorld;
        DeathWorld mDeathWorld;
        GameWorld  mCurrentWorld; // this is to point to whichever one we're in

        DebugViewXNA mDebugView;
        private Matrix mProjection;
        private Matrix mDebugCameraMatrix;

        Player mPlayer;

        Vector2 mScreenCenter;
        
        public Game1()
        {
            mGraphics = new GraphicsDeviceManager(this);
            mGraphics.PreferredBackBufferWidth = 800;
            mGraphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
            mPhysicsWorld = new World(new Vector2(0, 0));

            mDebugView = new DebugViewXNA(mPhysicsWorld);
            mPlayer = new Player(mPhysicsWorld);


            mLifeWorld    = new LifeWorld();
            mDeathWorld   = new DeathWorld();

            mCurrentWorld = mLifeWorld;
          

            
            mLifeWorld.AddGameObject(mPlayer);
          //  mLifeWorld.AddGameObject(new MonsterSpawner(mGraphics, mPhysicsWorld));

            
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

            mDebugView.LoadContent(mGraphics.GraphicsDevice, Content);

            mPlayer.LoadData(this);
            Monster.LoadData(this);
            SwordSlash.LoadData(this);

            mScreenCenter = new Vector2(Window.ClientBounds.Width / 2f, Window.ClientBounds.Height / 2f);
            mProjection = Matrix.CreateOrthographicOffCenter(0f, mScreenCenter.X / 32f, mScreenCenter.Y / 32f, 0f, 0f, 1f);
            mDebugCameraMatrix = Matrix.Identity;

            mScreenCenter = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
            mProjection = Matrix.CreateOrthographicOffCenter(0f, mScreenCenter.X / 32f,
                                                             mScreenCenter.Y / 32f, 0f, 0f, 1f);
            mDebugCameraMatrix = Matrix.Identity;

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
            mPhysicsWorld.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // custom drawing code here
            mSpriteBatch.Begin();
            mLifeWorld.Draw(mSpriteBatch);
            mDeathWorld.Draw(mSpriteBatch);

            mSpriteBatch.End();

            mDebugView.RenderDebugData(ref mProjection, ref mDebugCameraMatrix);


            base.Draw(gameTime);
        }
    }
}
