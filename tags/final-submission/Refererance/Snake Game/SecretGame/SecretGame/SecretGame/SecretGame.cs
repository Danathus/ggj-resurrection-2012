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

namespace SecretGame
{
   
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    public enum DIRECTION { UP, DOWN, LEFT, RIGHT } //Enum for direction of the char

    public class SecretGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        snakeChar playerSnake;
        toCollect heads;

        KeyboardState oldState, newState;

        DIRECTION lastDirection = DIRECTION.RIGHT;
        
        int speed = 3;
        int score = 0;
        int lives = 3;

        SpriteFont gameFont;

        public SecretGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
           
            playerSnake = new snakeChar();
            heads = new toCollect();
           
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatch.GraphicsDevice.Clear(Color.Black);
            gameFont = Content.Load<SpriteFont>("SpriteFont1");

            heads.LoadCollect(this);
            playerSnake.LoadThis(this);

            for (int i = 0; i < 10; i++)    //loads initial length of the snake
                playerSnake.chomp(this);
            
            
            // TODO: use this.Content to load your game content here
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

            newState = Keyboard.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if (newState.IsKeyDown(Keys.Right)&& lastDirection != DIRECTION.LEFT)                    //Changed isKeyUp to isKeyDown
                lastDirection = DIRECTION.RIGHT;                    //left and right is switched
            if (newState.IsKeyDown(Keys.Left) && lastDirection != DIRECTION.RIGHT)
                lastDirection = DIRECTION.LEFT;                     //left and right is switched
            if (newState.IsKeyDown(Keys.Up) && lastDirection != DIRECTION.DOWN)
                lastDirection = DIRECTION.UP;
            if (newState.IsKeyDown(Keys.Down) && lastDirection != DIRECTION.UP)
                lastDirection = DIRECTION.DOWN;
    
            /* Deleted "old state" checking, which made the game input slower and more unresponsive.
             * changed Right to isKeyDown instead of isKeyUp
             * 
             * Added code to reject input if user tries to move towards the opposite direction
             */

            if (oldState.IsKeyDown(Keys.G))
            {
                playerSnake.chomp(this);
                score++;
            }



            oldState = newState;
            playerSnake.allLength = 0;
            playerSnake.clientWidth = Window.ClientBounds.Width;
            playerSnake.clientHeight = Window.ClientBounds.Height;

            
            playerSnake.move(lastDirection, speed);

            if (heads.collideRand(Window.ClientBounds.Width, Window.ClientBounds.Height, playerSnake.headBox))
            {
                score++;
                playerSnake.chomp(this);
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.DrawString(gameFont, "Heads: " + score, new Vector2(5, 5), Color.Tomato);
            spriteBatch.DrawString(gameFont, "Lives: " + lives, new Vector2(Window.ClientBounds.Width - 100, 5), Color.Tomato);
            if (!playerSnake.isCollideSelf())
            {
                int rectLen = playerSnake.allLength;
                //String someString = "" + rectLen + "";
              

                //spriteBatch.DrawString(gameFont, someString, new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2), Color.White);
            }
            else  //RELOAD starting variables
            {
                //spriteBatch.DrawString(gameFont, "COOLLLIDDEEEE", new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2), Color.White);
                lives--;
                playerSnake.clearLists(this);
                lastDirection = DIRECTION.RIGHT;
                for (int i = 0; i < 10; i++)    //loads initial length of the snake
                    playerSnake.chomp(this);

                
            }

            if (lives <= 0)
            {
                spriteBatch.DrawString(gameFont, "You lost and disgraced your family. \nYou must now perform seppuku \nto redeem yourself", new Vector2(0, Window.ClientBounds.Height / 2), Color.White);
                                                   
            }
            if (lives > 0)
            {
                playerSnake.drawMe(spriteBatch);
                heads.drawCollect(spriteBatch);
            }
            

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
