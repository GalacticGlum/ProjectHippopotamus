using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Rendering;

using Hippopotamus.Components;
using Hippopotamus.Systems;

namespace Hippopotamus
{
    public class Game : GameInstance
    {
        private Entity player;

        public override void Initialize()
        {
            player = GameEngine.EntityPool.Create("player");
            player.Transform.Position = new Vector2(800 / 2.0f, 600 / 2.0f);
            player.Transform.Size = new Vector2(2);

            player.AddComponent<Player>();
            player.AddComponent<SpriteRenderer>(GameEngine.Content.Load<Texture2D>("Tiles/BlockA0"));
        }

        public override void Update(object sender, GameLoopUpdateEventArgs args)
        {
        }

        public override void Draw(object sender, GameLoopDrawEventArgs args)
        {
        }

        public override void Dispose()
        {
        }
    }
}
