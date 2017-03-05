using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;

namespace Hippopotamus.World
{
    public class WorldSystem : EntitySystem, IUpdatable
    {
        public World World { get; }

        public WorldSystem()
        {
            World = new World();
            World.Initialize(200, 4);

            World.AddGenerator(new TerrainWorldGenerator());
            EntitySystemManager.Register<TileGraphicSystem>();

            World.Generate();

            //Camera.Main.Transform.Position = new Vector2(0, 2000);
        }

        public void Update(GameLoopUpdateEventArgs args)
        {
            World.Update();
        }
    }
}
