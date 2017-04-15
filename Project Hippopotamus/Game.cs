using Hippopotamus.Engine.Core;
using Hippopotamus.World;

namespace Hippopotamus
{
    public class Game : GameInstance
    {
        private float timer;
        public override void Initialize()
        {
            Context.IsMouseVisible = true;
            EntitySystemManager.Register<WorldSystem>();
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
