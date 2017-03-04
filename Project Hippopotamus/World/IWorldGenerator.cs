namespace Hippopotamus.World
{
    public interface IWorldGenerator
    {
        void Reseed();
        void Reseed(int seed);
        void Generate(WorldData worldData);
    }
}
