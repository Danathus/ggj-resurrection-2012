using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ggj_resurrection
{
    class SpriteSheet
    {
        class Sprite
        {
            Vector2 mCorner, mDimensions;

            public Sprite(Vector2 corner, Vector2 dimensions)
            {
                mCorner = corner;
                mDimensions = dimensions;
            }

            public Rectangle GetRectangle()
            {
                return new Rectangle(
                    (int)mCorner.X, (int)mCorner.Y,        // x, y
                    (int)mDimensions.X, (int)mDimensions.Y // width, height
                    );
            }
        };

        Texture2D mTexture;
        Sprite[] mSprites;
        int mNextSpriteToAdd;

        public SpriteSheet()
        {
            mSprites = new Sprite[64];
            mNextSpriteToAdd = 0;
        }

        public void SetTexture(Texture2D texture) { mTexture = texture; }

        public int AddSprite(Vector2 corner, Vector2 dimensions)
        {
            if (mNextSpriteToAdd >= mSprites.Length)
            {
                // error
                return -1;
            }

            mSprites[mNextSpriteToAdd] = new Sprite(corner, dimensions);

            // move up for next time, return current value
            return mNextSpriteToAdd++;
        }

        public void Draw(SpriteBatch spriteBatch, int spriteIdx, Vector2 position, float rotation, Color color)
        {
            spriteBatch.Draw(
                mTexture,                           // texture
                position,                           // position
                mSprites[spriteIdx].GetRectangle(), // source rectangle
                color,                              // color
                rotation,                           // rotation
                new Vector2(0, 0),                  // origin
                2 * new Vector2(Camera.kPixelsToUnits, -Camera.kPixelsToUnits), // scale
                SpriteEffects.None,                 // effects
                0f);                                // layer depth
        }
    }
}
