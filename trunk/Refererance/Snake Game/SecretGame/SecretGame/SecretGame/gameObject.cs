using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SecretGame
{
    class gameObject
    {
        Texture2D seg;
        public int width;
        public int locx {get;set;}
        public int locy {get;set;}

        public gameObject()
        {
            locx = 50;
            locy = 50;
            
        }

        public gameObject(int x, int y)
        {
        /*
         * Initialize gameObject at a particular location 
         */

            locx = x;
            locy = y;    
        }


        public void LoadSegment(Game snakegame)
        {
         seg = snakegame.Content.Load<Texture2D>("Images/texture");         
         width = seg.Width;
        }

        public void DrawSegment(SpriteBatch sp)
        {
        /* 
         * DrawSegment draws spriteBatch at locx, locy with YellowGreen tint
         */
            sp.Draw(seg, new Vector2(locx, locy), Color.YellowGreen); 
            
        }

        public void Move(int locationx, int locationy)
        {
            /*
             * Move location to location indicated
             */
            locx = locationx;
            locy = locationy;
           
        }

    }
}
