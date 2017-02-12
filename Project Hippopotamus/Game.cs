using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Rendering;

using Hippopotamus.Components;

namespace Hippopotamus
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private EntityPool pool;
        private Entity player;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pool = new EntityPool("Entity Pool");

            DependencyInjector.Kernel.Bind<ContentManager>().ToConstant(Content);
            DependencyInjector.Kernel.Bind<SpriteBatch>().ToConstant(spriteBatch);
            DependencyInjector.Kernel.Bind<EntityPool>().ToConstant(pool);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            player = pool.Create("player");
            player.Transform.Position = new Vector2(GraphicsDevice.Viewport.Width / 2.0f, GraphicsDevice.Viewport.Height / 2.0f);
            player.Transform.Size = new Vector2(2);

            player.AddComponent<Player>();
            player.AddComponent<SpriteRenderer>(Content.Load<Texture2D>("Tiles/BlockA0"));
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();
            if (Input.GetKeyUp(Keys.A))
            {
                player.Disable();
            }

            //DependencyInjector.Kernel.Get<PhysicsWorld>().Update(gameTime);
            //GameObject.Root.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            EntitySystemManager.Get<RenderSystem>().Draw();
        }
    }
}
