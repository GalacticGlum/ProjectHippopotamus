using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hippopotamus
{
    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }

        public TileType Type { get; set; }

        public Tile(int x, int y, TileType type)
        {
            X = x;
            Y = y;

            Type = type;
        }
    }
}
