using System;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;
using Hippopotamus.Engine.Rendering;

namespace Hippopotamus.World
{
    public class WorldSystem : EntitySystem, IUpdatable
    {
        public World World { get; }

        public WorldSystem()
        {
            Camera.Main.OrthographicSize = .1f;

            World = new World(200, 4);
            World.AddGenerator(new TerrainWorldGenerator());

            EntitySystemManager.Register<TileGraphicSystem>();
            World.Generate();
        }

        public void Update(object sender, GameLoopUpdateEventArgs args)
        {
            World.Update();
        }
    }
}
