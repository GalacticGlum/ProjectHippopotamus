using System;
using System.Collections;
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

        SpriteRenderer spriteRenderer = tilePrefab.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "Tiles";
        spriteRenderer.material = Resources.Load<Material>("Materials/SpriteDiffuse");

        tilePrefab.SetActive(false);

        //ObjectPool.Warm(tilePrefab, (uint)Mathf.Pow(32, 3));
        //WorldController.Instance.StartCoroutine(ObjectPool.WarmAsync(tilePrefab, (uint)Mathf.Pow(32, 3)));
       
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
        tileGameObject.GetComponent<SpriteRenderer>().sprite = SpriteManager.Get("Tile", GetSpriteNameForTile(args.Tile));
    }

    public string GetSpriteNameForTile(Tile tile)
    {
        string spriteName = tile.Type.Name + "_";
        int x = tile.WorldPosition.X;
        int y = tile.WorldPosition.Y;

        Tile tileAt = World.Current.GetTileAt(x, y + 1);
        if (tileAt != null && tileAt.Type != TileType.Empty && DoesLinkWithNeighbour(tile, tileAt))
        {
            spriteName += "N";
        }

        tileAt = World.Current.GetTileAt(x + 1, y);
        if (tileAt != null && tileAt.Type != TileType.Empty && DoesLinkWithNeighbour(tile, tileAt))
        {
            spriteName += "E";
        }

        tileAt = World.Current.GetTileAt(x, y - 1);
        if (tileAt != null && tileAt.Type != TileType.Empty && DoesLinkWithNeighbour(tile, tileAt))
        {
            spriteName += "S";
        }

        tileAt = World.Current.GetTileAt(x - 1, y);
        if (tileAt != null && tileAt.Type != TileType.Empty && DoesLinkWithNeighbour(tile, tileAt))
        {
            spriteName += "W";
        }

        return spriteName;
    }

    private static bool DoesLinkWithNeighbour(Tile tile, Tile neighbour)
    {
        switch (tile.Type.LinkType)
        {
            case LinkType.None:
                return false;
            case LinkType.All:
                return true;
            case LinkType.Same:
                return tile.Type == neighbour.Type;
        }

        return false;
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

