using Hippopotamus.Engine.Bridge;

namespace Hippopotamus.Engine
{
    [LuaExposeType]
    public static class Random
    {
        public static int Seed { get; }
        private static System.Random randomEngine;

        static Random()
        {
            randomEngine = new System.Random();
        }

        public static int Range(int min, int max)
        {
            return randomEngine.Next(min, max);
        }

        public static float Range(float min, float max)
        {
            return (float)randomEngine.NextDouble() * (max - min) + min;
        }

        public static double Range(double min, double max)
        {
            return randomEngine.NextDouble() * (max - min) + min;
        }

        public static float Value()
        {
            return (float)randomEngine.NextDouble();
        }

        public static void Reseed()
        {
            randomEngine = new System.Random();
        }

        public static void Reseed(int seed)
        {
            randomEngine = new System.Random(seed);
        }

        public static void Reseed(string seed)
        {
            randomEngine = new System.Random(seed.GetHashCode());
        }
    }
}
