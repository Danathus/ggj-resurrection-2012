using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ggj_resurrection
{
    public class LifeWorld : GameWorld
    {
        private int[,] terrainMap;
        private int dimension;
        private Texture2D tile1, tile2, tile3, heart;
        

        public LifeWorld(Camera camera, String file, Game game)
            : base(camera)
        {
           // try
            //{
                //StreamReader read = new StreamReader(file);
                String path = string.Format("Content/lifeworld.txt");
                Stream fileStream = TitleContainer.OpenStream(path);
                          
                StreamReader read = new StreamReader(fileStream);
                string line = read.ReadLine();
                char[] setMatrix = line.ToCharArray();
                char[] chars;
                
                /*String waste = setMatrix[0].ToString();
                
                dimension = (setMatrix[0] - 48);*/
                int count = 0;

                int.TryParse(line, out dimension);

                terrainMap = new int[dimension, dimension];
                line = read.ReadLine();
                while (line != null)
                {
                    
                    chars = line.ToCharArray();

                    for (int i = 0; i < chars.Length; i++)
                    {
                        if (i < dimension)
                        {
                            terrainMap[count, i] = (chars[i] - 48);
                        }
                    }

                    line = read.ReadLine();
                    count++;
                }

                read.Close();
           // }

          /*  catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }

            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }*/
        } // LifeWorld

        public void loadTiles(Game game)
        {
            tile1 = game.Content.Load<Texture2D>("Grass001");
            tile2 = game.Content.Load<Texture2D>("Grass002");
            tile3 = game.Content.Load<Texture2D>("Grass003");
            heart = game.Content.Load<Texture2D>("Heart");
        }

        

        public override void Draw(SpriteBatch spriteBatch)
        {
            mRenderingEffect.View = mCamera.GetTopViewMatrix();
            base.Draw(spriteBatch);


            //spriteBatch.Draw(
            //mDebugView.RenderDebugData(ref mCamera.mProjectionMatrix, ref mCamera.mTopViewMatrix);
        }

        public void drawHealth(SpriteBatch mSpriteBatch, GraphicsDeviceManager manager)
        {
            int hitpoints = mPlayer.mHealth;
            Vector2 referencePoint = new Vector2(manager.GraphicsDevice.Viewport.Width * .02f, manager.GraphicsDevice.Viewport.Height * .9f);
            //referencePoint *= Camera.kPixelsToUnits;
            mSpriteBatch.Begin();

            for (int i = 0; i < hitpoints; i++)
            {
                
                mSpriteBatch.Draw(heart, referencePoint, Color.White);
                referencePoint.X += (50f);
                
            }

            mSpriteBatch.End();
        }

        public override void WakeUp()
        {
            base.WakeUp();
            AddGameObject(new MonsterSpawner(mPhysicsWorld, new Vector2(0, 0), mPlayer));
            mScore = 0;
        }

        public override void DrawCustomWorldDetails(SpriteBatch spriteBatch)
        {
            for (int i = dimension - 1; i >= 0; --i)
            {
                for (int j = dimension - 1; j >= 0 ; --j)
                {
                    int temp = terrainMap[i, j];

                    Texture2D tileToDraw = null;
                    switch (temp)
                    {
                    case 1: tileToDraw = tile1; break;
                    case 2: tileToDraw = tile2; break;
                    case 3: tileToDraw = tile3; break;
                    }
                    if (tileToDraw != null)
                    {
                        spriteBatch.Draw(tileToDraw,     // texture
                            new Vector2(                  // position
                                (50f * j) - 400, (50 * i) - 460) * Camera.kPixelsToUnits,
                            null,                         // source rectangle
                            Color.White,                  // color
                            0f,                           // rotation
                            new Vector2(0, 0),            // origin
                            Camera.kPixelsToUnits,        // scale
                            SpriteEffects.FlipVertically, // effects
                            0f);                          // layer depth
                    }
                }
            } // for
        } // DrawCustomWorldDetails
    } // LifeWorld
} // namespace ggj_resurrection
