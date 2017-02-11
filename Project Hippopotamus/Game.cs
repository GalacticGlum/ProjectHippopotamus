using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Rendering;

using Hippopotamus.Components;
using Hippopotamus.Systems;

namespace Hippopotamus
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private GameObject player;
        private GameObject platform;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            DependencyInjector.Kernel.Bind<ContentManager>().ToConstant(Content);
            DependencyInjector.Kernel.Bind<SpriteBatch>().ToConstant(spriteBatch);

            SystemManager.Add<RenderSystem>();
            SystemManager.Add<MovementSystem>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            player = new GameObject
            {
                Transform =
                {
                    Position = new Vector2(GraphicsDevice.Viewport.Width / 2.0f, GraphicsDevice.Viewport.Height / 2.0f),
                    Size = new Vector2(2)
                }
            };

            player.AddComponent<Player>();

            SpriteRenderer spriteRenderer = player.AddComponent<SpriteRenderer>(Content.Load<Texture2D>("Tiles/BlockA0"));
            spriteRenderer.Colour = new Color(Color.Red, 255 / 2);

            //player.AddComponent(new BoxRigidbody(new Vector2(spriteRenderer.Texture.Width, spriteRenderer.Texture.Height)));
            //player.AddComponent<PlayerController>();

            platform = new GameObject
            {
                Transform =
                {
                    Size = new Vector2(GraphicsDevice.Viewport.Width, 2)
                }
            };

            spriteRenderer = platform.AddComponent< SpriteRenderer>(Content.Load<Texture2D>("Tiles/BlockA1"));
            platform.Transform.Position = new Vector2(GraphicsDevice.Viewport.Width / 2.0f, GraphicsDevice.Viewport.Height - spriteRenderer.Texture.Height);

            //platform.AddComponent(new BoxRigidbody(new Vector2(spriteRenderer.Texture.Width, spriteRenderer.Texture.Height), BodyType.Kinematic));
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();
            //DependencyInjector.Kernel.Get<PhysicsWorld>().Update(gameTime);
            //GameObject.Root.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GameObjectManager.Update();
        }
    }
}
