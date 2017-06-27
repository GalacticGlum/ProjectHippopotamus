using System;
using UnityEngine;

public delegate void ChunkLoadedEventHandler(object sender, ChunkEventArgs args);
public class ChunkEventArgs : EventArgs
{
    public readonly Chunk Chunk;
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
    public readonly Vector2 Position;
    public readonly Vector2 WorldPosition;

    private Tile[,] tiles;

    public Chunk(Vector2 position, Vector2 worldPosition)
    {
        Position = position;
        WorldPosition = worldPosition;

        Generate();
    }

    private void Generate()
    {
        tiles = new Tile[Size, Size];
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                Vector2 position = new Vector2(x, y);
                tiles[x, y] = new Tile(TileType.Empty, position, WorldPosition + position, this);
            }
        }
    }

    public Tile GetTileAt(int x, int y)
    {
        if (x < 0 || x >= Size || y < 0 || y >= Size) return null;
        return tiles != null ? tiles[x, y] : null;
    }

    public void SetTileAt(int x, int y, Tile value)
    {
        if (x < 0 || x >= Size || y < 0 || y >= Size) return;
        if (value == null) return;

        tiles[x, y] = value;
    }

    public void Load(WorldData worldData)
    {
        if (Loaded) return;

        Loaded = true;

        Generate();
        int startX = (int)Position.x * Size;
        int startY = (int)Position.y * Size;

        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                tiles[x, y].Type = worldData.Tiles[startX + x, startY + y];
            }
        }

        World.Current.OnChunkLoaded(new ChunkEventArgs(this));
    }

    public void Unload()
    {
        if (!Loaded) return;

        Loaded = false;
        World.Current.OnChunkUnloaded(new ChunkEventArgs(this));

        tiles = null;
    }
}

