using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Rendering;
using Hippopotamus.World;

namespace Hippopotamus
{
    public class Game : GameInstance
    {
        public override void Initialize()
        {
            GameEngine.IsMouseVisible = true;
            EntityPool.Create("camera").AddComponent(new Camera(GameEngine.GraphicsDevice.Viewport));
            EntitySystemManager.Register<WorldSystem>();
        }

        public override void Update(GameLoopEventArgs args)
        {
            GameEngine.Window.Title = $"FPS: {(int)GameEngine.FramesPerSecond} | UPS: {(int)GameEngine.UpdatesPerSeconds}";
        }
    }
}
