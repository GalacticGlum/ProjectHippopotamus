using System;
using Hippopotamus.Engine;

namespace Hippopotamus.World
{
    // TODO: Abstract/genericize code, currently there is alot of code duplication.
    public class TerrainValleyGenerator : ITerrainGenerator
    {
        private const int minimumCanyonSteps = 1;
        private const int maximumCanyonSteps = 5;

        private const int minimumCanyonSize = 5;
        private const int maximumCanyonSize = 15;

        private const int minimumCanyonDistance = 400;
        private const float canyonChance = 0.25f;

        private const int minimumValleySteps = 1;
        private const int maximumValleySteps = 2;

        private const int minimumValleySize = 2;
        private const int maximumValleySize = 6;

        private const int minimumValleyDistance = 200;
        private const float valleyChance = 0.4f;
        private const float valleyExtrusionChance = 0.2f;

        private Random random;

        public TerrainValleyGenerator()
        {
            random = new Random();
        }

        public void Generate(WorldData worldData)
        {
            GenerateValleys(worldData);
            GenerateCanyons(worldData);
        }

        private void GenerateValleys(WorldData worldData)
        {
            int distanceFromLast = 0;
            for (int x = 0; x < worldData.Width; x++)
            {
                if (distanceFromLast > minimumValleyDistance && random.NextDouble() < valleyChance)
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
            int steps = random.Next(minimumValleySteps, maximumValleySteps);
            int currentX = startX;
            for (int i = 0; i < steps; i++)
            {
                int radius = random.Next(minimumValleySize, maximumValleySize);

                int pivot = random.Next(-radius, radius);
                int x = currentX + pivot;

                Vector2i spot = TerrainUtilities.FindUpperMostTile(x, worldData, type => type != TileType.Empty);
                TileType spotType = random.NextDouble() < valleyExtrusionChance ? TileType.Grass : TileType.Empty;
                TerrainUtilities.GenerateCircle(radius, worldData, spot, spotType);

                currentX = x;
            }
        }

        private void GenerateCanyons(WorldData worldData)
        {
            int distanceFromLast = 0;
            for (int x = 0; x < worldData.Width; x++)
            {
                if (distanceFromLast > minimumCanyonDistance && random.NextDouble() < canyonChance)
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
            int steps = random.Next(minimumCanyonSteps, maximumCanyonSteps);
            int currentX = startX;
            for (int i = 0; i < steps; i++)
            {
                int radius = random.Next(minimumCanyonSize, maximumCanyonSize);

                int pivot = random.Next(-radius, radius);
                int x = currentX + pivot;

                Vector2i spot = TerrainUtilities.FindUpperMostTile(x, worldData, type => type != TileType.Empty);
                TerrainUtilities.GenerateCircle(radius, worldData, spot, TileType.Empty);

                currentX = x;
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
