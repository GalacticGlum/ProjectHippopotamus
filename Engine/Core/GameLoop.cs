using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.Core
{
    public delegate void GameLoopEventHandler(GameLoopEventArgs args);
    public class GameLoopEventArgs : EventArgs
    {
        public float DeltaTime { get; }
        public float FixedDeltaTime { get; }
        public SpriteBatch SpriteBatch { get; }

        public GameLoopEventArgs(float deltaTime, float fixedDeltaTime, SpriteBatch spriteBatch)
        {
            DeltaTime = deltaTime;
            FixedDeltaTime = fixedDeltaTime;
            SpriteBatch = spriteBatch;
        }
    }

    public static class GameLoop
    {
        private static readonly Dictionary<GameLoopType, GameLoopEventHandler> processors;

        static GameLoop()
        {
            processors = new Dictionary<GameLoopType, GameLoopEventHandler>();
        }

        public static void Trigger(GameLoopType type, GameLoopEventArgs args)
        {
            if (processors.ContainsKey(type))
            {
                processors[type](args);
            }
        }

        public static void Register(GameLoopType type, GameLoopEventHandler handler)
        {
            if (!processors.ContainsKey(type))
            {
                processors.Add(type, null);
            }

            processors[type] += handler;
        }

        public static void Unregister(GameLoopType type, GameLoopEventHandler handler)
        {
            processors[type] -= handler;
        }
    }
}
