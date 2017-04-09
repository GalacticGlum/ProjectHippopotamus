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
            bool[,] grid = new bool[size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (random.NextDouble() < 0.35)
                    {
                        grid[x, y] = true;
                    }
                }
            }

            //for (int i = 0; i < steps; i++)
            //{
            //    grid = SimulateCellularAutomata(grid);
            //}

            for (int offsetX = 0; offsetX < size; offsetX++)
            {
                for (int offsetY = 0; offsetY < size; offsetY++)
                {
                    int x = tile.Position.X + offsetX;
                    int y = tile.Position.Y + offsetY;

                    Tile tileAt = World.Current.GetTileAt(x, y);
                    if (tileAt == null) return;
                    tileAt.Type = grid[offsetX, offsetY] ? TileType.Grass : TileType.Empty;
                }
            }
        }

        private static bool[,] SimulateCellularAutomata(bool[,] grid)
        {
            bool[,] simulatedGrid = grid;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    int neighbourCount = CountNeighbours(grid, new Vector2i(x, y));
                    if (grid[x, y])
                    {
                        if (neighbourCount < deathCellThreshold)
                        {
                            simulatedGrid[x, y] = false;
                        }
                        else
                        {
                            simulatedGrid[x, y] = true;
                        }
                    }
                    else
                    {
                        if (neighbourCount > birthCellThreshold)
                        {
                            simulatedGrid[x, y] = true;
                        }
                        else
                        {
                            simulatedGrid[x, y] = false;
                        }
                    }
                }
            }

            return simulatedGrid;
        }

        private static int CountNeighbours(bool[,] grid, Vector2i position)
        {
            int count = 0;
            for (int offsetX = -1; offsetX < 2; offsetX++)
            {
                for (int offsetY = -1; offsetY < 2; offsetY++)
                {
                    int x = position.X + offsetX;
                    int y = position.Y + offsetY;

                    if (offsetX == 0 && offsetY == 0) continue;
                    if (x < 0 || x >= size || y < 0 || y >= size)
                    {
                        count += 1;
                    }
                    else if (grid[x, y])
                    {
                        count += 1;
                    }
                }
            }

            return count;
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
