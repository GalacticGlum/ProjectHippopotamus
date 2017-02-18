using System;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.Core
{
    public delegate void UpdateGameLoopEventHandler(object sender, GameLoopUpdateEventArgs args);
    public class GameLoopUpdateEventArgs : EventArgs
    {
        public float DeltaTime { get; }
        public GameLoopUpdateEventArgs(float deltaTime)
        {
            DeltaTime = deltaTime;
        }
    }

    public delegate void FixedUpdateGameLoopEventHandler(object sender, GameLoopFixedUpdateEventArgs args);
    public class GameLoopFixedUpdateEventArgs : EventArgs
    {
        public float FixedDeltaTime { get; }
        public GameLoopFixedUpdateEventArgs(float fixedDeltaTime)
        {
            FixedDeltaTime = fixedDeltaTime;
        }
    }

    public delegate void DrawGameLoopEventHandler(object sender, GameLoopDrawEventArgs args);
    public class GameLoopDrawEventArgs : EventArgs
    {
        public SpriteBatch SpriteBatch { get; }
        public GameLoopDrawEventArgs(SpriteBatch spriteBatch)
        {
            SpriteBatch = spriteBatch;
        }
    }

    public class GameLoop
    {
        private event UpdateGameLoopEventHandler UpdateGameLoop;
        internal void Update(GameLoopUpdateEventArgs args) { UpdateGameLoop?.Invoke(this, args); }

        private event FixedUpdateGameLoopEventHandler FixedUpdateGameLoop;
        internal void FixedUpdate(GameLoopFixedUpdateEventArgs args) { FixedUpdateGameLoop?.Invoke(this, args); }

        private event DrawGameLoopEventHandler DrawGameLoop;
        internal void Draw(GameLoopDrawEventArgs args) { DrawGameLoop?.Invoke(this, args); }

        // These methods allow the game engine to keep control over the raising of game loop events.
        public void Register(UpdateGameLoopEventHandler eventHandler) { UpdateGameLoop += eventHandler; }
        public void Register(FixedUpdateGameLoopEventHandler eventHandler) { FixedUpdateGameLoop += eventHandler; }
        public void Register(DrawGameLoopEventHandler eventHandler) { DrawGameLoop += eventHandler; }

        public void Unregister(UpdateGameLoopEventHandler eventHandler) { UpdateGameLoop -= eventHandler; }
        public void Unregister(FixedUpdateGameLoopEventHandler eventHandler) { FixedUpdateGameLoop -= eventHandler; }
        public void Unregister(DrawGameLoopEventHandler eventHandler) { DrawGameLoop -= eventHandler; }
    }
}
