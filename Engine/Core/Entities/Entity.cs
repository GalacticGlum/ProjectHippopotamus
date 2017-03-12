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
                EntityPool.OnEntityChanged(this);
            }
        }

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
                EntityPool.OnEntityChanged(this);
            }
        }

        public bool Enabled
        {
            get
            {
                if (!IsFree()) return false;
                return State == EntityState.Enabled;
            }
        }

        internal Dictionary<Type, Component> Components { get; }
        internal List<Entity> Children { get; }

        internal Entity(string name)
        {
            Name = name;

            Components = new Dictionary<Type, Component>();
            Children = new List<Entity>();

            State = EntityState.Enabled;
            Transform = AddComponent<Transform>();
        }

        public void Enable()
        {
            if (!IsFree())
            {
                return;
            }

            State = EntityState.Enabled;
        }

        public void Disable()
        {
            if (!IsFree())
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
            if (!IsFree())
            {
                return;
            }

            State = State == EntityState.Enabled ? EntityState.Disabled : EntityState.Enabled;
        }

        public Component AddComponent(Component component)
        {
            if (!IsFree()) return null;
            if (HasComponent(component.GetType()))
            {
                throw new ComponentExistsException(this, component.GetType());
            }

            component.Entity = this;    
            Components.Add(component.GetType(), component);
     
            EntityPool.OnComponentAdded(component);

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
                    GameLoop.Register(GameLoopType.Update, updatable.Update);
                }
            }

            if (!interfaces.Contains(typeof(IFixedUpdatable))) return component;

            IFixedUpdatable fixedUpdatable = component as IFixedUpdatable;
            if (fixedUpdatable != null)
            {
                GameLoop.Register(GameLoopType.FixedUpdate, fixedUpdatable.FixedUpdate);
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
            if (!IsFree()) return;
            foreach (Component component in components)
            {
                AddComponent(component);
            }
        }

        public void AddComponents(params Component[] components)
        {
            if (!IsFree()) return;
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
            if (!IsFree()) return;
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
                    GameLoop.Unregister(GameLoopType.Update, updatable.Update);
                }
            }

            if (interfaces.Contains(typeof(IFixedUpdatable)))
            {
                IFixedUpdatable fixedUpdatable = component as IFixedUpdatable;
                if (fixedUpdatable != null)
                {
                    GameLoop.Unregister(GameLoopType.FixedUpdate, fixedUpdatable.FixedUpdate);
                }
            }

            Components.Remove(componentType);
            EntityPool.OnComponentRemoved(component);
        }

        public T GetComponent<T>() where T : Component
        {
            if (!IsFree())
            {
                return default(T);
            }

            Component component;
            if (Components.TryGetValue(typeof(T), out component)) return (T) component;

            throw new ComponentNotFoundException(this, typeof(T));
        }

        public Component GetComponent(Type componentType)
        {
            if (!IsFree())
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
        /// <param entityName="component"></param>
        /// <param entityName="destination"></param>
        public void TransferComponent(Component component, Entity destination)
        {
            if (!IsFree())
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
            if (!IsFree())
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
            if (!IsFree())
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

        public void Destroy()
        {
            EntityPool.Destroy(this);
        }

        internal void Reset()
        {
            if (!IsFree())
            {
                return;
            }

            RemoveAllComponents();

            foreach (Entity entity in Children)
            {
                Entity child = entity;
                EntityPool.Destroy(child);
            }

            Children.Clear();

            Name = string.Empty;
            State = EntityState.Cached;
        }

        /// <summary>
        /// Creates and returns a new entity as a child of this entity.
        /// </summary>
        /// <param entityName="entityName"></param>
        /// <param entityName="inheritComponents">Copy this entity's components to the child.</param>
        /// <returns></returns>
        public Entity CreateChild(string entityName, bool inheritComponents = false)
        {
            if (!IsFree())
            {
                return null;
            }

            Entity child = EntityPool.Create(entityName);
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
            if (!IsFree())
            {
                return null;
            }

            child.Parent = this;
            Children.Add(child);

            return child;
        }

        public Entity GetChild(string childName)
        {
            return !IsFree() ? null : Children.FirstOrDefault(child => child.Name == childName);
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

        public bool IsFree()
        {
            return State != EntityState.Cached;
        }
    }
}
