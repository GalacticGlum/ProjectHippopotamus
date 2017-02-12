namespace Hippopotamus.Engine.Core
{
    public interface IUpdatable
    {
        void Update(object sender, GameLoopUpdateEventArgs args);
    }
}
