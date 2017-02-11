using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.Core
{
    public static class SystemManager
    {
        private static readonly Dictionary<Type, System> systems = new Dictionary<Type, System>();

        internal static void Update(GameTime gameTime)
        {
            foreach (System system in systems.Values)
            {
                system.EarlyUpdate(gameTime);
                system.Update(gameTime);
                system.LateUpdate(gameTime);
            }
        }

        internal static void Update(GameObject gameObject, GameTime gameTime)
        {
            foreach (System system in systems.Values)
            {
                if (!system.Enabled || !system.Constraint.MeetsMaskCriteria(gameObject)) continue;

                system.GameLoop.Trigger(SystemGameLoopType.EarlyUpdate, gameTime, system.Constraint.GetMaskComponents(gameObject));
                system.GameLoop.Trigger(SystemGameLoopType.Update, gameTime, system.Constraint.GetMaskComponents(gameObject));
                system.GameLoop.Trigger(SystemGameLoopType.LateUpdate, gameTime, system.Constraint.GetMaskComponents(gameObject));
            }
        }

        internal static void Draw(SpriteBatch gameTime)
        {
            foreach (System system in systems.Values)
            {
                system.EarlyDraw(gameTime);
                system.Draw(gameTime);
                system.LateDraw(gameTime);
            }
        }

        internal static void Draw(GameObject gameObject, SpriteBatch spriteBatch)
        {
            foreach (System system in systems.Values)
            {
                if (!system.Enabled || !system.Constraint.MeetsMaskCriteria(gameObject)) continue;

                system.GameLoop.Trigger(SystemGameLoopType.EarlyDraw, spriteBatch, system.Constraint.GetMaskComponents(gameObject));
                system.GameLoop.Trigger(SystemGameLoopType.Draw, spriteBatch, system.Constraint.GetMaskComponents(gameObject));
                system.GameLoop.Trigger(SystemGameLoopType.LateDraw, spriteBatch, system.Constraint.GetMaskComponents(gameObject));
            }
        }

        public static void Add(Type type)
        {
            if (type.BaseType != typeof(System)) return;
            if (!systems.ContainsKey(type))
            {
                systems.Add(type, (System)Activator.CreateInstance(type));
            }
        }

        public static void Add<T>() where T : System, new()
        {
            if (!systems.ContainsKey(typeof(T)))
            {
                systems.Add(typeof(T), new T());
            }
        }

        public static void Remove(Type type)
        {
            if (type.BaseType != typeof(System)) return;
            if (systems.ContainsKey(type))
            {
                systems.Remove(type);
            }
        }

        public static void Remove<T>() where T : System, new()
        {
            if (systems.ContainsKey(typeof(T)))
            {
                systems.Remove(typeof(T));
            }
        }
    }
}
