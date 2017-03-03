using System;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.Core
{
    public delegate void UpdateGameLoopEventHandler(GameLoopUpdateEventArgs args);
    public class GameLoopUpdateEventArgs : EventArgs
    {
        public float DeltaTime { get; }
        public GameLoopUpdateEventArgs(float deltaTime)
        {
            DeltaTime = deltaTime;
        }
    }

    public delegate void FixedUpdateGameLoopEventHandler(GameLoopFixedUpdateEventArgs args);
    public class GameLoopFixedUpdateEventArgs : EventArgs
    {
        public float FixedDeltaTime { get; }
        public GameLoopFixedUpdateEventArgs(float fixedDeltaTime)
        {
            FixedDeltaTime = fixedDeltaTime;
        }
    }

    public delegate void DrawGameLoopEventHandler(GameLoopDrawEventArgs args);
    public class GameLoopDrawEventArgs : EventArgs
    {
        public SpriteBatch SpriteBatch { get; }
        public GameLoopDrawEventArgs(SpriteBatch spriteBatch)
        {
            SpriteBatch = spriteBatch;
        }
    }

    public static class GameLoop
    {
        private static event UpdateGameLoopEventHandler UpdateGameLoop;
        internal static void Update(GameLoopUpdateEventArgs args) { UpdateGameLoop?.Invoke(args); }

        private static event FixedUpdateGameLoopEventHandler FixedUpdateGameLoop;
        internal static void FixedUpdate(GameLoopFixedUpdateEventArgs args) { FixedUpdateGameLoop?.Invoke(args); }

        private static event DrawGameLoopEventHandler DrawGameLoop;
        internal static void Draw(GameLoopDrawEventArgs args) { DrawGameLoop?.Invoke(args); }

        // These methods allow the game engine to keep control over the raising of game loop events.
        public static void Register(UpdateGameLoopEventHandler eventHandler) { UpdateGameLoop += eventHandler; }
        public static void Register(FixedUpdateGameLoopEventHandler eventHandler) { FixedUpdateGameLoop += eventHandler; }
        public static void Register(DrawGameLoopEventHandler eventHandler) { DrawGameLoop += eventHandler; }
                
        public static void Unregister(UpdateGameLoopEventHandler eventHandler) { UpdateGameLoop -= eventHandler; }
        public static void Unregister(FixedUpdateGameLoopEventHandler eventHandler) { FixedUpdateGameLoop -= eventHandler; }
        public static void Unregister(DrawGameLoopEventHandler eventHandler) { DrawGameLoop -= eventHandler; }
    }
}
