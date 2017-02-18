using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;

namespace Hippopotamus
{
    public class WorldSystem : EntitySystem
    {
        private World world;
        private readonly EntityPool entityPool;

        public WorldSystem()
        {
            world = new World(100, 100);
            entityPool = DependencyInjector.Kernel.Get<GameEngine>().EntityPool;

            Texture2D texture = DependencyInjector.Kernel.Get<ContentManager>().Load<Texture2D>("Tiles/Grass");

            for (int x = 0; x < world.Width; x++)
            {
                for (int y = 0; y < world.Height; y++)
                {
                    Entity entity = entityPool.Create($"Tile ({x}, {y})");
                    entity.Transform.Position = new Vector2(x * 32, y * 32);
                    SpriteRenderer spriteRenderer = (SpriteRenderer)entity.AddComponent(new SpriteRenderer(texture));
                }
            }
        }
    }
}
