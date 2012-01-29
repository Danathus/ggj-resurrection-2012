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
        
        SoundEffect mDeathTheme;
        SoundEffectInstance mDeathThemeSEI;

        SoundEffect mPlayerFallSnd;
        SoundEffect mGongSnd;
        private static float mGongVolume = .6f;

        SpriteFont drawFont;
        
        //
        public static LifeWorld  mLifeWorld;
        DeathWorld mDeathWorld;
        GameWorld  mCurrentWorld; // this is to point to whichever one we're in
        public static GameWorld mDesiredWorld;

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
            //puzzleChunks = graveyardMaker.generatePuzzleSections(8);
            mDeathWorld = new DeathWorld(mCamera);
            
            //
            mCurrentWorld = mLifeWorld;
            mDesiredWorld = mCurrentWorld;

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
            drawFont = Content.Load<SpriteFont>("drawFont");

            mLifeWorld.LoadContent(mGraphics.GraphicsDevice, Content);
            mDeathWorld.LoadContent(mGraphics.GraphicsDevice, Content);

            Player.LoadData(this);
            DeadPlayer.LoadData(this);
            MonsterSpawner.LoadData(this);
            Monster.LoadData(this);
            Snake.LoadData(this);
            EvilCow.LoadData(this);
            SwordSlash.LoadData(this);
            Grave.LoadData(this);
            Soul.LoadData(this);

            mLifeWorld.loadTiles(this);

            mLifeWorld.AddGameObject(mAlivePlayer = new AlivePlayer(mLifeWorld.mPhysicsWorld, new Vector2(0, 0)));
            mLifeWorld.mPlayer = mAlivePlayer;
            mDeathWorld.AddGameObject(mDeadPlayer = new DeadPlayer(mDeathWorld.mPhysicsWorld, new Vector2(0, 0)));
            mDeathWorld.mPlayer = mDeadPlayer;

            mDeathTheme = Content.Load<SoundEffect>("Audio/DeathTheme");
            mDeathThemeSEI = mDeathTheme.CreateInstance();
            mDeathThemeSEI.IsLooped = true;
            mDeathThemeSEI.Volume = 1;

            mPlayerFallSnd = Content.Load<SoundEffect>("Audio/playerFall");
            mGongSnd = Content.Load<SoundEffect>("Audio/gong");

            mLifeTheme = Content.Load<SoundEffect>("Audio/LifeTheme");
            mLifeThemeSEI = mLifeTheme.CreateInstance();
            mLifeThemeSEI.IsLooped = true;
            mLifeThemeSEI.Volume = 1;
            mLifeThemeSEI.Play();

            mLifeWorld.WakeUp();
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

            //mDesiredWorld

            // debug testing world switch
            /*
            KeyboardState keyState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed ||
                keyState.IsKeyDown(Keys.Q))
            {
                mDesiredWorld = mDeathWorld;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed ||
                keyState.IsKeyDown(Keys.W))
            {
                mDesiredWorld = mLifeWorld;
            }
            //*/

            // should we die?
            if (mCurrentWorld == mLifeWorld && mAlivePlayer.mHealth <= 0)
            {
                // player should die...
                mDesiredWorld = mDeathWorld;
            }

            if (mCurrentWorld != mDesiredWorld )//&& mCurrentWorld.ReadyToTransition() && mDesiredWorld.ReadyToTransition())
            {
                if (mDesiredWorld == mDeathWorld)
                {
                    // start transition to death world
                    mCamera.mTargetRot = new Vector3(90f - 15f, 0f, 0f);
                    mCurrentWorld = mDeathWorld;

                    mLifeThemeSEI.Stop();
                    mPlayerFallSnd.Play(.4f, 0f, 0f);
                    mDeathThemeSEI.Play();
                    

                    // turn off/on players as appropriate
                    mDeadPlayer.SetPosition(mAlivePlayer.GetPosition());

                    // complete transition
                    mLifeWorld.GoToSleep();
                    mDeathWorld.WakeUp();
                }
                if (mDesiredWorld == mLifeWorld)
                {
                    mCamera.mTargetRot = new Vector3(0f, 0f, 0f);
                    mCurrentWorld = mLifeWorld;
                    mDeathThemeSEI.Stop();
                    mGongSnd.Play(mGongVolume, 0, 0);
                    mLifeThemeSEI.Play();

                    // turn off/on players as appropriate
                    mAlivePlayer.SetPosition(mDeadPlayer.GetPosition());

                    // complete transition
                    mDeathWorld.GoToSleep();
                    mLifeWorld.WakeUp();
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

            mLifeWorld.Draw(mSpriteBatch);
            mLifeWorld.drawHealth(mSpriteBatch, mGraphics);
            mDeathWorld.Draw(mSpriteBatch);

            mSpriteBatch.Begin();
            int score = mCurrentWorld.getScore();
            mSpriteBatch.DrawString(drawFont, "SCORE: " + score , new Vector2(mGraphics.GraphicsDevice.Viewport.Width, mGraphics.GraphicsDevice.Viewport.Height) * .01f, Color.DarkGreen);
            mSpriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawGameWorld()
        {
        }
    }
}
