using System;
using System.Collections.Generic;
using System.Linq;
using Hippopotamus.Engine.Utilities;
using Ninject;

namespace Hippopotamus.Engine.Core
{
    public abstract class EntitySystem
    {
        public EntityPool Pool { get; private set; }
        public HashSet<Entity> CompatibleEntities { get; private set; }

        protected HashSet<Type> CompatibleTypes { get; set; }
        private bool isDirty;

        protected EntitySystem(params Type[] compatibleTypes)
        {
            if (compatibleTypes.Any(type => !type.IsComponent()))
            {
                throw new ArgumentException("Type passed into EntitySystem was not a child of Component.");
            }

            CompatibleTypes = new HashSet<Type>(compatibleTypes);
            Pool = DependencyInjector.Kernel.Get<EntityPool>();

            CompatibleEntities = GetCompatibleEntities();

            Pool.ComponentAdded += OnEntityPoolChanged;
            Pool.ComponentRemoved += OnEntityPoolChanged;

            Pool.EntityAdded += OnEntityPoolChanged;
            Pool.EntityChanged += OnEntityPoolChanged;
            Pool.EntityRemoved += OnEntityPoolChanged;

            DependencyInjector.Kernel.Get<GameEngine>().GameLoop.Register((UpdateGameLoopEventHandler)((sender, args) => CheckEntityCompatibility()));
        }

        private void CheckEntityCompatibility()
        {
            if (!isDirty) return;

            CompatibleEntities = GetCompatibleEntities();
            isDirty = false;
        }

        public void AddCompatibility(Type type)
        {
            if (!type.IsComponent())
            {
                throw new ArgumentException($"Type: \"{type.Name} is not a child of Component");
            }

            CompatibleTypes.Add(type);
            CompatibleEntities = GetCompatibleEntities();
        }

        private void OnEntityPoolChanged(object sender, EntityPoolChangedEventArgs args)
        {
            Pool = args.Pool;
            isDirty = true;
        }

        protected HashSet<Entity> GetCompatibleEntities()
        {
            return new HashSet<Entity>(Pool.Entities.Where(entity => entity.State != EntityState.Disabled && entity.HasComponents(CompatibleTypes)));
        }
    }
}
