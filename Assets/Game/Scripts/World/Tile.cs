using System;
using System.IO;
using UnityEngine;

public delegate void TileChangedEventHandler(object sender, TileEventArgs args);
public class TileEventArgs : EventArgs
{
    public readonly Tile Tile;
    public TileEventArgs(Tile tile)
    {
        Tile = tile;
    }
}

public class Tile
{
    /// <summary>
    /// Position in world space.
    /// </summary>
    public Vector2i WorldPosition { get; private set; }
    /// <summary>
    /// Position inside the chunk.
    /// </summary>
    public Vector2i Position { get; private set; }
    public Chunk Chunk { get; private set; }

    private TileType type = TileType.Empty;
    public TileType Type
    {
        get { return type; }
        set
        {
            TileType oldTileType = type;
            type = value;

            if (oldTileType == type) return;
            World.Current.OnTileChanged(new TileEventArgs(this));
            UpdateNeighbouring();
            World.Current.WorldData.SetTileTypeAt(WorldPosition.X, WorldPosition.Y, Type);
        }
    }

    public Tile(TileType type, Vector2 position, Vector2 worldPosition, Chunk chunk)
    {
        Type = type;
        Position = new Vector2i((int)position.x, (int)position.y);
        WorldPosition = new Vector2i((int)worldPosition.x, (int)worldPosition.y);
        Chunk = chunk;
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Type.Id);
    }

    public void Load(BinaryReader reader)
    {
        Type = TileType.Get(reader.ReadByte());
    }

    private void UpdateNeighbouring()
    {
        foreach (Tile neighbour in GetNeighbours())
        {
            if (neighbour != null)
            {
                World.Current.OnTileChanged(new TileEventArgs(neighbour));
            }
        }
    }

    public Tile[] GetNeighbours(bool diagonal = false)
    {
        Tile[] neighbours = !diagonal ? new Tile[4] : new Tile[8];
        Tile tileAt = World.Current.GetTileAt(WorldPosition.X, WorldPosition.Y + 1);
        neighbours[0] = tileAt;
        tileAt = World.Current.GetTileAt(WorldPosition.X + 1, WorldPosition.Y);
        neighbours[1] = tileAt;
        tileAt = World.Current.GetTileAt(WorldPosition.X, WorldPosition.Y - 1);
        neighbours[2] = tileAt;
        tileAt = World.Current.GetTileAt(WorldPosition.X - 1, WorldPosition.Y);
        neighbours[3] = tileAt;

        if (!diagonal) return neighbours;

        tileAt = World.Current.GetTileAt(WorldPosition.X + 1, WorldPosition.Y + 1);
        neighbours[4] = tileAt;
        tileAt = World.Current.GetTileAt(WorldPosition.X + 1, WorldPosition.Y - 1);
        neighbours[5] = tileAt;
        tileAt = World.Current.GetTileAt(WorldPosition.X - 1, WorldPosition.Y - 1);
        neighbours[6] = tileAt;
        tileAt = World.Current.GetTileAt(WorldPosition.X - 1, WorldPosition.Y + 1);
        neighbours[7] = tileAt;

        return neighbours;
    }

    /// <summary>
    /// INTERNAL_METHOD
    /// This sets the Tile type without updating the WorldData.
    /// </summary>
    /// <param name="value"></param>
    public void WorldDataSetType(TileType value)
    {
        TileType oldTileType = type;
        type = value;

        if (oldTileType == type) return;
        World.Current.OnTileChanged(new TileEventArgs(this));
        UpdateNeighbouring();
    }
}

