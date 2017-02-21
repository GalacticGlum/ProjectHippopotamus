using System;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;
using Hippopotamus.Engine.Rendering;
using Microsoft.Xna.Framework;

namespace Hippopotamus.World
{
    public class WorldSystem : EntitySystem, IUpdatable
    {
        public World World { get; }

        public WorldSystem()
        {
            Camera.Main.OrthographicSize = .1f;

            World = new World(200, 8);
            World.AddGenerator(new TerrainWorldGenerator());

            EntitySystemManager.Register<TileGraphicSystem>();

            World.Generate();

        }

        public void Update(object sender, GameLoopUpdateEventArgs args)
        {
            Vector2 cameraPosition = Camera.Main.Transform.Position;
            Vector2 tileAtCameraPosition = new Vector2(cameraPosition.X / Tile.Size, cameraPosition.Y / Tile.Size);
            Chunk chunk = World.GetChunkContaining((int) Math.Round(tileAtCameraPosition.X), (int) Math.Round(tileAtCameraPosition.Y));

            if (chunk != null)
            {
                World.Load("moo.data", chunk);
            }
        }
    }
}
