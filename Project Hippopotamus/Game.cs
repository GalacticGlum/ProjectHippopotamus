using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Rendering;
using Hippopotamus.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus
{
    public class Game : GameInstance
    {
        private float timer;
        public override void Initialize()
        {
            Logger.Verbosity = LoggerVerbosity.None;
            Context.IsMouseVisible = true;

            EntitySystemManager.Register<WorldSystem>();
            Entity test = EntityPool.Create("Test");
            test.Transform.Position = new Vector2(101376, 1024);
            test.Transform.Size = new Vector2(50);

            SpriteRenderer spriteRenderer = test.AddComponent<SpriteRenderer>();
            spriteRenderer.Texture = Context.Content.Load<Texture2D>("Tiles/Grass");
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
