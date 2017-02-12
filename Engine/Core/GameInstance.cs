namespace Hippopotamus.Engine.Core
{
    public abstract class GameInstance
    {
        public GameEngine GameEngine { get; internal set; }

        public abstract void Initialize();
        public abstract void Update(object sender, GameLoopUpdateEventArgs args);
        public abstract void Draw(object sender, GameLoopDrawEventArgs args);
        public abstract void Dispose();
    }
}
