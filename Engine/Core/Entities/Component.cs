using Hippopotamus.Engine.Core.Entities;

namespace Hippopotamus.Engine.Core
{
    public abstract class Component 
    {
        public Entity Entity { get; internal set; }
        public Transform Transform => Entity.Transform;

        public abstract void Reset();
        public virtual void Start() {}
    }
}
