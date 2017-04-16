using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.UI;
using Hippopotamus.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus
{
    public class Game : GameInstance
    {
        private float timer;
        private Texture2D blankTexture;

        public override void Initialize()
        {
            Context.IsMouseVisible = true;
            EntitySystemManager.Register<WorldSystem>();

            blankTexture = new Texture2D(Context.GraphicsDevice, 1, 1);
            Color[] data = new Color[32*32];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Color.Red;
            }
            
            blankTexture.SetData(data);
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
