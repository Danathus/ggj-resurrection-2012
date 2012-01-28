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
    public abstract class GameObject
    {
        protected Vector2   mPosition, mDirection;
        protected Fixture   mFixture;
        protected World     mWorld;

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);

        public GameObject(GraphicsDeviceManager gdm, World world)
        {
            mPosition = new Vector2(0, 0);
            mDirection = new Vector2(1, 0);
            Color color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            mWorld = world;
            //mTexture = CreateRectangle(gdm.GraphicsDevice, 32, 32, color);
        }

        /*
        protected Texture2D CreateRectangle(GraphicsDevice graphicsDevice, int width, int height, Color colori)
        {
            Texture2D rectangleTexture = new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
            Color[] color = new Color[width * height];//set the color to the amount of pixels in the textures
            for (int i = 0; i < color.Length; i++)//loop through all the colors setting them to whatever values we want
            {
                color[i] = colori;
            }
            rectangleTexture.SetData(color);//set the color data on the texture
            return rectangleTexture;//return the texture
        }*/
    };
}
