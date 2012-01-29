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
            bool        mFlipped;

            public Frame(SpriteSheet spriteSheet, int spriteIdx, float duration, bool flipped)
            {
                mSpriteSheet = spriteSheet;
                mSpriteIdx   = spriteIdx;
                mDuration    = duration;
                mFlipped     = flipped;
            }

            public SpriteSheet GetSpriteSheet() { return mSpriteSheet; }
            public int GetSpriteIdx() { return mSpriteIdx; }
            public float GetDuration() { return mDuration; }
            public bool IsFlipped() { return mFlipped; }
        }

        Frame[] mFrames;
        int mNextFrameToAdd;

        public SpriteAnimation()
        {
            mFrames = new Frame[64];
            mNextFrameToAdd = 0;
        }

        public int AddFrame(SpriteSheet spriteSheet, int spriteIdx, float durationInSeconds, bool flipped = false)
        {
            if (mNextFrameToAdd >= mFrames.Length)
            {
                // error
                return -1;
            }

            mFrames[mNextFrameToAdd] = new Frame(spriteSheet, spriteIdx, durationInSeconds, flipped);

            // move up for next time, return current value
            return mNextFrameToAdd++;
        }

        public float GetFrameLength(int frameIdx) { return mFrames[frameIdx].GetDuration();  }
        public int GetNumFrames() { return mNextFrameToAdd; }

        public void Draw(SpriteBatch spriteBatch, int frameIdx, SpriteSheet.SpriteRenderingParameters parameters)
        {
            Frame currFrame = mFrames[frameIdx];
            if (currFrame.IsFlipped())
            {
                parameters = new SpriteSheet.SpriteRenderingParameters(parameters);
                parameters.SetFlipX(true);
            }
            currFrame.GetSpriteSheet().Draw(
                spriteBatch, currFrame.GetSpriteIdx(), parameters);
        }
    }
}
