using System.Collections.Generic;
using System.Linq;
using Ninject;

namespace Hippopotamus.Engine.Core.Entities
{
    public abstract class EntitySystem
    {
        protected EntityPool Pool { get; }

        protected EntitySystem()
        {
            Pool = DependencyInjector.Kernel.Get<EntityPool>();
        }

        protected HashSet<Entity> GetCompatibleEntities(EntityFilter filter)
        {
            return new HashSet<Entity>(Pool.Entities.Where(entity => entity.State != EntityState.Disabled && entity.DoesMatchFilter(filter)));
        }
    }
}
