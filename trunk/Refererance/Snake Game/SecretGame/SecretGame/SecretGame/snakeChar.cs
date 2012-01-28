using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SecretGame
{
    class snakeChar
    {

        private LinkedList<Segment> snakeList = new LinkedList<Segment>();
        public Rectangle headBox;
        public List<Rectangle> snakeBoxList = new List<Rectangle>();
        public int allLength {get; set;}
        public int clientWidth { get; set; }
        public int clientHeight { get; set; }

        public snakeChar()
        {
            Segment headSegment = new Segment();
            snakeList.AddFirst(headSegment);
        }

        public void LoadThis(Game myGame)
        {
            Segment first = (Segment)snakeList.First.Value;
            first.LoadSegment(myGame);
        }

        public void drawMe(SpriteBatch sp)
        {
            LinkedList<Segment>.Enumerator segIterator = snakeList.GetEnumerator();
            
            while (segIterator.MoveNext())
            {
                segIterator.Current.DrawSegment(sp);
            }
            segIterator.Dispose();

        }

        public void clearLists(Game myGame)
        {
            snakeBoxList.Clear();
            snakeList.Clear();
            Segment headSegment = new Segment();
            snakeList.AddFirst(headSegment);
            Segment first = (Segment)snakeList.First.Value;
            first.LoadSegment(myGame);
            
        }

        public void chomp(Game myGame)
        {
            /*
             *  "Eating" for the snake. Call when collide with
             *  an apple. Also increase points.
             */

            Segment newSeg = new Segment(snakeList.Last.Value.locx-snakeList.Last.Value.width, snakeList.Last.Value.locy);
            newSeg.LoadSegment(myGame);
            snakeList.AddAfter(snakeList.Last, newSeg);
            //snakeBoxList.Add(newSeg.boundBox());
             //For console checking not needed
        }

        public void move(DIRECTION dir, int speed)
        {
            /*
             *  ymultiplier and xmultiplier taken out
             *  Console.Out.WriteLine("Segments: {0}",snakeList.Count);
             *  taken out, console checking not needed.
             * 
 
             */
            LinkedList<Segment>.Enumerator segIterator = snakeList.GetEnumerator();

            headBox = snakeList.First.Value.boundBox();
            snakeBoxList.Clear();

            int tempx = snakeList.First.Value.locx;
            int tempy = snakeList.First.Value.locy;

            if (snakeList.First.Value.locx > clientWidth)       //code for wrap around
            {
                snakeList.First.Value.locx = 0;
            }
            else if (snakeList.First.Value.locx < 0)
            {
                snakeList.First.Value.locx = clientWidth;
            }
            if (snakeList.First.Value.locy > clientHeight)       //code for wrap around
            {
                snakeList.First.Value.locy = 0;
            }
            else if (snakeList.First.Value.locy < 0)
            {
                snakeList.First.Value.locy = clientHeight;
            }

            switch (dir)
            {
                case DIRECTION.DOWN:
                    snakeList.First.Value.locy += speed;
                    break;
                case DIRECTION.UP:
                    snakeList.First.Value.locy -= speed;
                    break;
                case DIRECTION.LEFT:
                    snakeList.First.Value.locx -= speed;
                    break;
                case DIRECTION.RIGHT:
                    snakeList.First.Value.locx += speed;
                    break;
            }

            int loopx, loopy;
            segIterator.MoveNext();


            while (segIterator.MoveNext())
            {
                /* 
                 *  Iterates movement and move each unit one
                 *  square foward. This is how the rest of the
                 *  Snake (besides the head) moves.
                 */

                loopx = segIterator.Current.locx;
                loopy = segIterator.Current.locy;

                segIterator.Current.Move(tempx,tempy);
                //loops each "current" unit to move one unit ahead
                
                

                tempx = loopx;
                tempy = loopy;
                //if(segIterator.Current.boundBox().Intersects(headBox))
                snakeBoxList.Add(segIterator.Current.boundBox());
            }
            
            
           foreach(Rectangle check in snakeBoxList) {
                allLength++;
            }
            segIterator.Dispose(); //dispose?

        }

        public bool isCollideSelf() //check if snake's head collided with body
        {
            for (int index = 1; index < snakeBoxList.Count; index++)
            {
                if (snakeBoxList[index].Intersects(headBox))
                {
                    return true;
                }
            }  
            return false;
        }

    }

}
