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
    class SpriteAnimation
    {
        class Frame
        {
            SpriteSheet mSpriteSheet; // reference to a sprite sheet
            int         mSpriteIdx;
            float       mDuration; // in seconds

            public Frame(SpriteSheet spriteSheet, int spriteIdx, float duration)
            {
                mSpriteSheet = spriteSheet;
                mSpriteIdx   = spriteIdx;
                mDuration    = duration;
            }

            public SpriteSheet GetSpriteSheet() { return mSpriteSheet; }
            public int GetSpriteIdx() { return mSpriteIdx; }
            public float GetDuration() { return mDuration; }
        }

        Frame[] mFrames;
        int mNextFrameToAdd;

        public SpriteAnimation()
        {
            mFrames = new Frame[64];
            mNextFrameToAdd = 0;
        }

        public int AddFrame(SpriteSheet spriteSheet, int spriteIdx, float durationInSeconds)
        {
            if (mNextFrameToAdd >= mFrames.Length)
            {
                // error
                return -1;
            }

            mFrames[mNextFrameToAdd] = new Frame(spriteSheet, spriteIdx, durationInSeconds);

            // move up for next time, return current value
            return mNextFrameToAdd++;
        }

        public float GetFrameLength(int frameIdx) { return mFrames[frameIdx].GetDuration();  }
        public int GetNumFrames() { return mNextFrameToAdd; }

        public void Draw(SpriteBatch spriteBatch, int frameIdx, Vector2 position, float rotation, Color color)
        {
            Frame currFrame = mFrames[frameIdx];
            currFrame.GetSpriteSheet().Draw(
                spriteBatch, currFrame.GetSpriteIdx(), position,  rotation, color
                );
        }
    }
}
