namespace Hippopotamus.Engine.Core.Entities
{
    public abstract class EntitySystem
    {
        internal uint ExecutionLayer { get; set; }
        internal bool IsStartDirty { get; set; }

        public virtual void Start() {}
        public virtual void Update(GameLoopEventArgs args) {}
        public virtual void FixedUpdate(GameLoopEventArgs args) {}
        public virtual void Draw(GameLoopEventArgs args) {}
    }
}
