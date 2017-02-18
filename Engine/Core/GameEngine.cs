using Hippopotamus.Engine.Core.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.Core
{
    public class GameEngine : Game
    {
        public static float FixedTimeStep { get; set; } = 1.0f / 120.0f;

        public EntityPool EntityPool { get; private set; }
        public GameLoop GameLoop { get; private set; }

        private readonly GameInstance gameInstance;
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private float unprocessedTimeSteps;

        public GameEngine(GameInstance gameInstance)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;

            this.gameInstance = gameInstance;
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            EntityPool = new EntityPool("GameEngine_EntityPool");

            GameLoop = new GameLoop();
            GameLoop.Register(Input.Update);
            GameLoop.Register(gameInstance.Update);
            GameLoop.Register(gameInstance.Draw);

            BindDependencies();
            base.Initialize();

            EntitySystemManager.Initialize();
            gameInstance.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            GameLoop.Update(new GameLoopUpdateEventArgs(deltaTime));
            FixedUpdate(deltaTime);

            MessageSystem.Update();
        }

        private void FixedUpdate(float deltaTime)
        {
            unprocessedTimeSteps += deltaTime;
            while (unprocessedTimeSteps >= FixedTimeStep)
            {
                GameLoop.FixedUpdate(new GameLoopFixedUpdateEventArgs(FixedTimeStep));
                unprocessedTimeSteps -= FixedTimeStep;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GameLoop.Draw(new GameLoopDrawEventArgs(spriteBatch));
        }

        private void BindDependencies()
        {
            DependencyInjector.Kernel.Bind<ContentManager>().ToConstant(Content);
            DependencyInjector.Kernel.Bind<SpriteBatch>().ToConstant(spriteBatch);
            DependencyInjector.Kernel.Bind<EntityPool>().ToConstant(EntityPool);
        }

        public static void Launch<TGameInstance>() where TGameInstance : GameInstance, new()
        {
            TGameInstance gameInstance = new TGameInstance();
            using (GameEngine gameEngine = new GameEngine(gameInstance))
            {
                gameInstance.GameEngine = gameEngine;
                DependencyInjector.Kernel.Bind<GameEngine>().ToConstant(gameEngine);
                gameEngine.Run();
            }
        }
    }
}
