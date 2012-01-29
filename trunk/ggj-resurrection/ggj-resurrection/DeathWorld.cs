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
    public class DeathWorld : GameWorld
    {
        float mCountdownTimer; // counts down until the end of the game

        int[,] graveyardMap;
        Random deathRand = new Random();
        PuzzleGenerator graveyardMaker;
        List<int[,]> puzzleChunks;

        enum DeathWorldStates
        {
            DWS_MAIN,
            DWS_WRAPUP
        };
        DeathWorldStates mState; // states of the death world!

        public DeathWorld(Camera camera)
            : base(camera)
        {
            puzzleChunks = new List<int[,]>();
            graveyardMaker = new PuzzleGenerator();
            graveyardMap = new int[16, 16];
            puzzleChunks = graveyardMaker.generatePuzzleSections(32);

            List<int> selections = new List<int>();
            List<int[,]> initialMap = new List<int[,]>();
            for (int i = 0; i < 16; i++)
            {
                int temp = deathRand.Next(1, 32);
                selections.Add(temp);
            }

            foreach (int s in selections)
            {
                initialMap.Add(puzzleChunks.ElementAt(s-1));
            }

            for (int j = 0; j < 16; j++)
            {
                int[,] temp = initialMap.ElementAt(j);
                switch (j)
                {
                    case 0:
                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                graveyardMap[k,l] = temp[k,l];
                            }

                        }
                        break;
                    case 1:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m+4,n] = temp[m,n];
                            }

                        }
                        break;
                    case 2:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m+8,n] = temp[m,n];
                            }

                        }
                        break;
                    case 3:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m+12,n] = temp[m,n];
                            }

                        }
                        break;
                    case 4:
                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                graveyardMap[k, l+4] = temp[k, l];
                            }

                        }
                        break;
                    case 5:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 4, n+4] = temp[m, n];
                            }

                        }
                        break;
                    case 6:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m+8, n + 4] = temp[m, n];
                            }

                        }
                        break;
                    case 7:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 12, n + 4] = temp[m, n];
                            }

                        }
                        break;
                    case 8:
                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                graveyardMap[k, l+8] = temp[k, l];
                            }

                        }
                        break;
                    case 9:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 4, n+8] = temp[m, n];
                            }

                        }
                        break;
                    case 10:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 8, n+8] = temp[m, n];
                            }

                        }
                        break;
                    case 11:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 12, n+8] = temp[m, n];
                            }

                        }
                        break;
                    case 12:
                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                graveyardMap[k, l + 12] = temp[k, l];
                            }

                        }
                        break;
                    case 13:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 4, n + 12] = temp[m, n];
                            }

                        }
                        break;
                    case 14:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 8, n + 12] = temp[m, n];
                            }

                        }
                        break;
                    case 15:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 12, n + 12] = temp[m, n];
                            }

                        }
                        break;

                }

            }

        }

        private void generateGraveyard()
        {
            graveyardMap = new int[16, 16];
            List<int> selections = new List<int>();
            List<int[,]> initialMap = new List<int[,]>();
            for (int i = 0; i < 16; i++)
            {
                int temp = deathRand.Next(1, 32);
                selections.Add(temp);
            }

            foreach (int s in selections)
            {
                initialMap.Add(puzzleChunks.ElementAt(s - 1));
            }

            for (int j = 0; j < 16; j++)
            {
                int[,] temp = initialMap.ElementAt(j);
                switch (j)
                {
                    case 0:
                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                graveyardMap[k, l] = temp[k, l];
                            }

                        }
                        break;
                    case 1:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 4, n] = temp[m, n];
                            }

                        }
                        break;
                    case 2:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 8, n] = temp[m, n];
                            }

                        }
                        break;
                    case 3:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 12, n] = temp[m, n];
                            }

                        }
                        break;
                    case 4:
                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                graveyardMap[k, l + 4] = temp[k, l];
                            }

                        }
                        break;
                    case 5:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 4, n + 4] = temp[m, n];
                            }

                        }
                        break;
                    case 6:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 8, n + 4] = temp[m, n];
                            }

                        }
                        break;
                    case 7:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 12, n + 4] = temp[m, n];
                            }

                        }
                        break;
                    case 8:
                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                graveyardMap[k, l + 8] = temp[k, l];
                            }

                        }
                        break;
                    case 9:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 4, n + 8] = temp[m, n];
                            }

                        }
                        break;
                    case 10:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 8, n + 8] = temp[m, n];
                            }

                        }
                        break;
                    case 11:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 12, n + 8] = temp[m, n];
                            }

                        }
                        break;
                    case 12:
                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                graveyardMap[k, l + 12] = temp[k, l];
                            }

                        }
                        break;
                    case 13:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 4, n + 12] = temp[m, n];
                            }

                        }
                        break;
                    case 14:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 8, n + 12] = temp[m, n];
                            }

                        }
                        break;
                    case 15:
                        for (int m = 0; m < 4; m++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                graveyardMap[m + 12, n + 12] = temp[m, n];
                            }

                        }
                        break;
                }
            }
        }

        public override void WakeUp()
        {
            base.WakeUp();
            //AddGameObject(new Grave(mPhysicsWorld, new Vector2(0, 0)));

            for (int y = 0; y < 16; ++y)
            {
                for (int x = 0; x < 16; ++x)
                {
                    if (graveyardMap[y, x] == 1)
                    {
                        Grave grave = new Grave(
                            mPhysicsWorld,
                            new Vector2(x - 8, y - 8) * 50 * Camera.kPixelsToUnits,
                            new Vector2(x, y));
                        grave.mDeathWorld = this;
                        AddGameObject(grave);
                    }
                }
            }

            score = 0;
            // for now set countdown timer to 10 seconds
            mCountdownTimer = 10f;

            // initialize state to first
            mState = DeathWorldStates.DWS_MAIN;
        }

        public bool IsTileOccupied(Vector2 tileIdx)
        {
            if (tileIdx.X < 0 || tileIdx.X >= 16 ||
                tileIdx.Y < 0 || tileIdx.Y >= 16)
            {
                return true;
            }
            return graveyardMap[(int)tileIdx.Y, (int)tileIdx.X] == 1;
        }

        public void SetTileOccupied(Vector2 tileIdx, bool occupied)
        {
            if (tileIdx.X < 0 || tileIdx.X >= 16 ||
                tileIdx.Y < 0 || tileIdx.Y >= 16)
            {
                return;
            }
            graveyardMap[(int)tileIdx.Y, (int)tileIdx.X] = occupied ? 1 : 0;
        }

        public virtual void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            mCountdownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            switch (mState)
            {
            case DeathWorldStates.DWS_MAIN:
                if (mCountdownTimer < 0)
                {
                    // go to coalesce phase
                    mState = DeathWorldStates.DWS_WRAPUP;
                    // three seconds should be safe, right?
                    mCountdownTimer = 3.0f;
                    //mPlayer.Disable();
                    mPlayer.mControllable = false;
                }
                break;
            case DeathWorldStates.DWS_WRAPUP:
                // collect all the souls
                foreach (GameObject go in mGameObjects)
                {
                    if (go is Soul)
                    {
                        float k = 0.99f;
                        float weight = (float)Math.Pow(1f - k, (float)gameTime.ElapsedGameTime.TotalSeconds); //0.9f;
                        go.SetPosition(weight * go.GetPosition() + (1f - weight) * mPlayer.GetPosition());
                    }
                }
                // when you're done, transition out
                if (mCountdownTimer < 0)
                {
                    // go to coalesce phase
                    Game1.mDesiredWorld = Game1.mLifeWorld;
                }
                break;
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // apply the camera view and projection matrices by passing a BasicEffect to the SpriteBatch
            mRenderingEffect.World = Matrix.Identity;
            mRenderingEffect.Projection = mCamera.mProjectionMatrix;
            //
            mRenderingEffect.TextureEnabled = true;
            mRenderingEffect.VertexColorEnabled = true;

            if (mAwake)
            {
                spriteBatch.Begin(
                        SpriteSortMode.Immediate,   // sprite sort mode (which is better, immediate or deffered?)
                        BlendState.AlphaBlend,      // blend state
                        SamplerState.LinearClamp,   // sampler state
                        DepthStencilState.None,     // depth stencil state
                        RasterizerState.CullNone,   // rasterizer state
                        null,           // effect (formerly null)
                        Matrix.Identity);           // transform matrix
                const float textShift = 4f / 5f;
                spriteBatch.DrawString(drawFont, "Collect souls", new Vector2(0, mCamera.mScreenDimensions.Y * textShift), Color.DarkGreen);
                spriteBatch.DrawString(drawFont, "from graves!", new Vector2(0, mCamera.mScreenDimensions.Y * textShift + 64), Color.DarkGreen);
                spriteBatch.End();
            }

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
            //mDebugView.RenderDebugData(ref mCamera.mProjectionMatrix, ref mCamera.mTopViewMatrix);
        }

        public override void DrawCustomWorldDetails(SpriteBatch spriteBatch)
        {
            // this doesn't really apply to the Death World, right?
        }

        public override void GoToSleep()
        {
            base.GoToSleep();

            // regenerate map
            generateGraveyard();
        }
    }
}
