using System;
using System.Collections.Generic;
using System.Linq;
using Hippopotamus.Engine.Core.Entities;
using Hippopotamus.Engine.Core.Exceptions;
using Hippopotamus.Engine.Utilities;
using IStartable = Hippopotamus.Engine.Core.Entities.IStartable;

namespace Hippopotamus.Engine.Core
{
    public sealed class Entity
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                Pool?.OnEntityChanged(this);
            }
        }

        public EntityPool Pool { get; internal set; }

        public Transform Transform { get; internal set; }
        public Entity Parent { get; private set; }
        public Entity Root
        {
            get
            {
                if (Parent == null) return this;
                Entity parent = Parent;
                while (parent != null)
                {
                    if (parent.Parent == null)
                    {
                        return parent;
                    }

                    parent = parent.Parent;
                }

                throw new Exception($"Entity: \"{Name}\" has no root entity!");
            }
        }

        private EntityState state;
        public EntityState State
        {
            get { return state; }
            set
            {
                state = value;
                Pool?.OnEntityChanged(this);
            }
        }

        public bool Enabled
        {
            get
            {
                if (!IsUsable()) return false;
                return State == EntityState.Enabled;
            }
        }

        internal Dictionary<Type, Component> Components { get; }
        internal List<Entity> Children { get; }

        internal Entity(string name, EntityPool pool)
        {
            if (pool == null)
            {
                throw new InvalidEntityPoolException(this);
            }

            Pool = pool;
            Name = name;

            Components = new Dictionary<Type, Component>();
            Children = new List<Entity>();

            State = EntityState.Enabled;
            Transform = AddComponent<Transform>();
        }

        public void Enable()
        {
            if (!IsUsable())
            {
                return;
            }

            State = EntityState.Enabled;
        }

        public void Disable()
        {
            if (!IsUsable())
            {
                return;
            }

            State = EntityState.Disabled;
        }

        /// <summary>
        /// Toggles the entity on or off.
        /// </summary>
        public void Toggle()
        {
            if (!IsUsable())
            {
                return;
            }

            State = State == EntityState.Enabled ? EntityState.Disabled : EntityState.Enabled;
        }

        public Component AddComponent(Component component)
        {
            if (!IsUsable()) return null;
            if (HasComponent(component.GetType()))
            {
                throw new ComponentExistsException(this, component.GetType());
            }

            component.Entity = this;    
            Components.Add(component.GetType(), component);
     
            Pool.OnComponentAdded(component);

            Type[] interfaces = component.GetType().GetInterfaces();
            if (interfaces.Contains(typeof(IStartable)))
            {
                IStartable startable = component as IStartable;
                startable?.Start();
            }

            if (interfaces.Contains(typeof(IUpdatable)))
            {
                IUpdatable updatable = component as IUpdatable;
                if (updatable != null)
                {
                    GameLoop.Register(updatable.Update);
                }
            }

            if (!interfaces.Contains(typeof(IFixedUpdatable))) return component;

            IFixedUpdatable fixedUpdatable = component as IFixedUpdatable;
            if (fixedUpdatable != null)
            {
                GameLoop.Register(fixedUpdatable.FixedUpdate);
            }

            return component;
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T component = new T();
            AddComponent(component);
            return component;
        }

        public void AddComponents(IEnumerable<Component> components)
        {
            if (!IsUsable()) return;
            foreach (Component component in components)
            {
                AddComponent(component);
            }
        }

        public void AddComponents(params Component[] components)
        {
            if (!IsUsable()) return;
            foreach (Component component in components)
            {
                AddComponent(component);
            }
        }

        public void RemoveComponent<T>() where T : Component
        {
            RemoveComponent(typeof(T));
        }

        public void RemoveComponent(Type componentType)
        {
            if (!IsUsable()) return;
            if (!componentType.IsComponent())
            {
                throw new ArgumentException($"The type \"{componentType.Name}\" is not a child of Component!");
            }

            if (!HasComponent(componentType))
            {
                throw new ComponentNotFoundException(this, componentType);
            }

            Component component = GetComponent(componentType);
            Type[] interfaces = componentType.GetInterfaces();
            if (interfaces.Contains(typeof(IUpdatable)))
            {
                IUpdatable updatable = component as IUpdatable;
                if (updatable != null)
                {
                    GameLoop.Unregister(updatable.Update);
                }
            }

            if (interfaces.Contains(typeof(IFixedUpdatable)))
            {
                IFixedUpdatable fixedUpdatable = component as IFixedUpdatable;
                if (fixedUpdatable != null)
                {
                    GameLoop.Unregister(fixedUpdatable.FixedUpdate);
                }
            }

            Components.Remove(componentType);
            Pool.OnComponentRemoved(component);
        }

        public T GetComponent<T>() where T : Component
        {
            if (!IsUsable())
            {
                return default(T);
            }

            Component component;
            if (Components.TryGetValue(typeof(T), out component)) return (T) component;

            throw new ComponentNotFoundException(this, typeof(T));
        }

        public Component GetComponent(Type componentType)
        {
            if (!IsUsable())
            {
                return null;
            }

            if (!componentType.IsComponent())
            {
                throw new ArgumentException($"The type \"{componentType.Name}\" is not a child of Component!");
            }

            Component component;
            if (Components.TryGetValue(componentType, out component)) return component;

            throw new ComponentNotFoundException(this, componentType);
        }

        /// <summary>
        /// Moves a component from this entity
        ///  to another.
        /// If the destination object or component is null then throw a ComponentNotFoundException.
        /// Otherwise, add the component to the destination object and remove it from this.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="destination"></param>
        public void TransferComponent(Component component, Entity destination)
        {
            if (!IsUsable())
            {
                return;
            }

            Type componentType = component.GetType();
            if (component == null && !HasComponent(componentType))
            {
                throw new ComponentNotFoundException(this, componentType);
            }

            destination.AddComponent(component);
            Components.Remove(component.GetType());
        }

        public bool HasComponent<TComponent>() where TComponent : Component
        {
            return HasComponent(typeof(TComponent));
        }

        public bool HasComponent(Type componentType)
        {
            if (!IsUsable())
            {
                return false;
            }

            if (!componentType.IsComponent())
            {
                throw new ArgumentException($"The type \"{componentType.Name}\" is not a child of Component!");
            }

            return Components.ContainsKey(componentType);
        }

        public void RemoveAllComponents()
        {
            if (!IsUsable())
            {
                return;
            }

            List<Type> keys = new List<Type>(Components.Keys);
            foreach (Type type in keys)
            {
                RemoveComponent(type);
            }

            Components.Clear();
        }

        public void Reset()
        {
            if (!IsUsable())
            {
                return;
            }

            RemoveAllComponents();

            foreach (Entity entity in Children)
            {
                Entity child = entity;
                Pool.Destroy(child);
            }

            Children.Clear();

            Name = string.Empty;
            Pool = null;
            State = EntityState.Cached;
        }

        /// <summary>
        /// Moves this entity from it's current object pool to the destination pool.
        /// If the destination pool is null then throw a NullEntityPoolException.
        /// Otherwise, add the entity to the destination pool and remove it from it's current pool.
        /// </summary>
        /// <param name="destination"></param>
        public void Transfer(EntityPool destination)
        {
            if (Pool == null)
            {
                throw new NullEntityPoolException(Pool);
            }

            destination.Add(this);
            Entity self = this;
            Pool.Destroy(self);
            Pool = destination;
        }

        /// <summary>
        /// Creates and returns a new entity as a child of this entity.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="inheritComponents">Copy this entity's components to the child.</param>
        /// <returns></returns>
        public Entity CreateChild(string name, bool inheritComponents = false)
        {
            if (!IsUsable())
            {
                return null;
            }

            Entity child = Pool.Create(name);
            child.Parent = this;

            if (inheritComponents)
            {
                child.AddComponents(Components.Values);
            }

            Children.Add(child);
            return child;
        }

        public Entity AddChild(Entity child)
        {
            if (!IsUsable())
            {
                return null;
            }

            child.Parent = this;
            Children.Add(child);

            return child;
        }

        public Entity GetChild(string childName)
        {
            return !IsUsable() ? null : Children.FirstOrDefault(child => child.Name == childName);
        }

        /// <summary>
        /// Returns a tree of this entity.
        /// </summary>
        public IEnumerable<Entity> GetTree()
        {
            Func<Entity, IEnumerable<Entity>> selector = entity => entity.Children;
            Stack<Entity> treeStack = new Stack<Entity>(Children);
            while (treeStack.Any())
            {
                Entity next = treeStack.Pop();
                yield return next;
                foreach (Entity entity in selector(next))
                {
                    treeStack.Push(entity);
                }
            }
        }

        public bool IsUsable()
        {
            return State != EntityState.Cached;
        }
    }
}
