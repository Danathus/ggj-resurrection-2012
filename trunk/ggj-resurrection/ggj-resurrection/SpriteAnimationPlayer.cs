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
    public class SpriteAnimationPlayer
    {
        SpriteAnimation mAnimToPlay;
        int             mCurrFrame;
        float           mSecondsOnCurrFrame;
        bool            mPlaying;

        public SpriteAnimationPlayer()
        {
            mAnimToPlay         = null;
            mCurrFrame          = 0;
            mSecondsOnCurrFrame = 0;
            mPlaying            = false;
        }

        public void SetAnimationToPlay(SpriteAnimation animation)
        {
            mAnimToPlay = animation;
            Stop();
        }

        public SpriteAnimation GetAnimationToPlay() { return mAnimToPlay; }

        public bool IsPlaying() { return mPlaying; }

        public void Play()
        {
            mPlaying = true;
        }

        public void Pause()
        {
            mPlaying = false;
        }

        public void Stop()
        {
            mPlaying            = false;
            mCurrFrame          = 0;
            mSecondsOnCurrFrame = 0;

            if (mAnimToPlay != null)
            {
                mSecondsOnCurrFrame = mAnimToPlay.GetFrameLength(mCurrFrame);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (mPlaying)
            {
                mSecondsOnCurrFrame -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (mSecondsOnCurrFrame <= 0)
                {
                    // advance to next frame
                    ++mCurrFrame;
                    // if we ran off the end, stop
                    if (mCurrFrame >= mAnimToPlay.GetNumFrames())
                    {
                        Stop();
                    }

                    // adjust the seconds-on-current-frame countdown timer accordingly
                    mSecondsOnCurrFrame += mAnimToPlay.GetFrameLength(mCurrFrame);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteSheet.SpriteRenderingParameters parameters)
        {
            if (mAnimToPlay != null)
            {
                mAnimToPlay.Draw(spriteBatch, mCurrFrame, parameters);
            }
        }
    }
}
