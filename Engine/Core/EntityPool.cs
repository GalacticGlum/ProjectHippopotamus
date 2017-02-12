﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hippopotamus.Engine.Core.Exceptions;

namespace Hippopotamus.Engine.Core
{
    public delegate void EntityPoolChangedEventHandler(object sender, EntityPoolChangedEventArgs args);
    public class EntityPoolChangedEventArgs : EventArgs
    {
        public EntityPool Pool { get; }
        public Entity Entity { get; }

        public EntityPoolChangedEventArgs(EntityPool pool, Entity entity)
        {
            Pool = pool;
            Entity = entity;
        }
    }

    public class EntityPool
    {
        public string Name { get; set; }

        public List<Entity> Entities { get; }
        public Stack<Entity> CachedEntities { get; }

        public event EntityPoolChangedEventHandler EntityAdded;

        public void OnEntityAdded(EntityPoolChangedEventArgs args)
        {
            args.Entity.Changed += (sender, arg) =>
            {
                OnEntityChanged(new EntityPoolChangedEventArgs(this, arg.Entity));
            };

            EntityAdded?.Invoke(this, args);
        }

        public event EntityPoolChangedEventHandler EntityChanged;
        public void OnEntityChanged(EntityPoolChangedEventArgs args) { EntityChanged?.Invoke(this, args); }

        public event EntityPoolChangedEventHandler EntityRemoved;
        public void OnEntityRemoved(EntityPoolChangedEventArgs args) { EntityRemoved?.Invoke(this, args); }

        public event EntityPoolChangedEventHandler ComponentAdded;
        internal void OnComponentAdded(Entity entity) { ComponentAdded?.Invoke(this, new EntityPoolChangedEventArgs(this, entity)); }

        public event EntityPoolChangedEventHandler ComponentRemoved;
        internal void OnComponentRemoved(Entity entity) { ComponentRemoved?.Invoke(this, new EntityPoolChangedEventArgs(this, entity)); }

        private const int entityCacheCap = 30;

        public EntityPool(string name)
        {
            Entities = new List<Entity>();
            CachedEntities = new Stack<Entity>();

            if (!string.IsNullOrEmpty(Name))
            {
                Name = name;
            }
        }

        public Entity Create(string name)
        {
            if (Entities.Any(obj => obj.Name == name))
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

                Entities.Add(entity);
            }
            else
            {
                entity = new Entity(name, this);
                Entities.Add(entity);
            }

            OnEntityAdded(new EntityPoolChangedEventArgs(this, entity));
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

            OnEntityAdded(new EntityPoolChangedEventArgs(this, entity));
        }

        public void Destroy(ref Entity entity)
        {
            if (!entity.IsUsable())
            {
                return;
            }

            if (!Entities.Contains(entity))
            {
                throw new EntityNotFoundException(this);
            }

            if (CachedEntities.Count < entityCacheCap)
            {
                entity.Reset();
                CachedEntities.Push(entity);
            }

            Entities.Remove(entity);
            OnEntityRemoved(new EntityPoolChangedEventArgs(this, entity));

            entity = null;
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

        public void ClearEntities()
        {
            Entities.Clear();
        }
    }
}
