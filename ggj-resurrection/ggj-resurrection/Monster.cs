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

using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;    

namespace ggj_resurrection
{
    class Monster : GameObject
    {

        public enum DIRECTION { UP, DOWN, LEFT, RIGHT } //Enum for direction of the char
        private DIRECTION currentDirection;
        private double timeElapsed;
        Random rand = new Random();

        public Monster(GraphicsDeviceManager gdm, World world)
            : base(gdm, world)
        {
            timeElapsed = 0;
            currentDirection = DIRECTION.RIGHT;
        }
        ~Monster()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mPosition, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeElapsed > 1000)
            {
                timeElapsed = 0;
                int randomNumber = rand.Next(1, 5);

                switch (randomNumber)
                {
                    //up
                    case 1:
                        currentDirection = DIRECTION.UP;
                        break;
                        
                    //right
                    case 2:
                        currentDirection = DIRECTION.RIGHT;
                        break;

                    //down
                    case 3:
                        currentDirection = DIRECTION.DOWN;
                        break;

                    //left
                    case 4:
                        currentDirection = DIRECTION.LEFT;
                        break;
                }
            }

            switch (currentDirection)
            {

                case DIRECTION.UP:
                    if (mPosition.Y <= 700) mPosition.Y += 3;
                    break;

                
                case DIRECTION.RIGHT:
                    if (mPosition.X <= 700) mPosition.X += 3;
                    break;

                
                case DIRECTION.DOWN:
                    if (mPosition.Y >= 100)  mPosition.Y -= 3;
                    break;

                
                case DIRECTION.LEFT:
                    if (mPosition.X >= 100) mPosition.X -= 3;
                    break;
            }
        }

        public override void LoadData(Game myGame)
        {
            mTexture = myGame.Content.Load<Texture2D>("monster");
        }
    }
}
