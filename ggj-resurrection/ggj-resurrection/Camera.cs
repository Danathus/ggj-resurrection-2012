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
using FarseerPhysics.DebugViews;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;

namespace ggj_resurrection
{
    public class Camera : GameObject
    {
        // screen info
        public Vector2 mScreenDimensions;

        // reference frame info
        public const float kPixelsToUnits = 1 / 64f; // k for constant!

        // positional data ("raw" values)
        public Vector3 mRot, mTargetRot;
        float mZoom;

        // matrices ("cooked" values)
        public Matrix mProjectionMatrix;
        public Matrix mTopViewMatrix;

        public Matrix GetTopViewMatrix()
        {
            return mTopViewMatrix;
        }

        public Camera(World world, Vector2 initPos, Vector2 screenDimensions)
            : base(world, initPos)
        {
            mScreenDimensions = screenDimensions;
            //
            mRot       = new Vector3(0, 0, 0);
            mTargetRot = new Vector3(0, 0, 0);
            mZoom      = 1.0f;
            //
            // multiply by kPixelsToUnits to make 1.0 in space equal the appropriate number of pixels
            mProjectionMatrix = Matrix.CreateOrthographicOffCenter(
                -mScreenDimensions.X / 2 * kPixelsToUnits, mScreenDimensions.X / 2 * kPixelsToUnits, // left, right
                -mScreenDimensions.Y / 2 * kPixelsToUnits, mScreenDimensions.Y / 2 * kPixelsToUnits, // bottom, top
                -1000f, 1000f);                                              // near, far
            //
            mTopViewMatrix  = Matrix.Identity;
        }

        public override void Update(GameTime gameTime)
        {
            // update positional data
            //mRot.Z += 10.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //mRot.X += 10.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // framerate-independent ease-in
            float k = 0.99f;
            float weight = (float)Math.Pow(1f - k, (float)gameTime.ElapsedGameTime.TotalSeconds); //0.9f;
            mRot = weight * mRot + (1f - weight) * mTargetRot;
            //weight = 0.99
            //camera_position = weight*camera_position + (1-weight)*target_position
            //
            //weight = (1-k) ^ dt
            //

            // generate top-view matrix from positional data
            {
                Matrix preRotTranslationMatrix =
                    Matrix.CreateTranslation(new Vector3(mPosition.X, mPosition.Y, 0));
                Matrix rotationMatrix = Matrix.CreateRotationX(MathHelper.ToRadians(mRot.X));
                Matrix postRotTranslationMatrix =
                    Matrix.Identity;
                Matrix zoomMatrix = Matrix.CreateScale(mZoom);

                Matrix compositeMatrix = preRotTranslationMatrix * rotationMatrix * postRotTranslationMatrix * zoomMatrix;

                mTopViewMatrix = compositeMatrix;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // what is this nonsense, to draw the camera?
        }
    };
}
