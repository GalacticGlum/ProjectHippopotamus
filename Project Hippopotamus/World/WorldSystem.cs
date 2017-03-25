﻿using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;

namespace Hippopotamus.World
{
    public class WorldSystem : EntitySystem
    {
        public World World { get; }

        public WorldSystem()
        {
            World = new World();
            World.Initialize(200, 4);

            World.AddGenerator(new TerrainWorldGenerator());
            EntitySystemManager.Register<TileGraphicSystem>();

            World.Generate();
        }

        public override void Update(GameLoopEventArgs args)
        {
            World.Update();
        }
    }
}
