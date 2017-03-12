using Hippopotamus.Engine.Core.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.Core
{
    public class GameEngine : Game
    {
        public static float FixedTimeStep { get; set; } = 1.0f / 120.0f;

        public GameTimer TimeStats { get; private set; }
        public float FramesPerSecond => TimeStats.FramesPerSecond;
        public float UpdatesPerSeconds => TimeStats.UpdatesPerSecond;

        private readonly GameInstance gameInstance;
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private float unprocessedTimeSteps;

        public GameEngine(GameInstance gameInstance)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.SynchronizeWithVerticalRetrace = true;
            IsFixedTimeStep = false;

            this.gameInstance = gameInstance;
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TimeStats = new GameTimer();

            GameLoop.Register(GameLoopType.Update, Input.Update);
            GameLoop.Register(GameLoopType.Update, gameInstance.Update);
            GameLoop.Register(GameLoopType.Draw, gameInstance.Draw);

            BindDependencies();
            base.Initialize();

            EntitySystemManager.Initialize();
            gameInstance.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            TimeStats.ProcessUpdateCalculation(deltaTime);

            GameLoop.Trigger(GameLoopType.Update, new GameLoopEventArgs(deltaTime, FixedTimeStep, spriteBatch));
            FixedUpdate(deltaTime);

            MessageSystem.Update();
        }

        private void FixedUpdate(float deltaTime)
        {
            unprocessedTimeSteps += deltaTime;
            while (unprocessedTimeSteps >= FixedTimeStep)
            {
                GameLoop.Trigger(GameLoopType.FixedUpdate, new GameLoopEventArgs(deltaTime, FixedTimeStep, spriteBatch));
                unprocessedTimeSteps -= FixedTimeStep;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            TimeStats.ProcessFrameCalculation((float)gameTime.ElapsedGameTime.TotalSeconds);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            GameLoop.Trigger(GameLoopType.Draw, new GameLoopEventArgs((float)gameTime.ElapsedGameTime.TotalSeconds, FixedTimeStep, spriteBatch));
        }

        private void BindDependencies()
        {
            DependencyInjector.Kernel.Bind<ContentManager>().ToConstant(Content);
            DependencyInjector.Kernel.Bind<SpriteBatch>().ToConstant(spriteBatch);
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
