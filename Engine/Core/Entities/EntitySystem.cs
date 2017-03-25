namespace Hippopotamus.Engine.Core.Entities
{
    public abstract class EntitySystem
    {
        public virtual void Start() {}
        public virtual void Update(GameLoopEventArgs args) {}
        public virtual void FixedUpdate(GameLoopEventArgs args) {}
        public virtual void Draw(GameLoopEventArgs args) {}
    }
}
