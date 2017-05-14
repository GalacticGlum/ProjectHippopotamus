using System;
using Hippopotamus.Engine;
using Random = Hippopotamus.Engine.Random;

namespace Hippopotamus.World
{
    public static class TerrainUtilities
    {
        /// <summary>
        /// Creates a circle with <paramref name="radius"/> around a given <paramref name="tile"/> with <paramref name="type"/>. 
        /// Use this to affect the <paramref name="world"/>.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="world"></param>
        /// <param name="tile"></param>
        /// <param name="type"></param>
        public static void StampCircle(int radius, World world, Tile tile, TileType type)
        {
            StampCircle(radius, world, tile.WorldPosition, type);
        }

        /// <summary>
        /// Creates a circle with <paramref name="radius"/> around a given <paramref name="tilePosition"/> with <paramref name="type"/>. 
        /// Use this to affect the <paramref name="world"/>.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="world"></param>
        /// <param name="tilePosition"></param>
        /// <param name="type"></param>
        public static void StampCircle(int radius, World world, Vector2i tilePosition, TileType type)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    double distance = Math.Sqrt(x * x + y * y);
                    if (!(distance <= radius)) continue;

                    world.GetTileAt(tilePosition.X + x, tilePosition.Y + y).Type = type;
                }
            }
        }

        /// <summary>
        /// Creates a circle with <paramref name="radius"/> around a given <paramref name="tilePosition"/> with <paramref name="type"/>. 
        /// Use this for terrain generation.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="worldData"></param>
        /// <param name="tilePosition"></param>
        /// <param name="type"></param>
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

        /// <summary>
        /// Creates a fuzzy circle with radius between <paramref name="minRadius"/> and <paramref name="maxRadius"/>
        /// around a given <paramref name="tile"/> with <paramref name="type"/>. 
        /// Use this to affect the <paramref name="world"/>.
        /// </summary>
        /// <param name="minRadius"></param>
        /// <param name="maxRadius"></param>
        /// <param name="world"></param>
        /// <param name="tile"></param>
        /// <param name="type"></param>
        public static void StampFuzzyCircle(float minRadius, float maxRadius, World world, Tile tile, TileType type)
        {
            StampFuzzyCircle(minRadius, maxRadius, world, tile.WorldPosition, type);
        }

        /// <summary>
        /// Creates a fuzzy circle with radius between <paramref name="minRadius"/> and <paramref name="maxRadius"/>
        /// around a given <paramref name="tilePosition"/> with <paramref name="type"/>. 
        /// Use this to affect the <paramref name="world"/>.
        /// </summary>
        /// <param name="minRadius"></param>
        /// <param name="maxRadius"></param>
        /// <param name="world"></param>
        /// <param name="tilePosition"></param>
        /// <param name="type"></param>
        public static void StampFuzzyCircle(float minRadius, float maxRadius, World world, Vector2i tilePosition, TileType type)
        {
            int range = (int)Math.Ceiling(maxRadius);
            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    double fuzzyRadius = Random.Range(minRadius, maxRadius);
                    double distance = Math.Sqrt(x * x + y * y);
                    if (!(distance <= fuzzyRadius)) continue;

                    Tile tileAt = world.GetTileAt(tilePosition.X + x, tilePosition.Y + y);
                    tileAt.Type = type;
                }
            }
        }


        /// <summary>
        /// Creates a fuzzy circle with radius between <paramref name="minRadius"/> and <paramref name="maxRadius"/>
        /// around a given <paramref name="tilePosition"/> with <paramref name="type"/>. 
        /// Use this for terrain generation.
        /// </summary>
        /// <param name="minRadius"></param>
        /// <param name="maxRadius"></param>
        /// <param name="worldData"></param>
        /// <param name="tilePosition"></param>
        /// <param name="type"></param>
        public static void GenerateFuzzyCircle(float minRadius, float maxRadius, WorldData worldData, Vector2i tilePosition, TileType type)
        {
            int range = (int)Math.Ceiling(maxRadius);
            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    double fuzzyRadius = Random.Range(minRadius, maxRadius);
                    double distance = Math.Sqrt(x * x + y * y);
                    if (!(distance <= fuzzyRadius)) continue;

                    worldData.SetTileTypeAt(tilePosition.X + x, tilePosition.Y + y, type);
                }
            }
        }

        /// <summary>
        /// Gets the position of the uppermost tile that matches the <paramref name="condition"/>.
        /// Use this for <paramref name="world"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="world"></param>
        /// <param name="condition"></param>
        /// <returns>The tile or null if none found.</returns>
        public static Tile FindUpperMostTile(int x, World world, Func<TileType, bool> condition)
        {
            for (int y = 0; y < world.HeightInTiles; y++)
            {
                Tile tileAt = world.GetTileAt(x, y);
                if (condition(tileAt.Type))
                {
                    return tileAt;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the position of the uppermost tile that matches the <paramref name="condition"/>.
        /// Use this for terrain generation.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="worldData"></param>
        /// <param name="condition"></param>
        /// <returns>The tile or (-1, -1) if none found.</returns>
        public static Vector2i FindUpperMostTile(int x, WorldData worldData, Func<TileType, bool> condition)
        {
            for (int y = 0; y < worldData.Height; y++)
            {
                if (condition(worldData.GetTileTypeAt(x, y)))
                {
                    return new Vector2i(x, y);
                }
            }

            return new Vector2i(-1);
        }
    }
}
