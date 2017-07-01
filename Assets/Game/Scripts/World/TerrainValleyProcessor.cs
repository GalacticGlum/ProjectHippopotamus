using UnityEngine;

// TODO: Abstract code, currently there is some code duplication.
public class TerrainValleyProcessor : ITerrainProcessor
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

    public void Generate(WorldData worldData)
    {
        GenerateValleys(worldData);
        GenerateCanyons(worldData);
    }

    private static void GenerateValleys(WorldData worldData)
    {
        int distanceFromLast = 0;
        for (int x = 0; x < worldData.Width; x++)
        {
            if (distanceFromLast > minimumValleyDistance && Random.value < valleyChance)
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

    private static void GenerateValley(int startX, WorldData worldData)
    {
        int steps = Random.Range(minimumValleySteps, maximumValleySteps);
        int currentX = startX;
        for (int i = 0; i < steps; i++)
        {
            int radius = Random.Range(minimumValleySize, maximumValleySize);

            int pivot = Random.Range(-radius, radius);
            int x = currentX + pivot;

            Vector2i spot = TerrainUtilities.FindUpperMostTile(x, worldData, type => type != TileType.Empty);
            TileType spotType = Random.value < valleyExtrusionChance ? TileType.Get("Quartz") : TileType.Empty;
            TerrainUtilities.GenerateFuzzyCircle(minimumValleySize, maximumValleySize, worldData, spot, spotType);

            currentX = x;
        }
    }

    public void GenerateCanyons(WorldData worldData)
    {
        int distanceFromLast = 0;
        for (int x = 0; x < worldData.Width; x++)
        {
            if (distanceFromLast > minimumCanyonDistance && Random.value < canyonChance)
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

    private static void GenerateCanyon(int startX, WorldData worldData)
    {
        int steps = Random.Range(minimumCanyonSteps, maximumCanyonSteps);
        int currentX = startX;
        for (int i = 0; i < steps; i++)
        {
            int radius = Random.Range(minimumCanyonSize, maximumCanyonSize);

            int pivot = Random.Range(-radius, radius);
            int x = currentX + pivot;

            Vector2i spot = TerrainUtilities.FindUpperMostTile(x, worldData, type => type != TileType.Empty);
            TerrainUtilities.GenerateFuzzyCircle(minimumCanyonSize, maximumCanyonSize, worldData, spot, TileType.Empty);

            currentX = x;
        }
    }
}

