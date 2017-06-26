using System;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities.ObjectPool;

public class TileGraphicController
{
    private readonly Dictionary<Tile, GameObject> tileGameObjectMap;
    private readonly GameObject tilePrefab;
    private readonly GameObject tileParent;

    public TileGraphicController()
    {
        tileGameObjectMap = new Dictionary<Tile, GameObject>();

        tileParent = new GameObject("Tiles");
        tilePrefab = new GameObject("Tile_Prefab", typeof(SpriteRenderer), typeof(BoxCollider2D));
        tilePrefab.GetComponent<BoxCollider2D>().size = Vector2.one;
        tilePrefab.SetActive(false);

        WorldController.Instance.StartCoroutine(ObjectPool.WarmAsync(tilePrefab, (uint)Mathf.Pow(32, 3)));
       
        World.Current.ChunkLoaded += OnChunkLoaded;
        World.Current.ChunkUnloaded += OnChunkUnloaded;
        World.Current.TileChanged += OnTileChanged;
    }

    private void OnChunkLoaded(object sender, ChunkEventArgs args)
    {
        for (int x = 0; x < Chunk.Size; x++)
        {
            for (int y = 0; y < Chunk.Size; y++)
            {
                Tile tileAt = args.Chunk.GetTileAt(x, y);
                if (tileAt == null) continue;

                CreateTileGameObject(tileAt);
            }
        }
    }

    private void OnChunkUnloaded(object sender, ChunkEventArgs args)
    {
        for (int x = 0; x < Chunk.Size; x++)
        {
            for (int y = 0; y < Chunk.Size; y++)
            {
                Tile tileAt = args.Chunk.GetTileAt(x, y);
                if (tileAt == null || !tileGameObjectMap.ContainsKey(tileAt)) continue;

                ObjectPool.Destroy(tileGameObjectMap[tileAt]);
                tileGameObjectMap.Remove(tileAt);
            }
        }
    }

    private void OnTileChanged(object sender, TileEventArgs args)
    {
        if (tileGameObjectMap.ContainsKey(args.Tile) == false) return;

        GameObject tileGameObject = tileGameObjectMap[args.Tile];
        if (tileGameObject == null)
        {
            return;
        }

        if (args.Tile.Type == TileType.Empty)
        {
            tileGameObject.SetActive(false);
            return;
        }

        tileGameObject.SetActive(true);
        tileGameObject.GetComponent<SpriteRenderer>().sprite = SpriteManager.GetSprite("Tile", GetSpriteNameForTile(args.Tile));
    }

    public string GetSpriteNameForTile(Tile tile)
    {
        string spriteName = Enum.GetName(tile.Type.GetType(), tile.Type) + "_";
        int x = tile.WorldPosition.X;
        int y = tile.WorldPosition.Y;

        Tile tileAt = World.Current.GetTileAt(x, y + 1);
        if (tileAt != null && tileAt.Type == tile.Type)
        {
            spriteName += "N";
        }

        tileAt = World.Current.GetTileAt(x + 1, y);
        if (tileAt != null && tileAt.Type == tile.Type)
        {
            spriteName += "E";
        }

        tileAt = World.Current.GetTileAt(x, y - 1);
        if (tileAt != null && tileAt.Type == tile.Type)
        {
            spriteName += "S";
        }

        tileAt = World.Current.GetTileAt(x - 1, y);
        if (tileAt != null && tileAt.Type == tile.Type)
        {
            spriteName += "W";
        }

        return spriteName;
    }

    private void CreateTileGameObject(Tile tileAt)
    {
        GameObject tileGameObject = ObjectPool.Spawn(tilePrefab, tileAt.WorldPosition.ToVector3(), Quaternion.identity);
        tileGameObject.name = "Tile_" + tileAt.WorldPosition.X + "_" + tileAt.WorldPosition.Y;
        tileGameObjectMap.Add(tileAt, tileGameObject);
        tileGameObject.transform.SetParent(tileParent.transform);
        tileGameObject.GetComponent<SpriteRenderer>().sprite = null;

        OnTileChanged(this, new TileEventArgs(tileAt));
    }
}

