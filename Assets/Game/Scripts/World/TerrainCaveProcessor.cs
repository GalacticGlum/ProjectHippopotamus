using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
            if (Random.value < caveChance)
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
            if (!(Random.value < caveContinuationChance)) continue;

            depth++;
            TerrainUtilities.GenerateFuzzyCircle(minimumCaveSize, maximumCaveSize, worldData, new Vector2i(x, y), TileType.Empty);
            //Logger.Log("TerrainCaveProcessor", string.Format("Generated cave at ({0}, {1}). Current depth: {2}", x * Chunk.Size, y * Chunk.Size, depth));

            float angle = Random.value * MathHelper.Tau;
            Vector2 direction = new Vector2((float)Math.Cos(angle), (float)-Math.Sin(angle));

            int step = Random.Range(minimumCaveSize, maximumCaveSize);
            x += (int)Math.Floor(direction.x * step);
            y += (int)Math.Floor(direction.y * step);
        }
        while (depth < maximumCaveDepth);
    }
}