using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hippopotamus.Engine;

namespace Hippopotamus.World
{
    public static class TerrainValleyGenerator //: IWorldGenerator
    {
        private const int minimumValleySteps = 1;
        private const int maximumValleySteps = 5;

        private static readonly Random random;

        static TerrainValleyGenerator()
        {
            random = new Random();
        }

        public static void Generate()
        {
            
        }

        private static void GenerateValley()
        {
            int x = random.Next(0, World.Current.WidthInTiles);
            Tile spotTile = null;

            for (int y = 0; y < World.Current.HeightInTiles; y++)
            {
                if (spotTile != null) continue;

                Tile tileAt = World.Current.GetTileAt(x, y);
                if (tileAt.Type != TileType.Empty)
                {
                    spotTile = tileAt;
                }
            }

            if (spotTile == null) return;
            TerrainUtilities.StampCircle(random.Next(5, 10), World.Current, spotTile, TileType.Empty);
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
