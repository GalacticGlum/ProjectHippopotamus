using System;
using Hippopotamus.Engine;

namespace Hippopotamus.World
{
    // TODO: Abstract code, currently there is some code duplication.
    public class TerrainValleyGenerator : TerrainGenerator
    {
        private const int minimumCanyonSteps = 1;
        private const int maximumCanyonSteps = 5;

        private const int minimumCanyonSize = 8;
        private const int maximumCanyonSize = 10;

        private const int minimumCanyonDistance = 400;
        private const float canyonChance = 0.25f;

        private const int minimumValleySteps = 1;
        private const int maximumValleySteps = 2;

        private const int minimumValleySize = 4;
        private const int maximumValleySize = 5;

        private const int minimumValleyDistance = 200;
        private const float valleyChance = 0.4f;
        private const float valleyExtrusionChance = 0.2f;

        public override void Generate(WorldData worldData)
        {
            GenerateValleys(worldData);
            GenerateCanyons(worldData);
        }

        private void GenerateValleys(WorldData worldData)
        {
            int distanceFromLast = 0;
            for (int x = 0; x < worldData.Width; x++)
            {
                if (distanceFromLast > minimumValleyDistance && Random.NextDouble() < valleyChance)
                {
                    distanceFromLast = 0;
                    GenerateValley(x, worldData);
                }
                else
                {
                    distanceFromLast++;
                }
            }
        }

        private void GenerateValley(int startX, WorldData worldData)
        {
            int steps = Random.Next(minimumValleySteps, maximumValleySteps);
            int currentX = startX;
            for (int i = 0; i < steps; i++)
            {
                int radius = Random.Next(minimumValleySize, maximumValleySize);

                int pivot = Random.Next(-radius, radius);
                int x = currentX + pivot;

                Vector2i spot = TerrainUtilities.FindUpperMostTile(x, worldData, type => type != TileType.Empty);
                TileType spotType = Random.NextDouble() < valleyExtrusionChance ? TileType.Grass : TileType.Empty;
                TerrainUtilities.GenerateFuzzyCircle(minimumValleySize, maximumValleySize, worldData, spot, spotType);

                currentX = x;
            }
        }

        private void GenerateCanyons(WorldData worldData)
        {
            int distanceFromLast = 0;
            for (int x = 0; x < worldData.Width; x++)
            {
                if (distanceFromLast > minimumCanyonDistance && Random.NextDouble() < canyonChance)
                {
                    distanceFromLast = 0;
                    GenerateCanyon(x, worldData);
                }
                else
                {
                    distanceFromLast++;
                }
            }
        }

        private void GenerateCanyon(int startX, WorldData worldData)
        {
            int steps = Random.Next(minimumCanyonSteps, maximumCanyonSteps);
            int currentX = startX;
            for (int i = 0; i < steps; i++)
            {
                int radius = Random.Next(minimumCanyonSize, maximumCanyonSize);

                int pivot = Random.Next(-radius, radius);
                int x = currentX + pivot;

                Vector2i spot = TerrainUtilities.FindUpperMostTile(x, worldData, type => type != TileType.Empty);
                TerrainUtilities.GenerateFuzzyCircle(minimumCanyonSize, maximumCanyonSize, worldData, spot, TileType.Empty);

                currentX = x;
            }
        }
    }
}
