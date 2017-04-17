using System;
using Hippopotamus.Engine;
using Hippopotamus.Engine.Core;

namespace Hippopotamus.World
{
    // TODO: Force prairie at start location.
    public class TerrainPrairieGenerator : IWorldGenerator
    {
        private const int minimumSize = 10;
        private const int maximumSize = 200;

        private const int minimumDistanceBetween = 200;
        private const float chance = 0.7f;

        private const int minimumOffset = 0;
        private const int maximumOffset = 3;

        private const int minimumOffsetSwitchRate = 2;
        private const int maximumOffsetSwitchRate = 8;

        private Random random = new Random();

        public void Generate(WorldData worldData)
        {
            int distanceFromLast = 0;
            for (int x = 0; x < worldData.Width; x++)
            {
                if (distanceFromLast > minimumDistanceBetween && random.NextDouble() < chance)
                {
                    distanceFromLast = 0;
                    GeneratePrairie(x, worldData);
                }
                else
                {
                    distanceFromLast++;
                }
            }
        }

        public void GeneratePrairie(int startX, WorldData worldData)
        {
            Logger.Log("Generator", $"Generating prairie at x: {startX}");

            int size = random.Next(minimumSize, maximumSize);
            int endX = startX + size;

            Vector2i spotPosition = TerrainUtilities.FindUpperMostTile(startX, worldData, type => type != TileType.Empty);
            for (int x = 0; x < size; x++)
            {
                int nx = x + endX;
                TileType tileAt = worldData.GetTileTypeAt(nx, spotPosition.Y);
                if (tileAt == TileType.Empty) continue;

                endX = nx;
                break;
            }

            if (worldData.GetTileTypeAt(endX, spotPosition.Y) == TileType.Empty) return;

            int offset = 0;
            int offsetSwitch = 0;
            int offsetRate = 0;

            for (int x = startX; x < endX; x++)
            {
                for (int y = spotPosition.Y; y < worldData.Height; y++)
                {
                    TileType tileAt = worldData.GetTileTypeAt(x, y + offset);
                    if (tileAt == TileType.Empty)
                    {
                        worldData.SetTileTypeAt(x, y + offset, TileType.Grass);
                    }
                    else
                    {
                        break;
                    }
                }

                if (offsetSwitch >= offsetRate)
                {
                    offsetSwitch = 0;
                    offsetRate = random.Next(minimumOffsetSwitchRate, maximumOffsetSwitchRate);
                    offset = random.Next(minimumOffset, maximumOffset);
                }
                else
                {
                    offsetSwitch++;
                }
            }      
        }

        public void Reseed()
        {
            random = new Random();
        }

        public void Reseed(int seed)
        {
            random = new Random(seed);
        }
    }
}
