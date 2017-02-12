using System;
using System.Collections.Generic;
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

    public delegate void DrawGameLoopEventHandler(object sender, GameLoopDrawEventArgs args);
    public class GameLoopDrawEventArgs : EventArgs
    {
        public SpriteBatch SpriteBatch { get; }
        public GameLoopDrawEventArgs(SpriteBatch spriteBatch)
        {
            SpriteBatch = spriteBatch;
        }
    }

    public sealed class GameLoop
    {
        private event UpdateGameLoopEventHandler UpdateGameLoop;
        internal void Update(GameLoopUpdateEventArgs args) { UpdateGameLoop?.Invoke(this, args); }

        private event DrawGameLoopEventHandler DrawGameLoop;
        internal void Draw(GameLoopDrawEventArgs args) { DrawGameLoop?.Invoke(this, args); }

        // These methods are to limit the raising of the game loop events to the engine.
        public void Register(UpdateGameLoopEventHandler eventHandler) { UpdateGameLoop += eventHandler; }
        public void Register(DrawGameLoopEventHandler eventHandler) { DrawGameLoop += eventHandler; }

        public void Unregister(UpdateGameLoopEventHandler eventHandler) { UpdateGameLoop -= eventHandler; }
        public void Unregister(DrawGameLoopEventHandler eventHandler) { DrawGameLoop -= eventHandler; }
    }
}
