using Hippopotamus.Engine.Bridge;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Rendering;
using Hippopotamus.World;

namespace Hippopotamus
{
    public class Game : GameInstance
    {
        public override void Initialize()
        {
            Context.IsMouseVisible = true;
            EntityPool.Create("camera").AddComponent(new Camera(Context.GraphicsDevice.Viewport));
            EntitySystemManager.Register<WorldSystem>();

            Lua.Parse("Lua/Test.lua");
        }

        public override void Update(GameLoopEventArgs args)
        {
            Context.Window.Title = $"FPS: {Lua.Call("GetFPS").String} | UPS: {(int)Context.UpdatesPerSeconds}";
        }
    }
}
