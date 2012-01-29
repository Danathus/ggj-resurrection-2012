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
    class PuzzleGenerator
    {
        static Random randomGenerator = new Random();
        public List<int[,]> generatePuzzleSections(int amount)
        {
            List<Vector2> entries = new List<Vector2>(); 
            List<int[,]> generatedSections = new List<int[,]>();
            for (int i = 0; i < amount; ++i)
            {
                int[,] chunk = new int[4,4];
                for (int j = 0; j < 2; j++)
                {
                    Vector2 temp;
                    temp.X = randomGenerator.Next(0, 3);
                    temp.Y = randomGenerator.Next(1, 2);
                    entries.Add(temp);

                    temp.X = randomGenerator.Next(1, 2);
                    temp.Y = randomGenerator.Next(0, 3);
                    entries.Add(temp);
                }

                for (int k = 0; k < 4; k++) {
                    for (int l = 0; l < 4; l++) {
                        chunk[k,l] = 0;
                    }
                }

                foreach (Vector2 address in entries)
                {
                    int r, c;
                    r = (int)address.X;
                    c = (int)address.Y;

                    chunk[r, c] = 1;

                }

                generatedSections.Add(chunk);
                entries.Clear();


            }

            return generatedSections;
                
        }

    }


}
