using Hippopotamus.Engine;
using Microsoft.Xna.Framework;

namespace Hippopotamus.World
{
    public class TerrainWorldProcessor : ITerrainProcessor
    {
        public void Generate(WorldData worldData)
        {
            double[] heightMap = new double[worldData.Width];
            double groundHeight = worldData.Height * 0.7f;
            SineCurveParameter[] curveParameters =
            {
              new SineCurveParameter(0.0, 0.3, 0.5, 1.5),     // main big terrain feature
              new SineCurveParameter(0.0, 0.01, 5.0, 12.0),   // medium scale randomization
              new SineCurveParameter(0.0, 0.01, 5.0, 12.0),  // medium scale randomization
            };

            // frequency of the noise
            const double noiseChance = 0.05;
            const double noiseMinimumMagnitude = -2.0;
            const double noiseMaxMagnitude = 1.0;

            for (int x = 0; x < worldData.Width; x++)
            {
                heightMap[x] = groundHeight;
            }

            foreach (SineCurveParameter curveParameter in curveParameters)
            {
                double amplitude = worldData.Height * MathHelper.Lerp((float)curveParameter.MinimumAmplitude, (float)curveParameter.MaximumAmplitude, Random.Value());
                double frequency = MathHelper.Lerp((float)curveParameter.MinimumFrequency, (float)curveParameter.MaximumFrequency, Random.Value()) / 100.0;

                const double offset = 0.0;
                double phase = Random.Value() * worldData.Width;
                for (int x = 0; x < worldData.Width; x++)
                {
                    heightMap[x] += amplitude * System.Math.Sin(frequency * x - phase) + offset;
                }
            }

            // do noise!
            for (int x = 0; x < worldData.Width; x++)
            {
                if (Random.Value() < noiseChance)
                {
                    heightMap[x] += MathHelper.Lerp((float)noiseMinimumMagnitude, (float)noiseMaxMagnitude, Random.Value());
                }
            }

            for (int x = 0; x < worldData.Width; x++)
            {
                for (int y = 0; y < worldData.Height; y++)
                {
                    if (worldData.Height - 1 - y <= heightMap[x])
                    {
                        worldData.Tiles[x, y] = TileType.Grass;
                    }
                }
            }
        }
    }
}
