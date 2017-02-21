using System;
using System.IO;
using Microsoft.Xna.Framework;

namespace Hippopotamus.World
{
    public delegate void ChunkLoadedEventHandler(object sender, ChunkEventArgs args);
    public class ChunkEventArgs : EventArgs
    {
        public Chunk Chunk { get; }
        public ChunkEventArgs(Chunk chunk)
        {
            Chunk = chunk;
        }
    }

    public class Chunk
    {
        public const int Size = 32;

        public Tile this[int x, int y]
        {
            get { return GetTileAt(x, y); }
            set { SetTileAt(x, y, value); }
        }

        public bool Loaded { get; set; }
        public Vector2 Position { get; }

        public event ChunkLoadedEventHandler ChunkLoaded;
        public void OnChunkLoaded(ChunkEventArgs args) { ChunkLoaded?.Invoke(this, args); }

        public event TileChangedEventHandler TileChanged;

        private readonly Tile[,] tiles;

        public Chunk(Vector2 position)
        {
            Position = position;

            tiles = new Tile[Size, Size];
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    tiles[x, y] = new Tile(TileType.Empty);
                    tiles[x, y].TileChanged += OnTileChanged;
                }
            }
        }

        private void OnTileChanged(object sender, TileEventArgs args)
        {
            TileChanged?.Invoke(this, args);
        }

        public Tile GetTileAt(int x, int y)
        {
            if (x < 0 || x >= Size || y < 0 || y >= Size) return null;
            return tiles[x, y];
        }

        public void SetTileAt(int x, int y, Tile value)
        {
            if (x < 0 || x >= Size || y < 0 || y >= Size) return;
            if(value == null) return;

            tiles[x, y] = value;
        }

        public void Save(BinaryWriter writer)
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    tiles[x, y].Save(writer);
                }
            }
        }

        public void Load(BinaryReader reader)
        {
            Loaded = true;
            OnChunkLoaded(new ChunkEventArgs(this));

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    tiles[x, y].Load(reader);
                }
            }
        }
    }
}
