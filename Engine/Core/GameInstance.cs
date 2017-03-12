namespace Hippopotamus.Engine.Core
{
    public abstract class GameInstance
    {
        public GameEngine Context { get; internal set; }

        public virtual void Initialize() { }
        public virtual void Update(GameLoopEventArgs args) { }
        public virtual void Draw(GameLoopEventArgs args) { }
    }
}
