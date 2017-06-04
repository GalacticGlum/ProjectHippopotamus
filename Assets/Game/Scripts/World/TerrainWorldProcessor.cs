using UnityEngine;

public class TerrainWorldProcessor : ITerrainProcessor
{
    public void Generate(WorldData worldData)
    {
        double[] heightMap = new double[worldData.Width];
        double groundHeight = worldData.Height * 0.7f;
        SineCurveParameter[] curveParameters =
        {
              new SineCurveParameter(0.0, 0.3, 0.5, 1.5),     // Main big terrain feature
              new SineCurveParameter(0.0, 0.01, 5.0, 12.0),   // Medium scale randomization
              new SineCurveParameter(0.0, 0.01, 5.0, 12.0),  // Medium scale randomization
        };

        // Frequency of the noise
        const double noiseChance = 0.05;
        const double noiseMinimumMagnitude = -2.0;
        const double noiseMaxMagnitude = 1.0;

        for (int x = 0; x < worldData.Width; x++)
        {
            heightMap[x] = groundHeight;
        }

        foreach (SineCurveParameter curveParameter in curveParameters)
        {
            double amplitude = worldData.Height * Mathf.Lerp((float)curveParameter.MinimumAmplitude, (float)curveParameter.MaximumAmplitude, Random.value);
            double frequency = Mathf.Lerp((float)curveParameter.MinimumFrequency, (float)curveParameter.MaximumFrequency, Random.value) / 100.0;

            const double offset = 0.0;
            double phase = Random.value * worldData.Width;
            for (int x = 0; x < worldData.Width; x++)
            {
                heightMap[x] += amplitude * System.Math.Sin(frequency * x - phase) + offset;
            }
        }

        for (int x = 0; x < worldData.Width; x++)
        {
            if (Random.value < noiseChance)
            {
                heightMap[x] += Mathf.Lerp((float)noiseMinimumMagnitude, (float)noiseMaxMagnitude, Random.value);
            }
        }

        for (int x = 0; x < worldData.Width; x++)
        {
            for (int y = 0; y < worldData.Height; y++)
            {
                if (y <= heightMap[x])
                {
                    worldData.Tiles[x, y] = TileType.Grass;
                }
            }
        }
    }

}
