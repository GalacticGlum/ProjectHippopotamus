using System.Collections.Generic;
using UnityUtilities.Generic;

public class TerrainCleanup : ITerrainProcessor
{
    public void Generate(WorldData worldData)
    {
        int width = worldData.Width;
        int height = worldData.Height;
        bool[,] mainLand = new bool[width, height];
        Stack<Tuple<int, int>> tileStack = new Stack<Tuple<int, int>>();

        // Flood fill from lower left corner tile, depends on the mainland as such being connected.
        if (worldData.GetTileTypeAt(0, height - 1) == TileType.Empty) return;

        tileStack.Push(Tuple.Create(0, height - 1));
        mainLand[0, height - 1] = true;
        while (tileStack.Count > 0)
        {
            Tuple<int, int> current = tileStack.Pop();

            // West
            if (current.Item1 > 0)
            {
                int newX = current.Item1 - 1;
                int newY = current.Item2;
                if (worldData.GetTileTypeAt(newX, newY) != TileType.Empty &&
                    !mainLand[newX, newY])
                {
                    tileStack.Push(Tuple.Create(newX, newY));
                    mainLand[newX, newY] = true;
                }
            }

            // North
            if (current.Item2 > 0)
            {
                int newX = current.Item1;
                int newY = current.Item2 - 1;
                if (worldData.GetTileTypeAt(newX, newY) != TileType.Empty &&
                    !mainLand[newX, newY])
                {
                    tileStack.Push(Tuple.Create(newX, newY));
                    mainLand[newX, newY] = true;
                }
            }

            // East
            if (current.Item1 < width - 2)
            {
                int newX = current.Item1 + 1;
                int newY = current.Item2;
                if (worldData.GetTileTypeAt(newX, newY) != TileType.Empty &&
                    !mainLand[newX, newY])
                {
                    tileStack.Push(Tuple.Create(newX, newY));
                    mainLand[newX, newY] = true;
                }
            }

            // South
            if (current.Item2 >= height - 2) continue;
            {
                int newX = current.Item1;
                int newY = current.Item2 + 1;
                if (worldData.GetTileTypeAt(newX, newY) == TileType.Empty || mainLand[newX, newY]) continue;
                tileStack.Push(Tuple.Create(newX, newY));
                mainLand[newX, newY] = true;
            }
        }

        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (!mainLand[x, y])
                {
                    worldData.SetTileTypeAt(x, y, TileType.Empty);
                }
            }
        }
    }
}

