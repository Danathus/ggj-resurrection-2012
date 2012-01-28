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
using FarseerPhysics.DebugViews;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;

namespace ggj_resurrection
{
    public class Camera : GameObject
    {
        // screen info
        Vector2 mScreenDimensions;

        // positional data ("raw" values)
        Vector3 mRot;
        float mZoom;

        // matrices ("cooked" values)
        public Matrix mProjectionMatrix;
        public Matrix mViewMatrix;

        public Camera(World world, Vector2 initPos, Vector2 screenDimensions)
            : base(world, initPos)
        {
            mScreenDimensions = screenDimensions;
            //
            mRot = new Vector3(0, 0, 0);
            mZoom = 1.0f;
            //
            mProjectionMatrix = Matrix.CreateOrthographicOffCenter(
                -mScreenDimensions.X / 2, mScreenDimensions.X / 2, // left, right
                -mScreenDimensions.Y / 2, mScreenDimensions.Y / 2, // bottom, top
                -1000f, 1000f);                                // near, far
            mViewMatrix = Matrix.Identity;
        }

        public override void Update(GameTime gameTime)
        {
            // update positional data
            //mRot.Z += 10.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // generate view matrix from positional data
            {
                Matrix preRotTranslationMatrix =
                    Matrix.Identity;
                //Matrix.CreateTranslation(
                //-mScreenDimensions.X / 2,
                //-mScreenDimensions.Y / 2, 0
                //);
                Matrix rotationMatrix = Matrix.CreateRotationZ(MathHelper.ToRadians(mRot.Z));
                Matrix postRotTranslationMatrix =
                    Matrix.Identity;
                //Matrix.CreateTranslation(
                //mScreenDimensions.X / 2,
                //mScreenDimensions.Y / 2, 0
                //);
                Matrix zoomMatrix = Matrix.CreateScale(mZoom);

                //Matrix translationMatrix =
                //  Matrix.Identity
                //Matrix.CreateTranslation(
                //-mScreenDimensions.X / 2,
                //-mScreenDimensions.Y / 2, 0)
                //;

                Matrix compositeMatrix = preRotTranslationMatrix * rotationMatrix * postRotTranslationMatrix * zoomMatrix;

                mViewMatrix = compositeMatrix;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // what is this nonsense, to draw the camera?
        }
    };
}
