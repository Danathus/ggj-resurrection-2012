using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace SecretGame
{
    class DIRECTION
    {
        public bool RIGHT { get; set { this.RIGHT = true; } }
        public bool LEFT { get; set; }
        public bool UP { get; set; }
        public bool DOWN { get; set; }

        public DIRECTION()
        {
            RIGHT = false;
            LEFT = false;
            UP = false;
            DOWN = false;
        }

 
    }

  
}
