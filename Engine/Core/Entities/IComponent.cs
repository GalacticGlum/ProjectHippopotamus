namespace Hippopotamus.Engine.Core.Entities
{
    public interface IComponent
    {
        Entity Entity { get; set; }
        Transform Transform { get; }
    }
}
