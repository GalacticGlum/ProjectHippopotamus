using System;
using System.Collections.Generic;
using Hippopotamus.Engine;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;
using Hippopotamus.Engine.Rendering;
using Microsoft.Xna.Framework;

namespace Hippopotamus.World
{
    public class TileGraphicSystem : EntitySystem
    {
        private readonly Dictionary<Tile, Entity> tileEntities;
        private readonly TextureAtlas grassAtlas;
        private readonly string genericName;

        public TileGraphicSystem()
        {
            grassAtlas = new TextureAtlas("Tiles/GrassAtlas.xml");
            tileEntities = new Dictionary<Tile, Entity>();

            World.Current.ChunkLoaded += OnChunkLoaded;
            World.Current.ChunkUnloaded += OnChunkUnloaded;
            World.Current.TileChanged += OnTileChanged;

            genericName = "Tile";
        }

        private void OnChunkLoaded(object sender, ChunkEventArgs args)
        {
            for (int x = 0; x < Chunk.Size; x++)
            {
                for (int y = 0; y < Chunk.Size; y++)
                {
                    Tile tileAt = args.Chunk.GetTileAt(x, y);

                    Entity entity = EntityPool.Create(string.IsInterned(genericName) ?? genericName);

                    entity.Transform.Position = new Vector2((args.Chunk.Position.X * Chunk.Size + x) * Tile.Size, (args.Chunk.Position.Y * Chunk.Size + y) * Tile.Size);
                    entity.AddComponent<SpriteRenderer>();

                    tileEntities.Add(tileAt, entity);
                    OnTileChanged(this, new TileEventArgs(tileAt));
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
                    if (!tileEntities.ContainsKey(tileAt)) continue;

                    tileEntities[tileAt].Destroy();
                    tileEntities.Remove(tileAt);
                }
            }
        }

        private void OnTileChanged(object sender, TileEventArgs args)
        {
            if (tileEntities.ContainsKey(args.Tile) == false) return;

            Entity entity = tileEntities[args.Tile];
            if (entity == null) return;

            switch (args.Tile.Type)
            {
                case TileType.Empty:
                    entity.GetComponent<SpriteRenderer>().Texture = null;
                    break;
                case TileType.Grass:
                    entity.GetComponent<SpriteRenderer>().Texture = grassAtlas.Get(GetSpriteNameForTile(args.Tile));
                    break;
            }
        }

        public string GetSpriteNameForTile(Tile tile)
        {
            string spriteName = Enum.GetName(tile.Type.GetType(), tile.Type) + "_";
            int x = tile.Position.X;
            int y = tile.Position.Y;

            Tile tileAt = World.Current.GetTileAt(x, y - 1);
            if (tileAt != null && tileAt.Type == tile.Type)
            {
                spriteName += "N";
            }

            tileAt = World.Current.GetTileAt(x + 1, y);
            if (tileAt != null && tileAt.Type == tile.Type)
            {
                spriteName += "E";
            }

            tileAt = World.Current.GetTileAt(x, y + 1);
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
    }
}
