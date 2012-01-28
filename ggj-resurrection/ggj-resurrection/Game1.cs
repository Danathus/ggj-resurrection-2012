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

        public class Camera : GameObject
        {
            // screen info
            Vector2 mScreenDimensions;

            // positional data ("raw" values)
            Vector3 mRot;
            float   mZoom;

            // matrices ("cooked" values)
            public Matrix mProjectionMatrix;
            public Matrix mViewMatrix; // formerly mDebugCameraMatrix
            //public Matrix mDebugCameraMatrix;

            public Camera(World world, Vector2 initPos, Vector2 screenCenter, Vector2 screenDimensions)
                : base(world, initPos)
            {
                mScreenDimensions = screenDimensions;
                //
                mRot = new Vector3(0, 0, 0);
                mZoom = 1.0f;
                //
                mProjectionMatrix = Matrix.CreateOrthographicOffCenter(
                    -mScreenDimensions.X/2, mScreenDimensions.X/2, // left, right
                    -mScreenDimensions.Y/2, mScreenDimensions.Y/2, // bottom, top
                    -1000f, 1000f);                                // near, far
                mViewMatrix = Matrix.Identity;
            }

            public override void Update(GameTime gameTime)
            {
                // update positional data
                //mRot.Z += 10.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // generate view matrix from positional data
                {
                    Matrix preRotTranslationMatrix =
                        Matrix.Identity;
                        //Matrix.CreateTranslation(
                        //-mScreenDimensions.X / 2,
                        //-mScreenDimensions.Y / 2, 0
                        //);
                    Matrix rotationMatrix = Matrix.CreateRotationZ(MathHelper.ToRadians(mRot.Z));
                    Matrix postRotTranslationMatrix =
                        Matrix.Identity;
                        //Matrix.CreateTranslation(
                        //mScreenDimensions.X / 2,
                        //mScreenDimensions.Y / 2, 0
                        //);
                    Matrix zoomMatrix = Matrix.CreateScale(mZoom);

                    //Matrix translationMatrix =
                      //  Matrix.Identity
                        //Matrix.CreateTranslation(
                        //-mScreenDimensions.X / 2,
                        //-mScreenDimensions.Y / 2, 0)
                        //;

                    Matrix compositeMatrix = preRotTranslationMatrix * rotationMatrix * postRotTranslationMatrix * zoomMatrix;

                    mViewMatrix = compositeMatrix;
                }
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                // what is this nonsense, to draw the camera?
            }
        };
        Camera mCamera;

        DebugViewXNA mDebugView;

        Player mPlayer;

        Vector2 mScreenCenter;

        MouseState mCurrMouseState;

        public Game1()
        {
            mGraphics = new GraphicsDeviceManager(this);
            mGraphics.PreferredBackBufferWidth  = 800;
            mGraphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
            mPhysicsWorld = new World(new Vector2(0, 0));

            mDebugView = new DebugViewXNA(mPhysicsWorld);
            mPlayer    = new Player(mPhysicsWorld, new Vector2(0, 0));
            
            mLifeWorld    = new LifeWorld();
            mDeathWorld   = new DeathWorld();

            mCurrentWorld = mLifeWorld;
          
            mLifeWorld.AddGameObject(mPlayer);
            mLifeWorld.AddGameObject( new MonsterSpawner(mPhysicsWorld, new Vector2(0,0)) );

            mScreenCenter = new Vector2(Window.ClientBounds.Width / 2f, Window.ClientBounds.Height / 2f);
            mCamera = new Camera(
                mPhysicsWorld, new Vector2(0, 0),
                mScreenCenter,
                new Vector2(mGraphics.PreferredBackBufferWidth, mGraphics.PreferredBackBufferHeight));
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

            Player.LoadData(this);
            MonsterSpawner.LoadData(this);
            Monster.LoadData(this);
            SwordSlash.LoadData(this);
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
            mCurrMouseState = Mouse.GetState();
            
            mLifeWorld.Update(gameTime);
            mDeathWorld.Update(gameTime);
            mCamera.Update(gameTime);

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

            // apply the camera view and projection matrices by passing a BasicEffect to the SpriteBatch
            BasicEffect basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.World       = Matrix.Identity;
            basicEffect.View        = mCamera.mViewMatrix;
            basicEffect.Projection  = mCamera.mProjectionMatrix;
            //
            basicEffect.TextureEnabled     = true;
            basicEffect.VertexColorEnabled = true;

            // custom drawing code here
            mSpriteBatch.Begin(
                SpriteSortMode.Immediate,   // sprite sort mode
                BlendState.AlphaBlend,      // blend state
                SamplerState.LinearClamp,   // sampler state
                DepthStencilState.None,     // depth stencil state
                RasterizerState.CullNone,   // rasterizer state
                basicEffect,                // effect (formerly null)
                Matrix.Identity);           // transform matrix
            mLifeWorld.Draw(mSpriteBatch);
            mDeathWorld.Draw(mSpriteBatch);

            mSpriteBatch.End();

            mDebugView.RenderDebugData(ref mCamera.mProjectionMatrix, ref mCamera.mViewMatrix);

            base.Draw(gameTime);
        }
    }
}
