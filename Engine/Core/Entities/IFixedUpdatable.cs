namespace Hippopotamus.Engine.Core.Entities
{
    public interface IFixedUpdatable
    {   
        void FixedUpdate(object sender, GameLoopFixedUpdateEventArgs args);
    }
}
