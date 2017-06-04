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
    public Vector2i WorldPosition { get; private set; }
    public Vector2i Position { get; private set; }
    public Chunk Chunk { get; private set; }

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
            UpdateNeighbouring();
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
        writer.Write((byte)Type);
    }

    public void Load(BinaryReader reader)
    {
        Type = (TileType)reader.ReadByte();
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
}

