using System;
using System.Diagnostics;
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

            Stopwatch stopwatch = Stopwatch.StartNew();

            GameEngine.EntityPool.Create("camera").AddComponent(new Camera(GameEngine.GraphicsDevice.Viewport));
            EntitySystemManager.Register<WorldSystem>();

            stopwatch.Stop();
            Console.WriteLine($"{stopwatch.ElapsedMilliseconds / 1000.0}");
        }
    }
}
