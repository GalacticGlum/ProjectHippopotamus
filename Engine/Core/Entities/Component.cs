using Hippopotamus.Engine.Core.Entities;

namespace Hippopotamus.Engine.Core
{
    public abstract class Component 
    {
        public Entity Entity { get; internal set; }
        public Transform Transform => Entity.Transform;

        public virtual void Start() {}
    }
}
