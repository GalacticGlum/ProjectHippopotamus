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
        private Widget testWidget;
        private Texture2D blankTexture;

        public override void Initialize()
        {
            Context.IsMouseVisible = true;
            EntitySystemManager.Register<WorldSystem>();

            Widget root = new Widget(new Vector2(Context.Width, Context.Height), Anchor.TopLeft);
            testWidget = new Widget(new Vector2(500, 32), Anchor.Centre)
            {
                Parent = root
            };


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
            testWidget.Update();

            if (!(timer >= 1)) return;

            Context.Window.Title = $"FPS: {(int)Context.FramesPerSecond} | UPS: {(int)Context.UpdatesPerSeconds}";
            timer = 0;
        }

        public override void Draw(GameLoopEventArgs args)
        {
            testWidget.Draw(args.SpriteBatch, blankTexture);
        }
    }
}
