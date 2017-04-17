namespace Hippopotamus.World
{
    public interface ITerrainGenerator
    {
        void Reseed();
        void Reseed(int seed);
        void Generate(WorldData worldData);
    }
}
