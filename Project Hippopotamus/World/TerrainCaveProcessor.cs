using System;
using Hippopotamus.Engine;
using Hippopotamus.Engine.Core;
using Microsoft.Xna.Framework;
using Random = Hippopotamus.Engine.Random;

namespace Hippopotamus.World
{
    public class TerrainCaveProcessor : ITerrainProcessor
    {
        private const int minimumCaveSize = 5;
        private const int maximumCaveSize = 6;

        private const int maximumCaveDepth = 15;
        private const float caveContinuationChance = .9f;
        private const float caveChance = 0.015f;

        public void Generate(WorldData worldData)
        {
            for (int x = 0; x < worldData.Width; x++)
            {
                if (Random.Value() < caveChance)
                {
                    GenerateCave(x, worldData);
                }
            }
        }

        public static void GenerateCave(int x, WorldData worldData)
        {
            int depth = 0;

            Vector2i surface = TerrainUtilities.FindUpperMostTile(x, worldData, type => type != TileType.Empty);
            int y = Random.Range(surface.Y, worldData.Height);

            do
            {
                if (!(Random.Value() < caveContinuationChance)) continue;

                depth++;
                TerrainUtilities.GenerateFuzzyCircle(minimumCaveSize, maximumCaveSize, worldData, new Vector2i(x, y), TileType.Empty);
                Logger.Log("TerrainCaveProcessor", $"Generated cave at ({x * Chunk.Size}, {y * Chunk.Size}). Current depth: {depth}");

                float angle = Random.Value() * MathHelper.TwoPi;
                Vector2 direction = new Vector2((float)Math.Cos(angle), (float)-Math.Sin(angle));

                int step = Random.Range(minimumCaveSize, maximumCaveSize);
                x += (int)Math.Floor(direction.X * step);
                y += (int)Math.Floor(direction.Y * step);
            }
            while (depth < maximumCaveDepth);
        }
    }
}
