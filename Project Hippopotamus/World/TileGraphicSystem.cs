using System;
using System.Collections.Generic;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;
using Hippopotamus.Engine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.World
{
    public class TileGraphicSystem : EntitySystem
    {
        private readonly Dictionary<Tile, Texture2D> tiles;
        private readonly TextureAtlas grassAtlas;

        public TileGraphicSystem()
        {
            grassAtlas = new TextureAtlas("Images/Tiles/GrassAtlas.xml");
            tiles = new Dictionary<Tile, Texture2D>();

            World.Current.ChunkLoaded += OnChunkLoaded;
            World.Current.ChunkUnloaded += OnChunkUnloaded;
            World.Current.TileChanged += OnTileChanged;
        }

        public override void Draw(GameLoopEventArgs args)
        {
            RenderSystem.BeginDraw(args.SpriteBatch);           

            foreach (KeyValuePair<Tile, Texture2D> pair in tiles)
            {
                if (pair.Key == null || pair.Value == null) continue;
                args.SpriteBatch.Draw(pair.Value, pair.Key.WorldPosition.ToVector2() * Tile.Size, null, Color.White, 0,
                    new Vector2(pair.Value.Width / 2.0f, pair.Value.Height / 2.0f), Vector2.One, SpriteEffects.None, 0);
            }

            RenderSystem.EndDraw(args.SpriteBatch);
        }

        private void OnChunkLoaded(object sender, ChunkEventArgs args)
        {
            for (int x = 0; x < Chunk.Size; x++)
            {
                for (int y = 0; y < Chunk.Size; y++)
                {
                    Tile tileAt = args.Chunk.GetTileAt(x, y);
                    if(tileAt == null) continue;

                    tiles.Add(tileAt, null);
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
                    if (tileAt == null || !tiles.ContainsKey(tileAt)) continue;

                    tiles.Remove(tileAt);
                }
            }
        }

        private void OnTileChanged(object sender, TileEventArgs args)
        {
            if (tiles.ContainsKey(args.Tile) == false) return;

            switch (args.Tile.Type)
            {
                case TileType.Empty:
                    tiles[args.Tile] = null;
                    break;
                case TileType.Grass:
                    tiles[args.Tile] = grassAtlas.Get(GetSpriteNameForTile(args.Tile));
                    break;
            }
        }

        public string GetSpriteNameForTile(Tile tile)
        {
            string spriteName = Enum.GetName(tile.Type.GetType(), tile.Type) + "_";
            int x = tile.WorldPosition.X;
            int y = tile.WorldPosition.Y;

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
