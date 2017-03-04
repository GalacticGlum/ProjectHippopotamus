using System;
using System.Collections.Generic;
using System.Linq;
using Hippopotamus.Engine.Core.Entities;
using Hippopotamus.Engine.Core.Exceptions;

namespace Hippopotamus.Engine.Core
{
    public delegate void EntityChangedEventHandler(object sender, EntityChangedEventArgs args);
    public class EntityChangedEventArgs : EventArgs
    {
        public EntityPool Pool { get; }
        public Entity Entity { get; }

        public EntityChangedEventArgs(EntityPool pool, Entity entity)
        {
            Pool = pool;
            Entity = entity;
        }
    }

    public delegate void ComponentChangedEventHandler(object sender, ComponentChangedEventArgs args);
    public class ComponentChangedEventArgs : EventArgs
    {
        public EntityPool Pool { get; }
        public Component Component { get; }

        public ComponentChangedEventArgs(EntityPool pool, Component component)
        {
            Pool = pool;
            Component = component;
        }
    }

    public class EntityPool
    {
        public string Name { get; set; }

        public HashSet<Entity> Entities { get; }
        public Stack<Entity> CachedEntities { get; }

        public event EntityChangedEventHandler EntityAdded;
        internal void OnEntityAdded(Entity entity) { EntityAdded?.Invoke(this, new EntityChangedEventArgs(this, entity)); }

        public event EntityChangedEventHandler EntityChanged;
        internal void OnEntityChanged(Entity entity) { EntityChanged?.Invoke(this, new EntityChangedEventArgs(this, entity)); }

        public event EntityChangedEventHandler EntityRemoved;
        internal void OnEntityRemoved(Entity entity)
        {
            foreach (Component component in entity.Components.Values)
            {
                groups[component.GetType()].Remove(entity);
            }

            EntityRemoved?.Invoke(this, new EntityChangedEventArgs(this, entity));
        }

        public event ComponentChangedEventHandler ComponentAdded;
        internal void OnComponentAdded(Component component)
        {
            Type componentType = component.GetType();
            if (!groups.ContainsKey(componentType))
            {
                groups[componentType] = new HashSet<Entity>();
            }

            groups[componentType].Add(component.Entity);
            ComponentAdded?.Invoke(this, new ComponentChangedEventArgs(this, component));
        }

        public event ComponentChangedEventHandler ComponentRemoved;
        internal void OnComponentRemoved(Component component)
        {
            Type componentType = component.GetType();
            if (groups.ContainsKey(componentType))
            {
                groups[componentType].Remove(component.Entity);
            }

            ComponentRemoved?.Invoke(this, new ComponentChangedEventArgs(this, component));
        }

        private readonly HashSet<string> usedEntityNames;
        private readonly Dictionary<Type, HashSet<Entity>> groups;

        private const int entityCacheCap = 16384;

        public EntityPool(string name)
        {
            Entities = new HashSet<Entity>();
            CachedEntities = new Stack<Entity>();

            usedEntityNames = new HashSet<string>();
            groups = new Dictionary<Type, HashSet<Entity>>();

            if (!string.IsNullOrEmpty(Name))
            {
                Name = name;
            }
        }

        public Entity Create(string name)
        {
            if (usedEntityNames.Contains(name))
            {
                throw new DuplicateEntityException(this, name);
            }

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
                    throw new EntityNotFoundException(this);
                }

                entity.Name = name;
                entity.Pool = this;
                entity.State = EntityState.Enabled;
                entity.Transform = entity.AddComponent<Transform>();
            }
            else
            {
                entity = new Entity(name, this);
            }

            Entities.Add(entity);
            usedEntityNames.Add(name);

            OnEntityAdded(entity);
            return entity;
        }

        internal void Add(Entity entity)
        {
            if (entity.IsUsable())
            {
                Entities.Add(entity);
            }
            else if (!entity.IsUsable())
            {
                CachedEntities.Push(entity);
            }

            OnEntityAdded(entity);
        }

        public void Destroy(Entity entity)
        {
            if (!entity.IsUsable())
            {
                return;
            }

            if (!Entities.Contains(entity))
            {
                throw new EntityNotFoundException(this);
            }

            usedEntityNames.Remove(entity.Name);
            Entities.Remove(entity);

            OnEntityRemoved(entity);

            if (CachedEntities.Count >= entityCacheCap) return;

            entity.Reset();
            CachedEntities.Push(entity);
        }

        public bool Exists(string name)
        {
            return !string.IsNullOrEmpty(name) && Entities.Any(obj => obj.Name == Name && Find(Name).IsUsable());
        }

        public bool Exists(Entity entity)
        {
            if (entity == null)
            {
                return false;
            }

            return !string.IsNullOrEmpty(entity.Name) && Entities.Any(obj => obj == entity && entity.IsUsable());
        }

        public Entity Find(string name)
        {
            Entity entity = Entities.FirstOrDefault(obj => obj.Name == name);
            if(entity != null) return entity;

            throw new EntityNotFoundException(this);
        }

        public void ClearCache()
        {
            CachedEntities.Clear();
        }

        public void Clear()
        {
            Entities.Clear();
            groups.Clear();
            usedEntityNames.Clear();
        }

        public HashSet<Entity> GetGroup(params Type[] types)
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
