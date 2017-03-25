using Hippopotamus.Engine.Bridge;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Rendering;
using Hippopotamus.World;

namespace Hippopotamus
{
    public class Game : GameInstance
    {
        private float timer;
        public override void Initialize()
        {
            Context.IsMouseVisible = true;
            EntityPool.Create("camera").AddComponent(new Camera(Context.GraphicsDevice.Viewport));
            EntitySystemManager.Register<WorldSystem>();

            Lua.Parse("Lua/Test.lua");

            Logger.Verbosity = LoggerVerbosity.Error;
        }

        public override void Update(GameLoopEventArgs args)
        {
            timer += args.DeltaTime;
            if (!(timer >= 1)) return;

            Context.Window.Title = $"FPS: {(int)Context.FramesPerSecond} | UPS: {(int)Context.UpdatesPerSeconds}";
            timer = 0;
        }
    }
}
