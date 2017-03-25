using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;
using Microsoft.Xna.Framework.Input;

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

        public override void Start()
        {
            Logger.Log("WorldSystem::Start: Just telling you that something has started up! It's not really that important. ");
        }

        public override void Update(GameLoopEventArgs args)
        {
            if (Input.GetKeyDown(Keys.Tab))
            {
                Logger.Log("WorldSystem::Update: Oh no! Something has gone very terribly wrong, we do care about this!", LogMessageVerbosity.Error);
            }
            else if(Input.GetKeyDown(Keys.X))
            {
                Logger.Log("WorldSystem::Update: Oh no! Something has gone wrong, we do care about this, but it's not fatal.", LogMessageVerbosity.Warning);
            }

            Logger.Log("WorldSystem::Update: We are flooding the logger with non-important information, how do we catch errors with this spam???");

            World.Update();
        }
    }
}
