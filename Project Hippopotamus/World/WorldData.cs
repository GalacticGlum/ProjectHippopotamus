using System.Collections.Generic;
using Hippopotamus.Engine;
using Hippopotamus.Engine.Bridge;
using MoonSharp.Interpreter;

namespace Hippopotamus.World
{
    // TODO: Make WorldData affect the world (even) after it has been loaded. 
    // This can be done by telling the world to reload the affected chunks (not the worst idea) or by telling the world that a specific tile has changed (*).
    [LuaExposeType]
    [MoonSharpUserData]
    public class WorldData
    {
        public TileType[,] Tiles { get; set; }

        public int Width { get; }
        public int Height { get; }

        public WorldData(int width, int height)
        {
            Width = width;
            Height = height;

            Tiles = new TileType[width, height];
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    Tiles[x, y] = TileType.Empty;
                }
            }
        }

        /// <summary>
        /// Get's the TileType at (<paramref name="x"/>, <paramref name="y"/>).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>The tile type at (<paramref name="x"/>, <paramref name="y"/>) or TileType.Empty if invalid coordinates. </returns>
        public TileType GetTileTypeAt(int x, int y)
        {
            if(x < 0 || x >= Width || y < 0 || y >= Height) return TileType.Empty;
            return Tiles[x, y];
        }

        /// <summary>
        /// Sets the TileType at (<paramref name="x"/>, <paramref name="y"/>).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="type"></param>
        public void SetTileTypeAt(int x, int y, TileType type)
        {
            if(x < 0 || x >= Width || y < 0 || y >= Height) return;
            Tiles[x, y] = type;
        }

        public void SetTileTypes(Dictionary<Vector2i, TileType> setTileTypes)
        {
            foreach (Vector2i tilePosition in setTileTypes.Keys)
            {
                SetTileTypeAt(tilePosition.X, tilePosition.Y, setTileTypes[tilePosition]);
            }
        }
    }
}
