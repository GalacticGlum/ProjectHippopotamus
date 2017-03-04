namespace Hippopotamus.World
{
    public class WorldData
    {
        public TileType[,] Tiles { get; set; }

        public int Width { get; }
        public int Height { get; }

        public WorldData(int width, int height)
        {
            Width = width;
            Height = height;

            Tiles = new TileType[width, height];
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    Tiles[x, y] = TileType.Empty;
                }
            }
        }
    }
}
