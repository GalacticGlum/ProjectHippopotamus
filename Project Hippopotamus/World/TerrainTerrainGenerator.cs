using System;
using Microsoft.Xna.Framework;

namespace Hippopotamus.World
{
    public class TerrainTerrainGenerator : ITerrainGenerator
    {
        private Random random;

        public TerrainTerrainGenerator()
        {
            random = new Random();
        }

        public void Generate(WorldData worldData)
        {
            double[] heightMap = new double[worldData.Width];
            double groundHeight = worldData.Height * 0.7f;
            SineCurveParameter[] curveParameters =
            {
              new SineCurveParameter(0.0, 0.2, 1.0, 5.0),     // main big terrain feature
              new SineCurveParameter(0.0, 0.05, 15.0, 30.0),  // medium scale randomization
              new SineCurveParameter(0.0, 0.05, 15.0, 30.0),  // medium scale randomization
              new SineCurveParameter(0.0, 0.03, 25.0, 50.0),  // small and frequent detail
              new SineCurveParameter(0.0, 0.02, 30.0, 200.0), // small and frequent detail
              new SineCurveParameter(0.0, 0.06, 0.1, 0.5),    // rare bumps
            };

            // frequency of the noise
            const double noiseChance = 0.05;
            const double noiseMinimumMagnitude = -2.0;
            const double noiseMaxMagnitude = 2.0;

            for (int x = 0; x < worldData.Width; x++)
            {
                heightMap[x] = groundHeight;
            }

            foreach (SineCurveParameter curveParameter in curveParameters)
            {
                double amplitude = worldData.Height * MathHelper.Lerp((float)curveParameter.MinimumAmplitude, (float)curveParameter.MaximumAmplitude, (float)random.NextDouble());
                double frequency = MathHelper.Lerp((float)curveParameter.MinimumFrequency, (float)curveParameter.MaximumFrequency, (float)random.NextDouble()) / 100.0;

                const double offset = 0.0;
                double phase = random.NextDouble() * worldData.Width;
                for (int x = 0; x < worldData.Width; x++)
                {
                    heightMap[x] += amplitude * Math.Sin(frequency * x - phase) + offset;
                }
            }

            // do noise!
            for (int x = 0; x < worldData.Width; x++)
            {
                if (random.NextDouble() < noiseChance)
                {
                    heightMap[x] += MathHelper.Lerp((float)noiseMinimumMagnitude, (float)noiseMaxMagnitude, (float)random.NextDouble());
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
