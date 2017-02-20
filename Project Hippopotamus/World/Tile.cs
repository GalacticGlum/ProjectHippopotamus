using System.IO;

namespace Hippopotamus.World
{
    public class Tile
    {
        public const int Size = 32;

        public TileType Type { get; set; }

        public Tile(TileType type)
        {
            Type = type;
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write((byte)Type);
        }

        public void Load(BinaryReader reader)
        {
            Type = (TileType) reader.ReadByte();
        }
    }
}
