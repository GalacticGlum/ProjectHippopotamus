using System;
using System.Collections.Generic;
using Hippopotamus.Engine.Core.Exceptions;

namespace Hippopotamus.Engine.Core
{
    public static class EntitySystemManager
    {
        private static readonly Dictionary<Type, EntitySystem> systems = new Dictionary<Type, EntitySystem>();

        public static void Register<T>() where T : EntitySystem, new()
        {
            systems.Add(typeof(T), new T());
        }

        public static void Remove<T>() where T : EntitySystem
        {
            if (!systems.ContainsKey(typeof(T)))
            {
                throw new EntitySystemNotFoundException(typeof(T));
            }

            systems.Remove(typeof(T));
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
