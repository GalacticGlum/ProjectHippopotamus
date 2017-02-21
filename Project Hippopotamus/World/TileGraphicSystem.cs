using System.Collections.Generic;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;
using Hippopotamus.Engine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;

namespace Hippopotamus.World
{
    // TODO: Only ACTUALLY create entities for CHUNKS that are currenty loaded (and destroy entities for unloaded chunks)
    // This is a huge optimization!
    public class TileGraphicSystem : EntitySystem
    {
        private readonly Dictionary<Tile, Entity> tileEntities;
        private readonly Texture2D grassTexture;

        public TileGraphicSystem()
        {
            grassTexture = DependencyInjector.Kernel.Get<ContentManager>().Load<Texture2D>("Tiles/Grass");
            tileEntities = new Dictionary<Tile, Entity>();

            World.Current.ChunkLoaded += OnChunkLoaded;
            World.Current.TileChanged += OnTileChanged;
        }

        private void OnChunkLoaded(object sender, ChunkEventArgs args)
        {
            for (int x = 0; x < Chunk.Size; x++)
            {
                for (int y = 0; y < Chunk.Size; y++)
                {
                    Tile tileAt = args.Chunk.GetTileAt(x, y);

                    Entity entity = Pool.Create($"Chunk ({args.Chunk.Position.X}, {args.Chunk.Position.Y}): Tile ({x}, {y})");
                    entity.Transform.Position = new Vector2(args.Chunk.Position.X * Chunk.Size + x * Tile.Size, args.Chunk.Position.Y * Chunk.Size + y * Tile.Size);
                    SpriteRenderer spriteRenderer = (SpriteRenderer)entity.AddComponent(new SpriteRenderer(null));

                    tileEntities.Add(tileAt, entity);
                    OnTileChanged(this, new TileEventArgs(tileAt));
                }
            }
        }

        private void OnTileChanged(object sender, TileEventArgs args)
        {
            if (tileEntities.ContainsKey(args.Tile) == false) return;

            Entity entity = tileEntities[args.Tile];
            if(entity == null) return;

            switch (args.Tile.Type)
            {
                case TileType.Empty:
                    entity.GetComponent<SpriteRenderer>().Texture = grassTexture;
                    break;
                case TileType.Grass:
                    entity.GetComponent<SpriteRenderer>().Texture = grassTexture;
                    break;
            }
        }
    }
}
