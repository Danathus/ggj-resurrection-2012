using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SecretGame
{
    class Segment
    {
        Texture2D seg;


        public Rectangle bounds { get; set; }

        public int width;
        public int locx {get;set;}
        public int locy {get;set;}

        public Segment()
        {
            locx = 50;
            locy = 50;       
        }

        public Segment(int x, int y)
        {
            locx = x;
            locy = y;    
        }


        public void LoadSegment(Game snakegame)
        {
            seg = snakegame.Content.Load<Texture2D>("Images/texture");
            bounds = new Rectangle(locx, locy, seg.Width, seg.Height);
            width = seg.Width;      //what's the point of width?
        }

        public void DrawSegment(SpriteBatch sp)
        {
            sp.Draw(seg, new Vector2(locx, locy), Color.YellowGreen);
        }

        public void Move(int locationx, int locationy)
        {
            locx = locationx;
            locy = locationy;
        }

        public Rectangle boundBox()
        {
            return new Rectangle(locx, locy, seg.Width, seg.Height);
        }
    }
}
