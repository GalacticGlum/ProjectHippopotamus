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
    }
}
