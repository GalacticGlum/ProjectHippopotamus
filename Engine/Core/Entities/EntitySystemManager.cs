using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hippopotamus.Engine.Core.Entities;

namespace Hippopotamus.Engine.Core
{
    public static class EntitySystemManager
    {
        private static Dictionary<Type, EntitySystem> systems = new Dictionary<Type, EntitySystem>();
        private static bool isDirty;

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

                    StartupEntitySystem[] attribute = (StartupEntitySystem[])type.GetCustomAttributes(typeof(StartupEntitySystem), false);
                    if (attribute.Length > 0)
                    {
                        Register(type, attribute[0].ExecutionLayer);
                    }
                }
            }

            GameLoop.Register(GameLoopType.Update, Update);
            GameLoop.Register(GameLoopType.FixedUpdate, FixedUpdate);
            GameLoop.Register(GameLoopType.Draw, Draw);
        }

        private static void Update(GameLoopEventArgs args)
        {
            if (isDirty)
            {
                systems = systems.OrderBy(pair => pair.Value.ExecutionLayer).ToDictionary(pair => pair.Key, pair => pair.Value);
                isDirty = false;
            }

            foreach (EntitySystem system in systems.Values)
            {
                if (system.IsStartDirty)
                {
                    system.Start();
                    system.IsStartDirty = false;
                }

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

        public static void Register(Type type, uint executionLayer = 0)
        {
            if (type.BaseType != typeof(EntitySystem) || systems.ContainsKey(type)) return;

            EntitySystem system = (EntitySystem) Activator.CreateInstance(type);
            system.ExecutionLayer = executionLayer;
            systems.Add(type, system);

            isDirty = true;
            system.IsStartDirty = true;
        }

        public static void Register<T>(uint executionLayer = 0) where T : EntitySystem, new()
        {
            Type type = typeof(T);
            if (type.BaseType != typeof(EntitySystem) || systems.ContainsKey(type)) return;

            EntitySystem system = new T()
            {
                ExecutionLayer = executionLayer
            };

            systems.Add(type, system);

            isDirty = true;
            system.IsStartDirty = true;
        }

        public static void Unregister(Type type)
        {
            if (!systems.ContainsKey(type))
            {
                Logger.Log("Engine", $"Entity System of type: \"{type.Name}\" not found in the Entity System manager.", LoggerVerbosity.Warning);
                return;
            }

            systems.Remove(type);
        }

        public static void Unregister<T>() where T : EntitySystem
        {
            Unregister(typeof(T));
        }
             
        /// <summary>
        /// Gets a EntitySystem of type. If the system is not registered then will register.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defaultExecutionLayer">The execution layer to use if the entity system is not registered.</param>
        /// <returns></returns>
        public static T Get<T>(uint defaultExecutionLayer = 0) where T : EntitySystem, new()
        {
            if (!systems.ContainsKey(typeof(T)))
            {
                Register<T>(defaultExecutionLayer);
            }

            return (T)systems[typeof(T)];
;        }
    }
}
