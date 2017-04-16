using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hippopotamus.Engine;

namespace Hippopotamus.World
{
    public static class TerrainCaveGenerator //: IWorldGenerator
    {
        private const int size = 16;
        private const int steps = 50;
        private const int birthCellThreshold = 4;
        private const int deathCellThreshold = 10;

        private static readonly Random random;

        static TerrainCaveGenerator()
        {
            random = new Random();
        }

        public static void Generate(Tile tile)
        {

            // Currently generates a circle around a given tile with a random radius from 0 to 3
            int radius = random.Next(0, 3);
            TileType type = random.NextDouble() > 0.8 ? TileType.Grass : TileType.Empty;

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    double distance = Math.Sqrt(x * x + y * y);
                    if (!(distance <= radius)) continue;              
                    World.Current.GetTileAt(tile.Position.X + x, tile.Position.Y + y).Type = type;
                }
            }
        }

        //public void Reseed()
        //{
        //    random = new Random();
        //}

        //public void Reseed(int seed)
        //{
        //    random = new Random(seed);
        //}
    }
}
