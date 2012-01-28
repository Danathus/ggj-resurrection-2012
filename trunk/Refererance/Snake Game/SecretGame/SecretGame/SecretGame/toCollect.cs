using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace SecretGame
{

    class toCollect
    {
            int locx {get; set;}
            int locy { get; set; }
            Texture2D texture;
            public Rectangle bounds { get; set; }   //collision box

            public toCollect()
            {
                locx = 250;
                locy = 250;
            }
            public void LoadCollect(Game thisGame)
            {
                texture = thisGame.Content.Load<Texture2D>("Images/texture");
                bounds = new Rectangle(locx, locy, texture.Width, texture.Height);
            }
            public bool collideRand(int boundWidth, int boundHeight, Rectangle check)
            {
                //object will colide with called rectangle, if collided, teleport to random location
                bounds = new Rectangle(locx, locy, texture.Width, texture.Height);
                Random random = new Random();
                if (check.Intersects(bounds))
                {
                    locx = random.Next(boundWidth);
                    locy = random.Next(boundHeight);
                    return true;
                }
                return false;
            }
            public void drawCollect(SpriteBatch sp)
            {
                sp.Draw(texture, new Vector2(locx, locy), Color.Red);
            }
    }
}
