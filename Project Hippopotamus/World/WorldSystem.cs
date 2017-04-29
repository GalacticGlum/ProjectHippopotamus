using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;
using Hippopotamus.Engine.Rendering;
using Microsoft.Xna.Framework;

namespace Hippopotamus.World
{
    public class WorldSystem : EntitySystem
    {
        public World World { get; }

        public WorldSystem()
        {
            World = new World();
            World.Initialize(200, 4);
            Camera.Main.Transform.Position = new Vector2((World.Width - 1) / 2 * Chunk.Size * Tile.Size, (World.Height - 1) / 2 * Chunk.Size * Tile.Size);
            //Camera.Main.Transform.Position = new Vector2(0, 0);
            World.AddGenerator(new TerrainWorldProcessor());
            World.AddGenerator(new TerrainValleyProcessor());
            World.AddGenerator(new TerrainPrairieProcessor());
            World.AddGenerator(new TerrainCleanup());

            EntitySystemManager.Register<TileGraphicSystem>();

            // Constant seed for debugging purposes.
            World.Generate("purplehippo");

        }

        public override void Update(GameLoopEventArgs args)
        {
            World.Update();
        }
    }
}
