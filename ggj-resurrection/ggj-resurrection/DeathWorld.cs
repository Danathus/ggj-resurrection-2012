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
        int[,] graveyardMap;
        Random deathRand = new Random();
        PuzzleGenerator graveyardMaker;
        List<int[,]> puzzleChunks;

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
