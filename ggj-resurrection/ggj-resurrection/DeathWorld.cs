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
    class DeathWorld : GameWorld
    {
        public DeathWorld(Camera camera)
            : base(camera)
        {
        }

        public override void WakeUp()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // apply the camera view and projection matrices by passing a BasicEffect to the SpriteBatch
            mRenderingEffect.World = Matrix.Identity;
            mRenderingEffect.Projection = mCamera.mProjectionMatrix;
            //
            mRenderingEffect.TextureEnabled = true;
            mRenderingEffect.VertexColorEnabled = true;

            foreach (GameObject go in mGameObjects)
            {
                if (go.IsEnabled())
                {
                    // reset view PER OBJECT
                    //
                    mRenderingEffect.View =
                        Matrix.CreateTranslation(//new Vector3(go.GetPosition().X, go.GetPosition().Y, 0) +
                            new Vector3(
                                go.GetPosition().X,
                                0,//go.GetPosition().Y,
                                -go.GetPosition().Y+
                                Player.kFrameSizeInPixels.Y / 2 * Camera.kPixelsToUnits))
                        * Matrix.CreateRotationX(MathHelper.ToRadians(90 - mCamera.mRot.X));
                    //
                    spriteBatch.Begin(
                        SpriteSortMode.Immediate,   // sprite sort mode (which is better, immediate or deffered?)
                        BlendState.AlphaBlend,      // blend state
                        SamplerState.LinearClamp,   // sampler state
                        DepthStencilState.None,     // depth stencil state
                        RasterizerState.CullNone,   // rasterizer state
                        mRenderingEffect,           // effect (formerly null)
                        Matrix.Identity);           // transform matrix

                    go.Draw(spriteBatch);

                    spriteBatch.End();
                }
            }

            // then draw the physics debug view...
            mDebugView.RenderDebugData(ref mCamera.mProjectionMatrix, ref mCamera.mTopViewMatrix);
        }

        public override void DrawCustomWorldDetails(SpriteBatch spriteBatch)
        {
            // this doesn't really apply to the Death World, right?
        }
    }
}
