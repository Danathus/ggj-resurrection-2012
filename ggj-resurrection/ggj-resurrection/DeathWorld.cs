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
            //
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            mRenderingEffect.View = mCamera.GetSideViewMatrix();
            base.Draw(spriteBatch);
            mDebugView.RenderDebugData(ref mCamera.mProjectionMatrix, ref mCamera.mSideViewMatrix);
        }

        public override void DrawCustomWorldDetails(SpriteBatch spriteBatch)
        {
        }
    }
}
