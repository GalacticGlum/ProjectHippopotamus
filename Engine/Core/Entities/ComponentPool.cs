using System;
using System.Collections.Generic;
using Hippopotamus.Engine.Utilities;

namespace Hippopotamus.Engine.Core.Entities
{
    internal static class ComponentPool
    {
        public static int AllocationCount { get; private set; }
        public static int PoolCount => pools.Count;

        private static readonly Dictionary<Type, Queue<Component>> pools;

        static ComponentPool()
        {
            pools = new Dictionary<Type, Queue<Component>>();
        }

        public static void Reclaim(Type componentType, Component component)
        {
            if (!componentType.IsComponent()) return;
            if (!pools.ContainsKey(componentType))
            {
                pools.Add(componentType, new Queue<Component>());
            }
      
            pools[componentType].Enqueue(component);
        }

        public static T Get<T>() where T : Component, new()
        {
            Type componentType = typeof(T);
            if (!pools.ContainsKey(componentType))
            {
                pools.Add(componentType, new Queue<Component>());
            }

            T component;
            if (pools[componentType].Count > 0)
            {
                component = (T)pools[componentType].Dequeue();
                component.Reset();
            }
            else
            {
                component = new T();
                AllocationCount++;
            }

            return component;
        }
    }
}
