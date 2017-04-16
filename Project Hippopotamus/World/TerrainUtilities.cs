using System;
using Hippopotamus.Engine;

namespace Hippopotamus.World
{
    public static class TerrainUtilities
    {
        public static void StampCircle(int radius, World world, Tile tile, TileType type)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    double distance = Math.Sqrt(x * x + y * y);
                    if (!(distance <= radius)) continue;

                    world.GetTileAt(tile.Position.X + x, tile.Position.Y + y).Type = type;
                }
            }
        }

        public static void GenerateCircle(int radius, WorldData worldData, Vector2i tilePosition, TileType type)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    double distance = Math.Sqrt(x * x + y * y);
                    if (!(distance <= radius)) continue;

                    worldData.SetTileTypeAt(tilePosition.X + x, tilePosition.Y + y, type);
                }
            }
        }
    }
}
