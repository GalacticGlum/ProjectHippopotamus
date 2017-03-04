using System;
using System.IO;

namespace Hippopotamus.World
{
    public delegate void TileChangedEventHandler(object sender, TileEventArgs args);
    public class TileEventArgs : EventArgs
    {
        public Tile Tile { get; }
        public TileEventArgs(Tile tile)
        {
            Tile = tile;
        }
    }

    public class Tile
    {
        public const int Size = 32;

        private TileType type;
        public TileType Type
        {
            get { return type; }
            set
            {
                TileType oldTileType = type;
                type = value;

                if (oldTileType == type) return;
                World.Current.OnTileChanged(new TileEventArgs(this));
            }
        }

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
