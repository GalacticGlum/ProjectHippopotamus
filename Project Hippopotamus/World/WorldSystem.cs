using System;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;
using Hippopotamus.Engine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;

namespace Hippopotamus.World
{
    public class WorldSystem : EntitySystem, IUpdatable
    {
        private readonly World world;
        private readonly Texture2D grassTexture;

        public WorldSystem()
        {
            Camera.Main.OrthographicSize = 0.15f;
            Camera.Main.Transform.Position = new Vector2(3042.896f, 1200f);

            grassTexture = DependencyInjector.Kernel.Get<ContentManager>().Load<Texture2D>("Tiles/Grass");

            world = new World(40, 8);
            world.AddGenerator(new TerrainWorldGenerator());

            world.Generate();
            BuildEntities();
            world.Save("moo.data");
        }

        public void Update(object sender, GameLoopUpdateEventArgs args)
        {
            Vector2 cameraPosition = Camera.Main.Transform.Position;
            Vector2 tileAtCameraPosition = new Vector2(cameraPosition.X / Tile.Size, cameraPosition.Y / Tile.Size);
            Chunk chunk = world.GetChunkContaining((int) Math.Round(tileAtCameraPosition.X), (int) Math.Round(tileAtCameraPosition.Y));

            if (chunk == null || chunk.Loaded) return;
            world.Load("moo.data", chunk);
            BuildEntities();
        }

        private void BuildEntities()
        {
            Pool.Clear();

            //for (int chunkX = 0; chunkX < world.HorizontalChunks; chunkX++)
            //{
            //    for (int chunkY = 0; chunkY < world.VerticalChunks; chunkY++)
            //    {
            //        Entity chunkEntity = Pool.Create($"Chunk ({chunkX}, {chunkY})");
            //        chunkEntity.Transform.Position = new Vector2(chunkX * Chunk.Size * Tile.Size, chunkY * Chunk.Size * Tile.Size);
            //        chunkEntity.Transform.Size = new Vector2(5);

            //        chunkEntity.AddComponent(new Text($"({chunkX}, {chunkY})"));
            //    }
            //}

            for (int x = 0; x < world.Width; x++)
            {
                for (int y = 0; y < world.Height; y++)
                {
                    if (!world.GetChunkContaining(x, y).Loaded) continue;

                    Entity entity = Pool.Create($"Tile ({x}, {y})");
                    entity.Transform.Position = new Vector2(x * Tile.Size, y * Tile.Size);
                    SpriteRenderer spriteRenderer = (SpriteRenderer)entity.AddComponent(new SpriteRenderer(null));

                    Tile tileAt = world.GetTileAtWorldCoordinates(x, y);
                    if (tileAt.Type == TileType.Grass)
                    {
                        spriteRenderer.Texture = grassTexture;
                    }
                }
            }
        }
    }
}
