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

namespace ggj_resurrection
{
    public class Player : GameObject
    {
        KeyboardState mCurrKeyboardState, mPrevKeyboardState;

        public Player(GraphicsDeviceManager gdm)
            : base(gdm)
        {
            mPrevKeyboardState = mCurrKeyboardState = Keyboard.GetState();
        }
        ~Player()
        {
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mPosition, Color.YellowGreen);
        }

        public override void Update(GameTime gameTime)
        {
            mPrevKeyboardState = mCurrKeyboardState;
            mCurrKeyboardState = Keyboard.GetState();

            Vector2 direction = new Vector2(0, 0);
            if (mCurrKeyboardState.IsKeyDown(Keys.Right)) direction.X += 1.0f;
            if (mCurrKeyboardState.IsKeyDown(Keys.Left)) direction.X -= 1.0f;
            if (mCurrKeyboardState.IsKeyDown(Keys.Up)) direction.Y -= 1.0f;
            if (mCurrKeyboardState.IsKeyDown(Keys.Down)) direction.Y += 1.0f;
            if (direction.Length() > 0) direction.Normalize();

            const float speed = 300.0f;
            mPosition += speed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void LoadData(Game myGame)
        {

        }
       
    }
}
