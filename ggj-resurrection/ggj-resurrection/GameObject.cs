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
    public abstract class GameObject : IComparable<GameObject>
    {
        private bool        mEnabled;
        protected Vector2   mPosition;
        protected Vector2   mDirection;
        protected Vector2   mTopLeftPos;
        // protected Vector2   mVelocity;
        protected Fixture   mFixture;
        //protected Body      mBody;
        protected World     mPhysicsWorld;
        protected GameWorld mGameWorld; // the game world this object is in

        protected float mRadius;        //radius of physics object (usually circle)
        protected float mDensity;

        public int mHealth;

        public void SetPosition(Vector2 pos) { mPosition = pos; }
        public Vector2 GetPosition() { return mPosition; }
        public void SetVelocity(Vector2 velo) { mFixture.Body.LinearVelocity = velo; }
        public Vector2 GetVelocity() { return mFixture.Body.LinearVelocity; }
        public void Enable()    { mEnabled = true;  }
        public void Disable()   { mEnabled = false; }
        public bool IsEnabled() { return mEnabled;  }

        public void SetGameWorld(GameWorld gameWorld) { mGameWorld = gameWorld; }
        public GameWorld GetGameWorld() { return mGameWorld; }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);

        public GameObject(World world, Vector2 initPos){
            mPosition = initPos;
           // mDirection = new Vector2(1, 0);
            //mVelocity = new Vector2(0, 0);
            Color color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            mPhysicsWorld = world;
            
            //mTexture = CreateRectangle(gdm.GraphicsDevice, 32, 32, color);

            mEnabled = true;
        }

        public void fixtureDestory()
        {
            if (mFixture != null && mFixture.Body != null)
            {
                mFixture.Body.Dispose();
            }
            //mFixture.Dispose();
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

        public int CompareTo(GameObject other)
        {
            return //this.mPosition.Y.CompareTo(other.mPosition.Y);
                this.mPosition.Y.CompareTo(other.mPosition.Y) * -1;
        }


    };
}
