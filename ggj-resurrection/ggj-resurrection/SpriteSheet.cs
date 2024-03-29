﻿using System;
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
    public class SpriteSheet
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
        }

        public class SpriteRenderingParameters
        {
            Vector2 mPosition, mScale;
            float   mRotation;
            Color   mColor;
            bool    mFlipX;

            public SpriteRenderingParameters()
            {
                mPosition = new Vector2(0, 0);
                mRotation = 0.0f;
                mColor    = Color.White;
                mScale    = new Vector2(1, 1);
                mFlipX    = false;
            }

            public SpriteRenderingParameters(Vector2 position, float rotation, Color color, Vector2 scale, bool flipX = false)
            {
                mPosition = position;
                mRotation = rotation;
                mColor    = color;
                mScale    = scale;
                mFlipX    = flipX;
            }

            public SpriteRenderingParameters(SpriteRenderingParameters copyMe)
            {
                mPosition = copyMe.mPosition;
                mRotation = copyMe.mRotation;
                mColor    = copyMe.mColor;
                mScale    = copyMe.mScale;
            }

            public void SetScale(Vector2 scale) { mScale = scale; }
            public void SetFlipX(bool flipX)    { mFlipX = flipX; }

            public Vector2 GetPosition() { return mPosition; }
            public float   GetRotation() { return mRotation; }
            public Color   GetColor()    { return mColor; }
            public Vector2 GetScale()    { return mScale; }
            public bool ShouldFlipX()    { return mFlipX; }
        }

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

        public void Draw(SpriteBatch spriteBatch, int spriteIdx, SpriteRenderingParameters parameters)
        {
            // hmm...how to draw a sprite standing up?
            spriteBatch.Draw(
                mTexture,                           // texture
                new Vector2(                        // position
                    parameters.GetPosition().X, parameters.GetPosition().Y),
                mSprites[spriteIdx].GetRectangle(), // source rectangle
                parameters.GetColor(),              // color
                parameters.GetRotation(),           // rotation
                new Vector2(0, 0),                  // origin
                new Vector2(                        // scale
                    parameters.GetScale().X, parameters.GetScale().Y),
                parameters.ShouldFlipX()            // effects
                    ? SpriteEffects.FlipHorizontally
                    : SpriteEffects.None,
                0f);                                // layer depth
        }
    }
}
