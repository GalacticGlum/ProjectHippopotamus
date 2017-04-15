using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hippopotamus.Engine.UI
{
    public abstract class Widget
    {
        public Vector2 Offset { get; set; }
        protected Rectangle Destination { get; set; }

        protected void CalculateDestination()
        {
            
        }
    }
}
