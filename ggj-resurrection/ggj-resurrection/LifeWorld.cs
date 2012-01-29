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
    class LifeWorld : GameWorld
    {
        private int[,] terrainMap;
        private int dimension;
        private Texture2D tile1, tile2, tile3, tile4, tile5;
        

        public LifeWorld(String file, Game game)
            : base()
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


           
        }

        public void loadTiles(Game game)
        {
            tile1 = game.Content.Load<Texture2D>("Grass001");
            tile2 = game.Content.Load<Texture2D>("Grass002");
            tile3 = game.Content.Load<Texture2D>("Grass003");
        }

        public void Draw(SpriteBatch mSpriteBatch)
        {

            for (int i = dimension - 1; i >= 0; --i)
            {
                for (int j = dimension - 1; j >= 0 ; --j)
                {
                    int temp = terrainMap[i, j];
                    
                    switch (temp)
                    {
                        case 1:
                            mSpriteBatch.Draw(tile1, new Vector2((50f * j) -400, (50 * i) - 460) * Camera.kPixelsToUnits, null, Color.White, 0f, new Vector2(0, 0), Camera.kPixelsToUnits, SpriteEffects.FlipVertically, 1f);
                            break;
                        case 2:
                            mSpriteBatch.Draw(tile2, new Vector2((50 * j) - 400, (50 * i) - 460) * Camera.kPixelsToUnits, null, Color.White, 0f, new Vector2(0, 0), Camera.kPixelsToUnits, SpriteEffects.FlipVertically, 1f);
                            break;
                        case 3:
                            mSpriteBatch.Draw(tile3, new Vector2((50 * j) - 400, (50 * i) - 460) * Camera.kPixelsToUnits, null, Color.White, 0f, new Vector2(0, 0), Camera.kPixelsToUnits, SpriteEffects.FlipVertically, 1f);
                            break;
                    }
                }

            }

            base.Draw(mSpriteBatch);


        }

    }

}
