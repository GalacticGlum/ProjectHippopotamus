namespace Hippopotamus.Engine.Core.Entities
{
    public interface IFixedUpdatable
    {   
        void FixedUpdate(GameLoopFixedUpdateEventArgs args);
    }
}
