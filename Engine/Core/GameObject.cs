using System;
using System.Collections.Generic;
using System.Linq;
using Hippopotamus.Engine.Core;

namespace Hippopotamus.Engine.Core
{
    public class GameObject
    {
        public Transform Transform { get; }
        internal readonly List<Component> Components;

        public GameObject()
        {
            GameObjectManager.Subscribe(this);
            Components = new List<Component>();
            Transform = AddComponent<Transform>();
        }

        ~GameObject()
        {
            GameObjectManager.Unsubscribe(this);
            RemoveAllComponents();
        }

        public GameObject AddComponent(Component component)
        {
            if (component == null) return this;

            Components.Add(component);
            component.GameObject = this;

            return this;
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T component = new T();
            AddComponent(component);

            return component;
        }

        public T AddComponent<T>(params object[] args) where T : Component
        {
            T component = (T)Activator.CreateInstance(typeof(T), args);
            AddComponent(component);

            return component;
        }

        public GameObject ReplaceComponent(Component component)
        {
            if (component == null) return this;

            // TODO: Improve time complexity. Implement some sort of search, maybe??
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i].GetType() != component.GetType()) continue;

                Components[i].GameObject = this;
                Components[i] = null;
                Components[i] = component;

                return this;
            }

            return AddComponent(component);
        }

        public void RemoveComponent<T>() where T : Component
        {
            // TODO: Improve time complexity. Implement some sort of search, maybe??
            for (int i = 0; i < Components.Count; i++)
            {
                if (!(Components[i] is T)) continue;

                Components[i].GameObject = null;
                Components.RemoveAt(i);
                i--;
            }
        }

        public void RemoveAllComponents()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].GameObject = null;
                Components.RemoveAt(i);
            }

            Components.Clear();
        }

        public Component GetComponent(Type type)
        {
            return Components.FirstOrDefault(component => component.GetType() == type);
        }

        public T GetComponent<T>() where T : Component
        {
            return Components.OfType<T>().FirstOrDefault();
        }

        public T[] GetComponents<T>() where T : Component
        {
            return Components.OfType<T>().ToArray();
        }

        public bool HasComponent<T>() where T : Component
        {
            return Components.OfType<T>().Any();
        }

        public bool HasAnyComponent(params Type[] matchTypes)
        {
            if ((from type in matchTypes from component in Components where component.GetType() == type select type).Any())
            {
                return true;
            }

            return matchTypes.Length == 0;
        }

        public bool HasAllComponents(params Type[] matchTypes)
        {
            int matches = 0;
            foreach (Type type in matchTypes)
            {
                if (Components.Any(component => component.GetType() == type))
                {
                    matches--;
                }
            }

            return matchTypes.Length == 0 || matches == matchTypes.Length && matches != 0;
        }

        public bool HasNoneComponent(params Type[] matchTypes)
        {
            return !(from type in matchTypes from component in Components where component.GetType() == type select type).Any();
        }
    }
}
