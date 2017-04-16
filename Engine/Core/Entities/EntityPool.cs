using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hippopotamus.Engine.Core.Entities;

namespace Hippopotamus.Engine.Core
{
    public delegate void EntityChangedEventHandler(EntityChangedEventArgs args);
    public class EntityChangedEventArgs : EventArgs
    {
        public Entity Entity { get; }
        public EntityChangedEventArgs(Entity entity)
        {
            Entity = entity;
        }
    }

    public delegate void ComponentChangedEventHandler(ComponentChangedEventArgs args);
    public class ComponentChangedEventArgs : EventArgs
    {
        public Component Component { get; }
        public ComponentChangedEventArgs(Component component)
        {
            Component = component;
        }
    }

    public static class EntityPool
    {
        public static HashSet<Entity> Entities { get; }
        public static Stack<Entity> CachedEntities { get; }

        public static int Count => Entities.Count;

        public static event EntityChangedEventHandler EntityAdded;
        internal static void OnEntityAdded(Entity entity) { EntityAdded?.Invoke(new EntityChangedEventArgs(entity)); }

        public static event EntityChangedEventHandler EntityChanged;
        internal static void OnEntityChanged(Entity entity) { EntityChanged?.Invoke(new EntityChangedEventArgs(entity)); }

        public static event EntityChangedEventHandler EntityRemoved;
        internal static void OnEntityRemoved(Entity entity)
        {
            foreach (Component component in entity.Components.Values)
            {
                groups[component.GetType()].Remove(entity);
            }

            EntityRemoved?.Invoke(new EntityChangedEventArgs(entity));
        }

        public static event ComponentChangedEventHandler ComponentAdded;
        internal static void OnComponentAdded(Component component)
        {
            Type componentType = component.GetType();
            if (!groups.ContainsKey(componentType))
            {
                groups[componentType] = new HashSet<Entity>();
            }

            groups[componentType].Add(component.Entity);
            ComponentAdded?.Invoke(new ComponentChangedEventArgs(component));
        }

        public static event ComponentChangedEventHandler ComponentRemoved;
        internal static void OnComponentRemoved(Component component)
        {
            Type componentType = component.GetType();
            if (groups.ContainsKey(componentType))
            {
                groups[componentType].Remove(component.Entity);
            }

            ComponentRemoved?.Invoke(new ComponentChangedEventArgs(component));
        }

        private static readonly Dictionary<Type, HashSet<Entity>> groups;
        private const int entityCacheCap = 16384;

        static EntityPool()
        {
            Entities = new HashSet<Entity>();
            CachedEntities = new Stack<Entity>();         

            groups = new Dictionary<Type, HashSet<Entity>>();
        }

        public static Entity Create(string name, params Type[] componentsToAdd)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("The string: name provided was null or empty.");
            }

            Entity entity;
            if (CachedEntities.Any())
            {
                entity = CachedEntities.Pop();
                if (entity == null)
                {
                    Logger.Log("Engine", $"Entity of name: \"{name}\" not found.", LoggerVerbosity.Warning);
                    return null;
                }

                entity.Name = name;
                entity.State = EntityState.Enabled;
                entity.Transform = entity.AddComponent<Transform>();
            }
            else
            {
                entity = new Entity(name);
            }

            Entities.Add(entity);

            OnEntityAdded(entity);
            return entity;
        }

        internal static void Add(Entity entity)
        {
            if (entity.IsFree())
            {
                Entities.Add(entity);
            }
            else if (!entity.IsFree())
            {
                CachedEntities.Push(entity);
            }

            OnEntityAdded(entity);
        }

        public static void Destroy(Entity entity)
        {
            if (!entity.IsFree())
            {
                return;
            }

            if (!Entities.Contains(entity))
            {
                Logger.Log("Engine", $"Entity of name: \"{entity.Name}\" not found.", LoggerVerbosity.Warning);
                return;
            }

            Entities.Remove(entity);

            OnEntityRemoved(entity);

            if (CachedEntities.Count >= entityCacheCap) return;

            entity.Reset();
            CachedEntities.Push(entity);
        }

        public static bool Exists(string name)
        {
            return !string.IsNullOrEmpty(name) && Entities.Any(obj => obj.Name == name && Find(name).IsFree());
        }

        public static bool Exists(Entity entity)
        {
            if (entity == null)
            {
                return false;
            }

            return !string.IsNullOrEmpty(entity.Name) && Entities.Any(obj => obj == entity && entity.IsFree());
        }

        public static Entity Find(string name)
        {
            Entity entity = Entities.FirstOrDefault(obj => obj.Name == name);
            if(entity != null) return entity;

            Logger.Log("Engine", $"Entity of name: \"{name}\" not found.", LoggerVerbosity.Warning);
            return null;
        }

        internal static void ClearCache()
        {
            CachedEntities.Clear();
        }

        internal static void Clear()
        {
            Entities.Clear();
            groups.Clear();
        }

        public static HashSet<Entity> GetGroup(params Type[] types)
        {
            if (types.Length == 1)
            {
                return groups.ContainsKey(types[0]) ? groups[types[0]] : new HashSet<Entity>();
            }

            HashSet<Entity> result = new HashSet<Entity>();
            foreach (Type type in types)
            {
                if (groups.ContainsKey(type))
                {
                    result.UnionWith(groups[type]);
                }
            }

            return result;
        }
    }
}
