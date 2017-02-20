using System;
using Microsoft.Xna.Framework;

namespace Hippopotamus.World
{
    public class TerrainWorldGenerator : IWorldGenerator
    {
        private Random random;

        public TerrainWorldGenerator()
        {
            random = new Random();
        }

        public void Generate(World world)
        {
            double[] heightMap = new double[world.Width];
            double groundHeight = world.Height * 0.7f;
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

            for (int x = 0; x < world.Width; x++)
            {
                heightMap[x] = groundHeight;
            }

            foreach (SineCurveParameter curveParameter in curveParameters)
            {
                double amplitude = world.Height * MathHelper.Lerp((float)curveParameter.MinimumAmplitude, (float)curveParameter.MaximumAmplitude, (float)random.NextDouble());
                double frequency = MathHelper.Lerp((float)curveParameter.MinimumFrequency, (float)curveParameter.MaximumFrequency, (float)random.NextDouble()) / world.Width;

                const double offset = 0.0;
                double phase = random.NextDouble() * world.Width;
                for (int x = 0; x < world.Width; x++)
                {
                    heightMap[x] += amplitude * Math.Sin(frequency * x - phase) + offset;
                }
            }

            // do noise!
            for (int x = 0; x < world.Width; x++)
            {
                if (random.NextDouble() < noiseChance)
                {
                    heightMap[x] += MathHelper.Lerp((float)noiseMinimumMagnitude, (float)noiseMaxMagnitude, (float)random.NextDouble());
                }
            }

            for (int x = 0; x < world.Width; x++)
            {
                for (int y = 0; y < world.Height; y++)
                {
                    if (world.Height - 1 - y <= heightMap[x])
                    {
                        world.GetTileAtWorldCoordinates(x, y).Type = TileType.Grass;
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
