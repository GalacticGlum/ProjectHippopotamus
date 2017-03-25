using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hippopotamus.Engine.Core.Entities;
using Hippopotamus.Engine.Core.Exceptions;
using Microsoft.Xna.Framework;

namespace Hippopotamus.Engine.Core
{
    public static class EntitySystemManager
    {
        private static readonly Dictionary<Type, EntitySystem> systems = new Dictionary<Type, EntitySystem>();

        /// <summary>
        /// Does a full scan of all assemblies in the current app domain and registers any systems that need to be registered.
        /// </summary>
        internal static void Initialize()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    // If type is not a child (inherits) of EntitySystem then we don't have an business here.
                    if(type.BaseType != typeof(EntitySystem)) continue;

                    object[] attribute = type.GetCustomAttributes(typeof(StartupEntitySystem), false);
                    if (attribute.Length > 0)
                    {
                        Register(type);
                    }
                }
            }

            GameLoop.Register(GameLoopType.Update, Update);
            GameLoop.Register(GameLoopType.FixedUpdate, FixedUpdate);
            GameLoop.Register(GameLoopType.Draw, Draw);
        }

        private static void Update(GameLoopEventArgs args)
        {
            foreach (EntitySystem system in systems.Values)
            {
                system.Update(args);
            }
        }

        private static void FixedUpdate(GameLoopEventArgs args)
        {
            foreach (EntitySystem system in systems.Values)
            {
                system.FixedUpdate(args);
            }
        }

        private static void Draw(GameLoopEventArgs args)
        {
            foreach (EntitySystem system in systems.Values)
            {
                system.Draw(args);
            }
        }

        public static void Register(Type type)
        {
            if (type.BaseType != typeof(EntitySystem) || systems.ContainsKey(type)) return;

            EntitySystem system = (EntitySystem) Activator.CreateInstance(type);
            systems.Add(type, system);

            system.Start();
        }

        public static void Register<T>() where T : EntitySystem, new()
        {
            Register(typeof(T));
        }

        public static void Unregister(Type type)
        {
            if (!systems.ContainsKey(type))
            {
                throw new EntitySystemNotFoundException(type);
            }

            systems.Remove(type);
        }

        public static void Unregister<T>() where T : EntitySystem
        {
            Unregister(typeof(T));
        }
             
        public static T Get<T>() where T : EntitySystem, new()
        {
            if (!systems.ContainsKey(typeof(T)))
            {
                Register<T>();
            }

            return (T)systems[typeof(T)];
;        }
    }
}
