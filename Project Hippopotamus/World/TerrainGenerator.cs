using System;

namespace Hippopotamus.World
{
    public abstract class TerrainGenerator
    { 
        public abstract void Generate(WorldData worldData);
        protected Random Random { get; private set; }

        public TerrainGenerator()
        {
            Random = new Random();
        }

        public TerrainGenerator(int seed)
        {
            Random = new Random(seed);
        }

        public void Reseed()
        {
            Random = new Random();
        }

        public void Reseed(int seed)
        {
            Random = new Random(seed);
        }
    }
}
