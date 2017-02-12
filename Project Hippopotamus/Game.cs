using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Rendering;

using Hippopotamus.Components;
using Microsoft.Xna.Framework.Content;
using Ninject;

namespace Hippopotamus
{
    public class Game : GameInstance
    {
        private Entity player;

        public override void Initialize()
        {
            player = DependencyInjector.Kernel.Get<EntityPool>().Create("player");
            player.Transform.Position = new Vector2(800 / 2.0f, 600 / 2.0f);
            player.Transform.Size = new Vector2(2);

            player.AddComponent<Player>();
            player.AddComponent<SpriteRenderer>(DependencyInjector.Kernel.Get<ContentManager>().Load<Texture2D>("Tiles/BlockA0"));
        }

        public override void Update(object sender, GameLoopUpdateEventArgs args)
        {
            if (Input.GetKeyUp(Keys.A))
            {
                player.Toggle();
            }
        }

        public override void Draw(object sender, GameLoopDrawEventArgs args)
        {
            EntitySystemManager.Get<RenderSystem>().Draw();
        }

        public override void Dispose()
        {
        }
    }
}
