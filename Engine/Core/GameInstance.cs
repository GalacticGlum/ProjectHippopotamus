namespace Hippopotamus.Engine.Core
{
    public abstract class GameInstance
    {
        public GameEngine GameEngine { get; internal set; }

        public virtual void Initialize() { }
        public virtual void Update(object sender, GameLoopUpdateEventArgs args) { }
        public virtual void Draw(object sender, GameLoopDrawEventArgs args) { }
    }
}
