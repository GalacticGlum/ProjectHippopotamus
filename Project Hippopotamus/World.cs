using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hippopotamus
{
    public class World
    {
        public int Width { get; }
        public int Height { get; }

        private readonly Tile[,] tiles;

        public World(int width, int height)
        {
            Width = width;
            Height = height;

            tiles = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = new Tile(x, y, TileType.Empty);
                }
            }
        }

        public Tile GetTileAt(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return null;
            }

            return tiles[x, y];
        }
    }
}
