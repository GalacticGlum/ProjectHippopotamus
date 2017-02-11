namespace Hippopotamus.Engine.Core
{
    // A component is a small data object requiring only a gameobject.
    public class Component
    {
        public GameObject GameObject { get; internal set; }
        public Transform Transform => GameObject.Transform;
    }
}
